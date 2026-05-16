# Kế hoạch Dự án: IronIQ

## Context
Mobile app (iOS + Android) quản lý lịch tập cá nhân, tích hợp AI Coach. Solo dev. React Native + Expo / ASP.NET Core 9 / PostgreSQL / Claude API.

---

## Tech Stack

| Thành phần | Lựa chọn |
|---|---|
| Mobile | React Native + Expo + TypeScript |
| Backend | ASP.NET Core 9 Web API |
| Database | PostgreSQL (EF Core) |
| AI | Claude API (Anthropic SDK) |
| Auth | JWT + Refresh Token |
| Local Storage | expo-sqlite + MMKV (Guest mode offline) |
| i18n | i18next + react-i18next (Vi + En) |
| Subscription | RevenueCat |
| Ads | Google AdMob (Rewarded) |
| UI | NativeWind (Tailwind) + React Native Paper |
| State | Zustand (client) + TanStack Query (server) |
| Theme | Dark / Light mode (NativeWind `dark:`) |

---

## BACKEND — Kiến trúc & Pattern

### Nguyên tắc chọn lựa
- **Clean Architecture** — tách rõ Domain, Application, Infrastructure, API
- **CQRS + MediatR** — mọi business logic đi qua Commands/Queries, không viết logic trong Controller
- **Result Pattern** — không dùng exception cho business flow, trả về `Result<T>` tường minh
- **Domain Events** — loose coupling giữa các aggregate (vd: buổi tập hoàn thành → tự động cập nhật streak → kiểm tra achievement)

### Cấu trúc Thư mục Backend

```
src/
├── IronIQ.Domain/               # Không phụ thuộc gì cả
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Exercise.cs
│   │   ├── WorkoutPlan.cs
│   │   ├── WorkoutSession.cs
│   │   └── CoinTransaction.cs
│   ├── ValueObjects/
│   │   ├── UserProfile.cs        # Height, Weight, Goal, FitnessLevel
│   │   └── CoinBalance.cs
│   ├── Events/                   # Domain Events
│   │   ├── WorkoutSessionCompletedEvent.cs
│   │   └── AchievementUnlockedEvent.cs
│   ├── Enums/
│   │   ├── FitnessGoal.cs        # LoseWeight, BuildMuscle, Maintain
│   │   ├── FitnessLevel.cs       # Beginner, Intermediate, Advanced
│   │   └── SubscriptionTier.cs   # Free, Premium
│   └── Interfaces/
│       └── IRepository.cs        # Generic interface

├── IronIQ.Application/          # Chỉ phụ thuộc Domain
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── Commands/Register/
│   │   │   ├── Commands/Login/
│   │   │   └── Commands/RefreshToken/
│   │   ├── Exercises/
│   │   │   ├── Commands/CreateExercise/
│   │   │   ├── Queries/GetExercises/     # Có filter, search, pagination
│   │   │   └── Queries/GetExerciseById/
│   │   ├── WorkoutPlans/
│   │   │   ├── Commands/CreateWorkoutPlan/
│   │   │   ├── Commands/GenerateAIPlan/  # Gọi Claude API
│   │   │   ├── Commands/UpdateWorkoutPlan/
│   │   │   └── Queries/GetMyPlans/
│   │   ├── WorkoutSessions/
│   │   │   ├── Commands/StartSession/
│   │   │   ├── Commands/LogExercise/
│   │   │   ├── Commands/CompleteSession/ # Trigger Domain Event
│   │   │   └── Queries/GetSessionHistory/
│   │   ├── Users/
│   │   │   ├── Commands/UpdateProfile/
│   │   │   └── Queries/GetMyProfile/
│   │   └── Coins/
│   │       ├── Commands/EarnFromAd/
│   │       ├── Commands/SpendCoins/
│   │       └── Queries/GetBalance/
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── LoggingBehavior.cs        # Log mọi Command/Query
│   │   │   ├── ValidationBehavior.cs     # FluentValidation pipeline
│   │   │   └── TransactionBehavior.cs    # Wrap Command trong DB transaction
│   │   ├── Interfaces/
│   │   │   ├── ICurrentUserService.cs    # Lấy userId từ JWT context
│   │   │   ├── IAIService.cs             # Contract với Claude
│   │   │   └── ISubscriptionService.cs  # Contract với RevenueCat
│   │   ├── Models/
│   │   │   └── Result.cs                 # Result<T> pattern
│   │   └── Mappings/
│   │       └── MappingProfile.cs         # AutoMapper config

├── IronIQ.Infrastructure/       # Phụ thuộc Application + external libs
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Repositories/
│   │   └── Configurations/       # EF Fluent API configs
│   ├── External/
│   │   ├── Claude/
│   │   │   ├── ClaudeAIService.cs       # Implement IAIService
│   │   │   └── PromptBuilder.cs         # Build prompt từ user profile
│   │   ├── RevenueCat/
│   │   │   └── RevenueCatService.cs
│   │   └── AdMob/
│   │       └── AdRewardValidator.cs     # Validate server-side reward
│   └── DependencyInjection.cs    # Wiring tất cả services

└── IronIQ.API/                  # Entry point
    ├── Controllers/
    │   ├── AuthController.cs
    │   ├── ExercisesController.cs
    │   ├── WorkoutPlansController.cs
    │   ├── WorkoutSessionsController.cs
    │   ├── UsersController.cs
    │   └── CoinsController.cs
    ├── Middleware/
    │   ├── ExceptionHandlingMiddleware.cs  # Global error handler
    │   └── LanguageMiddleware.cs           # Accept-Language → i18n
    └── Program.cs
```

### Luồng xử lý chuẩn (mọi feature đều theo pattern này)

```
HTTP Request
  → Controller (chỉ nhận request, gọi Mediator)
    → MediatR Pipeline:
        1. LoggingBehavior
        2. ValidationBehavior (FluentValidation)
        3. TransactionBehavior
        → Command/Query Handler
          → Domain logic
          → Repository / External Service
          ← Result<T>
    ← Result<T>
  ← HTTP Response (200 / 400 / 401 / 404 / 500)
```

### Domain Events Flow (ví dụ hoàn thành buổi tập)

```
CompleteSessionCommand
  → WorkoutSession.Complete()        # Entity raise Domain Event
  → WorkoutSessionCompletedEvent
    ├── UpdateStreakHandler           # Cập nhật streak
    ├── CheckAchievementsHandler     # Kiểm tra badge mới
    └── AutoProgressionHandler       # Gợi ý tăng tạ (Phase 3)
```

---

## FRONTEND — Cấu trúc & Quản lý

### Kiến trúc

- **Expo Router** (file-based routing) — định tuyến theo cấu trúc thư mục `app/`
- **Feature-first structure** — mỗi feature tự quản lý API, hooks, types của mình
- **Server state** qua TanStack Query, **client state** qua Zustand
- **Dark/Light mode** dùng NativeWind `dark:` prefix + `useColorScheme` + user override

### Cấu trúc Thư mục Frontend

```
src/
├── app/                          # Expo Router screens
│   ├── (auth)/                   # Public routes (chưa đăng nhập)
│   │   ├── _layout.tsx
│   │   ├── login.tsx
│   │   └── register.tsx
│   ├── (guest)/                  # Guest mode routes
│   │   └── onboarding.tsx
│   ├── (tabs)/                   # Bottom tab navigation (đã đăng nhập)
│   │   ├── _layout.tsx           # Tab bar config
│   │   ├── index.tsx             # Home — lịch tập hôm nay
│   │   ├── explore.tsx           # Thư viện bài tập
│   │   ├── progress.tsx          # Biểu đồ tiến trình
│   │   └── profile.tsx           # Hồ sơ + cài đặt
│   ├── workout/
│   │   ├── [id].tsx              # Session đang tập (timer, log sets)
│   │   ├── create.tsx            # Tạo lịch tập thủ công
│   │   └── summary.tsx           # Tóm tắt sau buổi tập
│   ├── ai-coach/
│   │   ├── index.tsx             # Chat với AI Coach
│   │   └── generate.tsx          # Tạo lịch tập bằng AI
│   └── _layout.tsx               # Root layout (theme, auth guard)

├── features/                     # Business logic theo feature
│   ├── auth/
│   │   ├── api.ts                # login(), register(), refresh()
│   │   ├── hooks.ts              # useAuth(), useLogin()
│   │   ├── store.ts              # Zustand: tokens, user
│   │   └── types.ts
│   ├── exercises/
│   │   ├── api.ts                # getExercises(), createExercise()
│   │   ├── hooks.ts              # useExercises(), useExerciseSearch()
│   │   ├── query-keys.ts         # exerciseKeys factory
│   │   └── types.ts
│   ├── workout-plans/
│   │   ├── api.ts
│   │   ├── hooks.ts
│   │   ├── query-keys.ts
│   │   └── types.ts
│   ├── workout-sessions/
│   │   ├── api.ts
│   │   ├── hooks.ts
│   │   ├── store.ts              # Zustand: active session state
│   │   └── types.ts
│   ├── progress/
│   │   ├── api.ts
│   │   ├── hooks.ts
│   │   └── types.ts
│   ├── ai-coach/
│   │   ├── api.ts
│   │   ├── hooks.ts
│   │   ├── store.ts              # Zustand: conversation history
│   │   └── types.ts
│   └── coins/
│       ├── api.ts
│       ├── hooks.ts
│       └── types.ts

├── components/                   # Shared components (không chứa business logic)
│   ├── ui/                       # Primitive, dùng mọi nơi
│   │   ├── Button.tsx
│   │   ├── Input.tsx
│   │   ├── Card.tsx
│   │   ├── Text.tsx              # Theme-aware, hỗ trợ i18n
│   │   ├── Badge.tsx
│   │   └── Skeleton.tsx          # Loading placeholder
│   ├── charts/
│   │   ├── WeightChart.tsx
│   │   └── StrengthChart.tsx
│   ├── workout/
│   │   ├── ExerciseCard.tsx
│   │   ├── SetRow.tsx            # Row nhập sets/reps/weight
│   │   ├── RestTimer.tsx         # Đồng hồ nghỉ giữa hiệp
│   │   └── WorkoutDayCard.tsx
│   └── layout/
│       ├── ScreenWrapper.tsx     # Safe area + padding chuẩn
│       └── Header.tsx

├── lib/                          # Utilities thuần túy
│   ├── api.ts                    # Axios instance + JWT interceptor + auto refresh
│   ├── query-client.ts           # TanStack Query global config
│   ├── storage.ts                # MMKV wrapper (keys typed)
│   ├── offline-db.ts             # expo-sqlite cho Guest mode
│   ├── i18n.ts                   # i18next setup, load vi.json / en.json
│   └── sync.ts                   # Sync offline data khi đăng nhập

├── locales/
│   ├── vi.json                   # Tiếng Việt
│   └── en.json                   # Tiếng Anh

├── constants/
│   ├── colors.ts                 # Bảng màu Light + Dark
│   ├── typography.ts             # Font size, weight, line height
│   └── spacing.ts                # Spacing scale (4, 8, 12, 16, 24, 32...)

└── hooks/                        # Global hooks dùng nhiều nơi
    ├── useTheme.ts               # Theme hiện tại + toggle
    ├── useTranslation.ts         # Wrapper i18next
    └── usePermission.ts          # Kiểm tra subscription tier
```

### API Client (lib/api.ts) — Quản lý tập trung

```typescript
// Axios instance với JWT + auto refresh + language header
const api = axios.create({ baseURL: ENV.API_URL })

api.interceptors.request.use(config => {
  config.headers.Authorization = `Bearer ${getToken()}`
  config.headers['Accept-Language'] = getLanguage()  // Vi / En
  return config
})

// Auto refresh JWT khi 401
api.interceptors.response.use(null, async error => {
  if (error.response?.status === 401) {
    const newToken = await refreshToken()
    return api(error.config)  // retry
  }
  return Promise.reject(error)
})
```

### Query Keys Factory (mỗi feature có file riêng)

```typescript
// features/exercises/query-keys.ts
export const exerciseKeys = {
  all: ['exercises'] as const,
  list: (filters: ExerciseFilters) => [...exerciseKeys.all, 'list', filters] as const,
  detail: (id: string) => [...exerciseKeys.all, 'detail', id] as const,
}
```

### Dark / Light Mode

```typescript
// constants/colors.ts
export const Colors = {
  light: {
    background: '#FFFFFF',
    surface: '#F5F5F5',
    primary: '#FF6B35',      // Cam — màu chủ đạo gym
    text: '#1A1A1A',
    textSecondary: '#666666',
    border: '#E0E0E0',
  },
  dark: {
    background: '#121212',
    surface: '#1E1E1E',
    primary: '#FF6B35',
    text: '#F5F5F5',
    textSecondary: '#A0A0A0',
    border: '#333333',
  },
}

// hooks/useTheme.ts
// Ưu tiên: user preference (lưu MMKV) → system default
```

```tsx
// NativeWind usage — tự động theo theme
<View className="bg-white dark:bg-gray-900">
  <Text className="text-gray-900 dark:text-white">Hello</Text>
</View>
```

---

## Quyết định đã chốt

| Vấn đề | Quyết định | Ghi chú rủi ro |
|---|---|---|
| Guest Mode | Có — lưu local SQLite, sync khi đăng nhập | ⚠️ Cần sync logic + conflict resolution |
| Ngôn ngữ | Song ngữ Vi + En | ⚠️ Mọi string phải có trong locales/ |
| Theme | Dark + Light mode | NativeWind hỗ trợ sẵn |
| Routing | Expo Router (file-based) | Không cần cấu hình thủ công |
| Nền tảng | iOS + Android | Cần test thực tế trên cả hai |

## Câu hỏi còn mở

- [x] Tên ứng dụng? → **IronIQ**
- [ ] Deploy backend: Azure / Render.com / Railway?

---

## Lộ trình Phát triển

### Giai đoạn 1 — MVP Core
Auth → Profile → Exercise Library → Workout Builder → Session Logging → Progress Charts → AI Plan Generation

### Giai đoạn 2 — Monetization
Coin system → AdMob Rewarded Ads → RevenueCat Premium subscription → Free tier limits

### Giai đoạn 3 — AI Coach
Chat AI Coach → Auto-progression → Nhận xét buổi tập

### Giai đoạn 4 — Social & Engagement
Streak → Badge/Achievement → Chia sẻ lịch tập → Bảng xếp hạng

---

## Tiêu chí thành công MVP
- Tạo tài khoản / Guest mode hoạt động
- Tạo lịch tập thủ công + bằng AI
- Ghi lại buổi tập và xem lịch sử
- Dark/Light mode hoạt động đúng
- Song ngữ Vi/En switch được
