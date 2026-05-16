# Agent Context — Frontend

Bạn đang làm việc trên **frontend React Native + Expo** của dự án IronIQ.

## Tài liệu cần đọc ngay
- `.claude/rules/frontend.md` — quy tắc code bắt buộc
- `.claude/rules/commits.md` — scopes frontend hợp lệ
- `.claude/DESIGN.md` — design system (màu, typography, component specs)
- `.claude/MEMORY.md` — bước hiện tại đang làm

## Cấu trúc project

```
apps/mobile/src/
├── app/           ← Expo Router screens
├── features/      ← api · hooks · store · types (per feature)
├── components/    ← ui/ · workout/ · charts/ · layout/
├── lib/           ← api.ts · storage.ts · i18n.ts · sync.ts
├── constants/     ← colors.ts · typography.ts · spacing.ts
├── hooks/         ← useTheme · useTranslation · usePermission
└── locales/       ← vi.json · en.json
```

## Checklist trước khi code

- [ ] Đây là Screen, Feature logic, hay Component?
- [ ] Screen → import từ `features/` và `components/` thôi
- [ ] Component → nhận data qua props, không tự fetch
- [ ] Đã thêm i18n keys vào cả `vi.json` và `en.json` chưa?
- [ ] Đã dùng NativeWind `dark:` class chưa? (không hardcode màu)
- [ ] TypeScript — không có `any`?

## Lệnh thường dùng

```bash
# Start dev server
npx expo start --tunnel

# Start trên thiết bị cụ thể
npx expo start --ios
npx expo start --android

# Cài package Expo-compatible
npx expo install <package-name>

# Cài package npm thường
npm install <package-name> --prefix apps/mobile

# Type check
npx tsc --noEmit --project apps/mobile/tsconfig.json

# Xem logs
npx expo start --clear
```

## Design Tokens (tham chiếu nhanh)

```ts
// Màu primary
primary: '#FF6B35'

// Background
light bg: '#FFFFFF'  |  dark bg: '#0F0F0F'
light surface: '#F5F5F5'  |  dark surface: '#1A1A1A'

// Text
light text: '#111111'  |  dark text: '#F0F0F0'
light text-secondary: '#6B6B6B'  |  dark text-secondary: '#9A9A9A'

// Spacing base: 4px  |  Screen padding: 16px
// Border radius card: 16px  |  button: 12px
```

## Khi thêm một màn hình mới

1. Tạo file trong `app/` đúng vị trí route (xem navigation structure trong PLAN.md)
2. Tạo hoặc cập nhật `features/<name>/` nếu cần logic mới
3. Thêm i18n keys vào `locales/vi.json` + `locales/en.json`
4. Test dark mode: bật dark mode trên thiết bị và kiểm tra visually
5. Commit với scope đúng từ `rules/commits.md`

## Khi thêm một component mới

1. Xác định: `ui/` (primitive) hay `workout/`, `charts/`, `layout/`?
2. Viết interface Props rõ ràng
3. Không import từ `features/` — chỉ nhận qua props
4. Không hardcode text — truyền qua props hoặc dùng `t()`
5. Đảm bảo dark mode hoạt động

## API calls qua Axios instance

```ts
// Luôn dùng instance từ lib/api.ts — KHÔNG tạo axios instance mới
import { api } from '@/lib/api'

// Trong features/<name>/api.ts
export const workoutApi = {
  create: (data: CreatePlanDto) => api.post<WorkoutPlan>('/workout-plans', data),
}
```
