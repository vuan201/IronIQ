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
- [~] Tạo solution và 4 project:
  ```
  dotnet new sln -n IronIQ -o apps/backend
  dotnet new classlib -n IronIQ.Domain -o apps/backend/src/IronIQ.Domain
  dotnet new classlib -n IronIQ.Application -o apps/backend/src/IronIQ.Application
  dotnet new classlib -n IronIQ.Infrastructure -o apps/backend/src/IronIQ.Infrastructure
  dotnet new webapi -n IronIQ.API -o apps/backend/src/IronIQ.API
  dotnet sln add **/*.csproj
  ```
- [ ] Thêm project references (Domain ← Application ← Infrastructure ← API)
- [ ] Cài NuGet packages:
  - `MediatR` + `MediatR.Extensions.Microsoft.DependencyInjection`
  - `FluentValidation.AspNetCore`
  - `AutoMapper.Extensions.Microsoft.DependencyInjection`
  - `Microsoft.EntityFrameworkCore.Design`
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - `BCrypt.Net-Next`
- [ ] Tạo `appsettings.json` skeleton (ConnectionStrings, JWT, Claude, RevenueCat)
- [ ] commit: `chore(setup): scaffold dotnet solution with 4 layer architecture`

### 0.3 Backend — Skeleton Infrastructure
- [ ] Tạo `Result<T>` model (`Application/Common/Models/Result.cs`)
- [ ] Tạo 3 MediatR Behaviors: `LoggingBehavior`, `ValidationBehavior`, `TransactionBehavior`
- [ ] Tạo `ICurrentUserService` interface + `CurrentUserService` implementation
- [ ] Tạo `AppDbContext` rỗng
- [ ] Tạo `DependencyInjection.cs` trong Infrastructure và Application
- [ ] Wiring trong `Program.cs`
- [ ] commit: `chore(infra): add mediatr pipeline behaviors and base infrastructure`

### 0.4 Frontend — Expo Project
- [ ] Khởi tạo project:
  ```
  npx create-expo-app apps/mobile --template expo-template-blank-typescript
  ```
- [ ] Cài packages:
  ```
  npx expo install expo-router react-native-safe-area-context react-native-screens
  npx expo install expo-sqlite expo-secure-store
  npm install nativewind tailwindcss
  npm install zustand @tanstack/react-query axios
  npm install i18next react-i18next
  npm install react-native-mmkv
  npm install @expo/vector-icons
  ```
- [ ] Setup `tailwind.config.js` + `babel.config.js` cho NativeWind
- [ ] Setup Expo Router: cấu hình `app.json` scheme + entry point
- [ ] commit: `chore(setup): init expo project with core dependencies`

### 0.5 Frontend — Foundation Layer
- [ ] Tạo `constants/colors.ts` — Light/Dark palette đầy đủ từ DESIGN.md
- [ ] Tạo `constants/typography.ts` — font scale
- [ ] Tạo `constants/spacing.ts` — spacing scale 4px
- [ ] Tạo `lib/storage.ts` — MMKV wrapper với typed keys
- [ ] Tạo `lib/i18n.ts` + `locales/vi.json` + `locales/en.json` skeleton
- [ ] Tạo `lib/query-client.ts` — TanStack Query config
- [ ] Tạo `lib/api.ts` — Axios instance (baseURL từ env, JWT interceptor placeholder)
- [ ] Tạo `hooks/useTheme.ts` — đọc MMKV preference → colorScheme
- [ ] Tạo `app/_layout.tsx` — root layout: ThemeProvider + QueryClientProvider + i18next
- [ ] commit: `chore(setup): add foundation layer (theme, i18n, api client, storage)`

---

## GIAI ĐOẠN 1 — MVP Core

### 1.1 Auth — Backend
- [ ] Tạo `User` entity (Id, Email, PasswordHash, CreatedAt, SubscriptionTier, CoinBalance)
- [ ] Tạo `UserProfile` value object (Name, Age, Height, Weight, Goal, FitnessLevel)
- [ ] Tạo enum `FitnessGoal`, `FitnessLevel`, `SubscriptionTier`
- [ ] Tạo EF config + migration: `dotnet ef migrations add InitialCreate`
- [ ] Tạo `RegisterCommand` + `RegisterCommandHandler` + `RegisterValidator`
- [ ] Tạo `LoginCommand` + `LoginCommandHandler` (BCrypt verify + JWT generate)
- [ ] Tạo `RefreshTokenCommand` + handler
- [ ] Tạo `AuthController` (`POST /auth/register`, `POST /auth/login`, `POST /auth/refresh`)
- [ ] commit: `feat(auth): add register, login and refresh token endpoints`

### 1.2 Auth — Frontend
- [ ] Tạo `features/auth/types.ts`
- [ ] Tạo `features/auth/api.ts` — `register()`, `login()`, `refreshToken()`
- [ ] Tạo `features/auth/store.ts` — Zustand: `accessToken`, `refreshToken`, `user`
- [ ] Cập nhật `lib/api.ts` — gắn JWT interceptor + auto refresh khi 401
- [ ] Tạo `app/(auth)/_layout.tsx` + `login.tsx` + `register.tsx` (UI)
- [ ] Tạo auth guard trong `app/_layout.tsx` — redirect dựa trên token
- [ ] commit: `feat(auth): add login and register screens with JWT auth flow`

### 1.3 Guest Mode
- [ ] Tạo `lib/offline-db.ts` — khởi tạo SQLite schema cho guest data
- [ ] Tạo `app/(guest)/onboarding.tsx` — chọn "Dùng ngay" vs "Đăng nhập"
- [ ] Tạo `lib/sync.ts` — sync offline data lên server khi guest → login
- [ ] commit: `feat(auth): add guest mode with offline sqlite storage`

### 1.4 User Profile
- [ ] Tạo `UpdateProfileCommand` + handler
- [ ] Tạo `GetMyProfileQuery` + handler
- [ ] Tạo `UsersController` (`GET /users/me`, `PUT /users/me`)
- [ ] Tạo `features/users/` (api, hooks, types)
- [ ] Tạo `app/(tabs)/profile.tsx` — form chỉnh sửa profile
- [ ] commit: `feat(users): add profile view and edit with personal metrics`

### 1.5 Exercise Library — Backend
- [ ] Tạo `Exercise` entity (Id, Name, Description, MuscleGroups[], Equipment[], Difficulty, IsSystem, CreatedByUserId)
- [ ] Tạo EF config + migration
- [ ] Tạo seed file JSON (~100 bài tập hệ thống)
- [ ] Tạo `DataSeeder` chạy khi startup
- [ ] Tạo `GetExercisesQuery` + handler (filter: muscle, equipment, difficulty; search; pagination)
- [ ] Tạo `GetExerciseByIdQuery` + handler
- [ ] Tạo `CreateExerciseCommand` + handler (user-created)
- [ ] Tạo `ExercisesController` (`GET /exercises`, `GET /exercises/{id}`, `POST /exercises`)
- [ ] commit: `feat(exercises): add exercise library with seed data and CRUD endpoints`

### 1.6 Exercise Library — Frontend
- [ ] Tạo `features/exercises/` (api, hooks, query-keys, types)
- [ ] Tạo `components/workout/ExerciseCard.tsx`
- [ ] Tạo `app/(tabs)/explore.tsx` — danh sách + search + filter chip
- [ ] Tạo `components/ui/Skeleton.tsx` — loading placeholder
- [ ] Tạo màn hình tạo bài tập tùy chỉnh
- [ ] commit: `feat(exercises): add exercise library screen with search and filter`

### 1.7 Workout Builder — Backend
- [ ] Tạo entities: `WorkoutPlan`, `WorkoutDay`, `PlanExercise`
- [ ] Tạo EF config + migration
- [ ] Tạo `CreateWorkoutPlanCommand` + handler
- [ ] Tạo `UpdateWorkoutPlanCommand` + handler
- [ ] Tạo `DeleteWorkoutPlanCommand` + handler
- [ ] Tạo `GetMyPlansQuery` + handler
- [ ] Tạo `WorkoutPlansController`
- [ ] commit: `feat(workout-plans): add workout plan CRUD endpoints`

### 1.8 Workout Builder — Frontend
- [ ] Tạo `features/workout-plans/` (api, hooks, query-keys, types)
- [ ] Tạo `components/workout/WorkoutDayCard.tsx`
- [ ] Tạo `app/(tabs)/index.tsx` — Home: lịch tập hôm nay + weekly overview
- [ ] Tạo `app/workout/create.tsx` — builder: chọn ngày, thêm bài tập
- [ ] commit: `feat(workout-plans): add workout builder and home screen`

### 1.9 Session Logging — Backend
- [ ] Tạo entities: `WorkoutSession`, `ExerciseLog`, `SetLog`
- [ ] Tạo EF config + migration
- [ ] Tạo `StartSessionCommand` + handler
- [ ] Tạo `LogSetCommand` + handler
- [ ] Tạo `CompleteSessionCommand` + handler + raise `WorkoutSessionCompletedEvent`
- [ ] Tạo `GetSessionHistoryQuery` + handler
- [ ] Tạo `WorkoutSessionsController`
- [ ] commit: `feat(sessions): add workout session logging with domain event on complete`

### 1.10 Session Logging — Frontend
- [ ] Tạo `features/workout-sessions/` (api, hooks, store, types)
- [ ] Tạo `features/workout-sessions/store.ts` — Zustand: active session state
- [ ] Tạo `components/workout/SetRow.tsx` — row nhập kg / reps / done
- [ ] Tạo `components/workout/RestTimer.tsx` — countdown + haptic
- [ ] Tạo `app/workout/[id].tsx` — màn hình tập: exercise list + set rows + timer
- [ ] Tạo `app/workout/summary.tsx` — tóm tắt sau buổi tập
- [ ] commit: `feat(sessions): add active workout screen with set logging and rest timer`

### 1.11 Progress Charts
- [ ] Backend: `GetProgressQuery` (weight over time, volume per muscle group, 1RM estimate)
- [ ] Frontend: cài `victory-native` hoặc `react-native-gifted-charts`
- [ ] Tạo `components/charts/WeightChart.tsx`
- [ ] Tạo `components/charts/StrengthChart.tsx`
- [ ] Tạo `app/(tabs)/progress.tsx` — biểu đồ + period selector (1W/1M/3M/1Y)
- [ ] commit: `feat(progress): add progress charts for weight and strength tracking`

### 1.12 AI Plan Generation
- [ ] Backend: Tạo `IAIService` interface
- [ ] Backend: Tạo `PromptBuilder.cs` — build prompt từ user profile + goal + equipment
- [ ] Backend: Tạo `ClaudeAIService.cs` — gọi Claude API, parse response thành `WorkoutPlan`
- [ ] Backend: Tạo `GenerateAIPlanCommand` + handler
- [ ] Backend: `WorkoutPlansController` thêm `POST /workout-plans/generate`
- [ ] Frontend: Tạo `app/ai-coach/generate.tsx` — form chọn goal + equipment + days/week
- [ ] Frontend: Loading animation khi AI đang tạo
- [ ] Frontend: Preview plan trước khi lưu
- [ ] commit: `feat(ai): add AI workout plan generation using Claude API`

---

## GIAI ĐOẠN 2 — Monetization

### 2.1 Coin System — Backend
- [ ] Tạo `CoinTransaction` entity (UserId, Amount, Type, Reason, CreatedAt)
- [ ] Tạo migration
- [ ] Tạo `EarnFromAdCommand` + handler (validate AdMob server-side callback)
- [ ] Tạo `SpendCoinsCommand` + handler (kiểm tra balance đủ)
- [ ] Tạo `GetBalanceQuery`
- [ ] Tạo `CoinsController`
- [ ] commit: `feat(coins): add coin system with earn and spend endpoints`

### 2.2 AdMob Integration
- [ ] Cài `react-native-google-mobile-ads`
- [ ] Setup AdMob app ID trong `app.json`
- [ ] Tạo `features/coins/` (api, hooks, types)
- [ ] Tạo Rewarded Ad hook `useRewardedAd.ts`
- [ ] Tích hợp vào màn hình Coins: nút "Xem quảng cáo → +10 xu"
- [ ] commit: `feat(coins): integrate AdMob rewarded ads for coin earning`

### 2.3 RevenueCat — Premium Subscription
- [ ] Cài `react-native-purchases`
- [ ] Setup RevenueCat project (iOS + Android)
- [ ] Backend: `ISubscriptionService` + `RevenueCatService` (webhook validate)
- [ ] Backend: Webhook endpoint `POST /webhooks/revenuecat`
- [ ] Frontend: `usePermission.ts` — check subscription tier
- [ ] Frontend: Paywall screen với gói tháng/năm
- [ ] Frontend: Premium badge trên Profile
- [ ] Backend: Enforce free tier limits (3 AI plans/tháng, 2 active plans)
- [ ] commit: `feat(coins): add RevenueCat premium subscription with paywall`

---

## GIAI ĐOẠN 3 — AI Coach

### 3.1 AI Chat
- [ ] Backend: `AskCoachQuery` + handler (context: profile + 5 buổi tập gần nhất)
- [ ] Backend: `AiCoachController` (`POST /ai-coach/ask`)
- [ ] Frontend: Tạo `features/ai-coach/store.ts` — conversation history (Zustand)
- [ ] Frontend: Tạo `app/ai-coach/index.tsx` — chat UI (bubble messages)
- [ ] commit: `feat(ai-coach): add AI coach chat with workout context`

### 3.2 Auto-Progression
- [ ] Backend: `AutoProgressionHandler` — lắng nghe `WorkoutSessionCompletedEvent`
  - Detect: 100% sets hoàn thành trong 2 buổi liên tiếp → suggest tăng tạ 2.5kg
- [ ] Frontend: Hiển thị suggestion card sau session summary
- [ ] commit: `feat(ai-coach): add auto-progression suggestions based on session history`

### 3.3 Session Review
- [ ] Backend: `GetSessionReviewQuery` — AI tóm tắt buổi tập, so với lần trước
- [ ] Frontend: Tích hợp vào `app/workout/summary.tsx`
- [ ] commit: `feat(ai-coach): add AI session review on workout completion`

---

## GIAI ĐOẠN 4 — Social & Engagement

### 4.1 Streak & Domain Events
- [ ] Backend: `UpdateStreakHandler` lắng nghe `WorkoutSessionCompletedEvent`
- [ ] Backend: `Streak` field trên `User` entity
- [ ] Frontend: Streak flame badge trên Home screen
- [ ] Frontend: Push notification nhắc tập (expo-notifications)
- [ ] commit: `feat(social): add workout streak tracking with push notification reminder`

### 4.2 Achievements
- [ ] Backend: `Achievement` entity + seed data (định nghĩa ~15 badge)
- [ ] Backend: `UserAchievement` entity
- [ ] Backend: `CheckAchievementsHandler` lắng nghe `WorkoutSessionCompletedEvent`
- [ ] Frontend: Achievement list screen + badge card (locked/unlocked)
- [ ] Frontend: Celebration animation khi mở khóa badge mới
- [ ] commit: `feat(social): add achievement system with 15 milestone badges`

### 4.3 Share & Leaderboard
- [ ] Frontend: Share lịch tập (export as image hoặc deep link)
- [ ] Backend: Leaderboard query (weekly streak, total volume — opt-in)
- [ ] Frontend: Leaderboard screen
- [ ] commit: `feat(social): add workout sharing and opt-in leaderboard`

---

## Log Hoàn thành

| Ngày | Bước | Commit Hash |
|---|---|---|
| 2026-05-17 | 0.1 Git & Monorepo | d031097 |
