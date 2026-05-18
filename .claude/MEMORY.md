# MEMORY — IronIQ

Trạng thái: `[ ]` chưa làm · `[~]` đang làm · `[x]` xong

---

## GIAI ĐOẠN 0 — Setup Dự án

### 0.1 Git & Monorepo
- [x] `git init` tại `Gym-Room/`
- [x] Tạo cấu trúc thư mục gốc:
  ```
  Gym-Room/
  ├── apps/
  │   ├── mobile/     ← Expo project
  │   └── backend/    ← .NET solution
  └── .claude/
  ```
- [x] `git add .gitignore .claudeignore .claude/` → commit `chore(setup): init repo with ignore files and project docs`

### 0.2 Backend — .NET Solution
- [x] Tạo solution và 4 project:
  ```
  dotnet new sln -n IronIQ -o apps/backend
  dotnet new classlib -n IronIQ.Domain -o apps/backend/src/IronIQ.Domain
  dotnet new classlib -n IronIQ.Application -o apps/backend/src/IronIQ.Application
  dotnet new classlib -n IronIQ.Infrastructure -o apps/backend/src/IronIQ.Infrastructure
  dotnet new webapi -n IronIQ.API -o apps/backend/src/IronIQ.API
  dotnet sln add **/*.csproj
  ```
- [x] Thêm project references (Domain ← Application ← Infrastructure ← API)
- [x] Cài NuGet packages:
  - `MediatR` + `MediatR.Extensions.Microsoft.DependencyInjection`
  - `FluentValidation.AspNetCore`
  - `AutoMapper.Extensions.Microsoft.DependencyInjection`
  - `Microsoft.EntityFrameworkCore.Design`
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - `BCrypt.Net-Next`
- [x] Tạo `appsettings.json` skeleton (ConnectionStrings, JWT, Claude, RevenueCat)
- [x] commit: `chore(setup): scaffold dotnet solution with 4 layer architecture`

### 0.3 Backend — Skeleton Infrastructure
- [x] Tạo `Result<T>` model (`Application/Common/Models/Result.cs`)
- [x] Tạo 3 MediatR Behaviors: `LoggingBehavior`, `ValidationBehavior`, `TransactionBehavior`
- [x] Tạo `ICurrentUserService` interface + `CurrentUserService` implementation
- [x] Tạo `AppDbContext` rỗng
- [x] Tạo `DependencyInjection.cs` trong Infrastructure và Application
- [x] Wiring trong `Program.cs`
- [x] commit: `chore(infra): add mediatr pipeline behaviors and base infrastructure`

### 0.4 Frontend — Expo Project
- [x] Khởi tạo project:
  ```
  npx create-expo-app apps/mobile --template expo-template-blank-typescript
  ```
- [x] Cài packages:
  ```
  npx expo install expo-router react-native-safe-area-context react-native-screens
  npx expo install expo-sqlite expo-secure-store
  npm install nativewind tailwindcss
  npm install zustand @tanstack/react-query axios
  npm install i18next react-i18next
  npm install react-native-mmkv
  npm install @expo/vector-icons
  ```
- [x] Setup `tailwind.config.js` + `babel.config.js` cho NativeWind
- [x] Setup Expo Router: cấu hình `app.json` scheme + entry point
- [x] commit: `chore(setup): init expo project with core dependencies`

### 0.5 Frontend — Foundation Layer
- [x] Tạo `constants/colors.ts` — Light/Dark palette đầy đủ từ DESIGN.md
- [x] Tạo `constants/typography.ts` — font scale
- [x] Tạo `constants/spacing.ts` — spacing scale 4px
- [x] Tạo `lib/storage.ts` — MMKV wrapper với typed keys
- [x] Tạo `lib/i18n.ts` + `locales/vi.json` + `locales/en.json` skeleton
- [x] Tạo `lib/query-client.ts` — TanStack Query config
- [x] Tạo `lib/api.ts` — Axios instance (baseURL từ env, JWT interceptor placeholder)
- [x] Tạo `hooks/useTheme.ts` — đọc MMKV preference → colorScheme
- [x] Tạo `app/_layout.tsx` — root layout: ThemeProvider + QueryClientProvider + i18next
- [x] commit: `chore(setup): add foundation layer (theme, i18n, api client, storage)`

---

## GIAI ĐOẠN 1 — MVP Core

### 1.1 Auth — Backend
- [x] Tạo `User` entity (Id, Email, PasswordHash, CreatedAt, SubscriptionTier, CoinBalance)
- [x] Tạo `UserProfile` value object (Name, Age, Height, Weight, Goal, FitnessLevel)
- [x] Tạo enum `FitnessGoal`, `FitnessLevel`, `SubscriptionTier`
- [x] Tạo EF config + migration: `dotnet ef migrations add InitialCreate`
- [x] Tạo `RegisterCommand` + `RegisterCommandHandler` + `RegisterValidator`
- [x] Tạo `LoginCommand` + `LoginCommandHandler` (BCrypt verify + JWT generate)
- [x] Tạo `RefreshTokenCommand` + handler
- [x] Tạo `AuthController` (`POST /auth/register`, `POST /auth/login`, `POST /auth/refresh`)
- [x] commit: `feat(auth): add register, login and refresh token endpoints`

### 1.2 Auth — Frontend
- [x] Tạo `features/auth/types.ts`
- [x] Tạo `features/auth/api.ts` — `register()`, `login()`, `refreshToken()`
- [x] Tạo `features/auth/store.ts` — Zustand: `accessToken`, `refreshToken`, `user`
- [x] Cập nhật `lib/api.ts` — gắn JWT interceptor + auto refresh khi 401
- [x] Tạo `app/(auth)/_layout.tsx` + `login.tsx` + `register.tsx` (UI)
- [x] Tạo auth guard trong `app/_layout.tsx` — redirect dựa trên token
- [x] commit: `feat(auth): add login and register screens with JWT auth flow`

### 1.3 Guest Mode
- [x] Tạo `lib/offline-db.ts` — khởi tạo SQLite schema cho guest data
- [x] Tạo `app/(guest)/onboarding.tsx` — chọn "Dùng ngay" vs "Đăng nhập"
- [x] Tạo `lib/sync.ts` — sync offline data lên server khi guest → login
- [x] commit: `feat(auth): add guest mode with offline sqlite storage`

### 1.4 User Profile
- [x] Tạo `UpdateProfileCommand` + handler
- [x] Tạo `GetMyProfileQuery` + handler
- [x] Tạo `UsersController` (`GET /users/me`, `PUT /users/me`)
- [x] Tạo `features/users/` (api, hooks, types)
- [x] Tạo `app/(tabs)/profile.tsx` — form chỉnh sửa profile
- [x] commit: `feat(users): add profile view and edit with personal metrics`

### 1.5 Exercise Library — Backend
- [x] Tạo `Exercise` entity (Id, Name, Description, MuscleGroups[], Equipment[], Difficulty, IsSystem, CreatedByUserId)
- [x] Tạo EF config + migration
- [x] Tạo `DataSeeder` với 32 bài tập hệ thống (chest/back/shoulders/biceps/triceps/legs/abs/cardio)
- [x] Tạo `GetExercisesQuery` + handler (filter: muscle, equipment, difficulty; search; pagination)
- [x] Tạo `GetExerciseByIdQuery` + handler
- [x] Tạo `CreateExerciseCommand` + handler (user-created)
- [x] Tạo `ExercisesController` (`GET /exercises`, `GET /exercises/{id}`, `POST /exercises`)
- [x] commit: `feat(exercises): add exercise library with seed data and CRUD endpoints`

### 1.6 Exercise Library — Frontend
- [x] Tạo `features/exercises/` (api, hooks, query-keys, types)
- [x] Tạo `components/workout/ExerciseCard.tsx`
- [x] Tạo `app/(tabs)/explore.tsx` — danh sách + search + filter chip
- [x] Tạo `components/ui/Skeleton.tsx` — loading placeholder
- [x] Tạo màn hình tạo bài tập tùy chỉnh (`app/exercise/create.tsx`)
- [x] commit: `feat(exercises): add exercise library screen with search and filter`

### 1.7 Workout Builder — Backend
- [x] Tạo entities: `WorkoutPlan`, `WorkoutDay`, `PlanExercise`
- [x] Tạo EF config + migration
- [x] Tạo `CreateWorkoutPlanCommand` + handler
- [x] Tạo `UpdateWorkoutPlanCommand` + handler
- [x] Tạo `DeleteWorkoutPlanCommand` + handler
- [x] Tạo `GetMyPlansQuery` + handler
- [x] Tạo `WorkoutPlansController`
- [x] commit: `feat(workout-plans): add workout plan CRUD endpoints`

### 1.8 Workout Builder — Frontend
- [x] Tạo `features/workout-plans/` (api, hooks, query-keys, types)
- [x] Tạo `components/workout/WorkoutDayCard.tsx`
- [x] Tạo `app/(tabs)/index.tsx` — Home: lịch tập hôm nay + weekly overview
- [x] Tạo `app/workout/create.tsx` — builder: chọn ngày, thêm bài tập
- [x] commit: `feat(workout-plans): add workout builder and home screen`

### 1.9 Session Logging — Backend
- [x] Tạo entities: `WorkoutSession`, `ExerciseLog`, `SetLog`
- [x] Tạo EF config + migration
- [x] Tạo `StartSessionCommand` + handler
- [x] Tạo `LogSetCommand` + handler
- [x] Tạo `CompleteSessionCommand` + handler + raise `WorkoutSessionCompletedEvent`
- [x] Tạo `GetSessionHistoryQuery` + handler
- [x] Tạo `WorkoutSessionsController`
- [x] commit: `feat(sessions): add workout session logging with domain event on complete`

### 1.10 Session Logging — Frontend
- [x] Tạo `features/workout-sessions/` (api, hooks, store, types)
- [x] Tạo `features/workout-sessions/store.ts` — Zustand: active session state
- [x] Tạo `components/workout/SetRow.tsx` — row nhập kg / reps / done
- [x] Tạo `components/workout/RestTimer.tsx` — countdown + haptic
- [x] Tạo `app/workout/session.tsx` — màn hình tập: exercise list + set rows + timer
- [x] Tạo `app/workout/summary.tsx` — tóm tắt sau buổi tập
- [x] commit: `feat(sessions): add active workout screen with set logging and rest timer`

### 1.11 Progress Charts
- [x] Backend: `GetProgressQuery` (weekly frequency + volume per session)
- [x] Backend: `ProgressController` (`GET /progress`)
- [x] Frontend: `features/progress/` (api, hooks, query-keys, types)
- [x] Tạo `components/charts/BarChart.tsx`
- [x] Tạo `components/charts/LineChart.tsx`
- [x] Tạo `app/(tabs)/progress.tsx` — biểu đồ + period selector (4W/8W/12W)
- [x] commit: `feat(progress): add progress charts for workout frequency and volume`

### 1.12 AI Plan Generation
- [x] Backend: Tạo `IAIService` interface
- [x] Backend: Tạo `PromptBuilder.cs` — build prompt từ user profile + goal + equipment
- [x] Backend: Tạo `ClaudeAIService.cs` — gọi Claude API qua HttpClient
- [x] Backend: Tạo `GenerateAIPlanCommand` + handler
- [x] Backend: `WorkoutPlansController` thêm `POST /workout-plans/generate`
- [x] Frontend: Tạo `features/ai-coach/` (api, hooks, types)
- [x] Frontend: Tạo `app/ai-coach/generate.tsx` — form chọn goal + equipment + days/week
- [x] commit: `feat(ai): add AI workout plan generation using Claude API`

---

## GIAI ĐOẠN 2 — Monetization

### 2.1 Coin System — Backend
- [x] Tạo `CoinTransaction` entity (UserId, Amount, Type, Reason, CreatedAt)
- [x] Tạo migration
- [x] Tạo `EarnFromAdCommand` + handler (validate AdMob server-side callback)
- [x] Tạo `SpendCoinsCommand` + handler (kiểm tra balance đủ)
- [x] Tạo `GetBalanceQuery`
- [x] Tạo `CoinsController`
- [x] commit: `feat(coins): add coin system with earn and spend endpoints`

### 2.2 AdMob Integration
- [x] Cài `react-native-google-mobile-ads`
- [x] Setup AdMob app ID trong `app.json`
- [x] Tạo `features/coins/` (api, hooks, types)
- [x] Tạo Rewarded Ad hook `useRewardedAd.ts`
- [x] Tích hợp vào màn hình Coins: nút "Xem quảng cáo → +10 xu"
- [x] commit: `feat(coins): integrate AdMob rewarded ads for coin earning`

### 2.3 RevenueCat — Premium Subscription
- [x] Cài `react-native-purchases`
- [x] Setup RevenueCat project (iOS + Android) ← cần setup thủ công trong RC dashboard
- [x] Backend: `ISubscriptionService` + `RevenueCatService` (webhook validate)
- [x] Backend: Webhook endpoint `POST /webhooks/revenuecat`
- [x] Frontend: `usePermission.ts` — check subscription tier
- [x] Frontend: Paywall screen với gói tháng/năm
- [x] Frontend: Premium badge trên Profile
- [x] Backend: Enforce free tier limits (3 AI plans/tháng, 2 active plans)
- [x] commit: `feat(coins): add RevenueCat premium subscription with paywall`

---

## GIAI ĐOẠN 3 — AI Coach

### 3.1 AI Chat
- [x] Backend: `AskCoachQuery` + handler (context: profile + 5 buổi tập gần nhất)
- [x] Backend: `AiCoachController` (`POST /ai-coach/ask`)
- [x] Frontend: Tạo `features/ai-coach/store.ts` — conversation history (Zustand)
- [x] Frontend: Tạo `app/ai-coach/index.tsx` — chat UI (bubble messages)
- [x] commit: `feat(ai-coach): add AI coach chat with workout context`

### 3.2 Auto-Progression
- [x] Backend: `AutoProgressionHandler` — lắng nghe `WorkoutSessionCompletedEvent`
  - Detect: 100% sets hoàn thành trong 2 buổi liên tiếp → suggest tăng tạ 2.5kg
- [x] Frontend: Hiển thị suggestion card sau session summary
- [x] commit: `feat(ai-coach): add auto-progression suggestions based on session history`

### 3.3 Session Review
- [x] Backend: `GetSessionReviewQuery` — AI tóm tắt buổi tập, so với lần trước
- [x] Frontend: Tích hợp vào `app/workout/summary.tsx`
- [x] commit: `feat(ai-coach): add AI session review on workout completion`

---

## GIAI ĐOẠN 4 — Social & Engagement

### 4.1 Streak & Domain Events
- [x] Backend: `UpdateStreakHandler` lắng nghe `WorkoutSessionCompletedEvent`
- [x] Backend: `Streak` field trên `User` entity
- [x] Frontend: Streak flame badge trên Home screen
- [x] Frontend: Push notification nhắc tập (expo-notifications)
- [x] commit: `feat(social): add workout streak tracking with push notification reminder`

### 4.2 Achievements
- [x] Backend: `Achievement` entity + seed data (định nghĩa ~15 badge)
- [x] Backend: `UserAchievement` entity
- [x] Backend: `CheckAchievementsHandler` lắng nghe `WorkoutSessionCompletedEvent`
- [x] Frontend: Achievement list screen + badge card (locked/unlocked)
- [x] Frontend: Celebration animation khi mở khóa badge mới
- [x] commit: `feat(social): add achievement system with 15 milestone badges`

### 4.3 Share & Leaderboard
- [x] Frontend: Share lịch tập (export as image hoặc deep link)
- [x] Backend: Leaderboard query (weekly streak, total volume — opt-in)
- [x] Frontend: Leaderboard screen
- [x] commit: `feat(social): add workout sharing and opt-in leaderboard`

---

## Log Hoàn thành

| Ngày | Bước | Commit Hash |
|---|---|---|
| 2026-05-17 | 0.1 Git & Monorepo | d031097 |
| 2026-05-17 | 0.2 Backend .NET Solution | 26f8c89 |
| 2026-05-17 | 0.3 Backend Skeleton Infrastructure | 67dbdd6 |
| 2026-05-17 | 0.4 Frontend Expo Project | 2a78e6f |
| 2026-05-17 | 0.5 Frontend Foundation Layer | a493371 |
| 2026-05-17 | 1.1 Auth — Backend | 2816edb |
| 2026-05-17 | 1.2 Auth — Frontend | ea25a32 |
| 2026-05-17 | 1.3 Guest Mode | a3b41ea |
| 2026-05-17 | 1.4 User Profile | c163d85 |
| 2026-05-17 | 1.5 Exercise Library — Backend | bbacf64 |
| 2026-05-17 | 1.6 Exercise Library — Frontend | d7850d8 |
| 2026-05-17 | 1.7 Workout Builder — Backend | 4ee0d68 |
| 2026-05-17 | 1.8 Workout Builder — Frontend | 1ca2573 |
| 2026-05-17 | 1.9 Session Logging — Backend | af979e7 |
| 2026-05-17 | 1.10 Session Logging — Frontend | 32d7302 |
| 2026-05-17 | 1.11 Progress Charts | baed80a |
| 2026-05-17 | 1.12 AI Plan Generation | 9f3f280 |
| 2026-05-18 | 2.1 Coin System — Backend | 3747b26 |
| 2026-05-18 | 2.2 AdMob Integration | bf1eb55 |
| 2026-05-18 | 2.3 RevenueCat — Premium Subscription | dbef8d5 |
| 2026-05-18 | 3.1 AI Chat | 9b0e752 |
| 2026-05-18 | 3.2 Auto-Progression | 2e5ef82 |
| 2026-05-18 | 3.3 Session Review | 34190fb |
| 2026-05-18 | 4.1 Streak & Domain Events | 95b8c37 |
| 2026-05-18 | 4.2 Achievements | 302a130 |
| 2026-05-18 | 4.3 Share & Leaderboard | 86d290f |
