# Skill: Thêm Screen Mới (Frontend Only)

Dùng khi chỉ cần thêm màn hình mới mà không cần thay đổi backend.

## Xác định trước

- Route path: `___________` (vd: `app/workout/create.tsx`)
- Nằm trong group: `(auth)` · `(guest)` · `(tabs)` · stack riêng
- Cần data từ API: `[ ] Có  [ ] Không`
- Cần Zustand store: `[ ] Có  [ ] Không`

---

## Checklist

### 1. Tạo file screen
```
□ Tạo file tại đúng vị trí trong app/
□ Export default function <ScreenName>Screen()
□ Wrap trong <ScreenWrapper> (safe area + padding chuẩn)
```

### 2. Navigation
```
□ Nếu màn hình mới cần navigate từ màn hình khác:
   □ Thêm link/router.push('<route>') tại điểm trigger
□ Nếu cần params: dùng useLocalSearchParams() từ expo-router
□ Nếu là modal: thêm vào _layout.tsx với presentation: 'modal'
```

### 3. Data (nếu cần)
```
□ Dùng hook đã có trong features/<name>/hooks.ts
□ Xử lý loading state: hiện <Skeleton /> hoặc <ActivityIndicator />
□ Xử lý error state: hiện thông báo rõ ràng
□ Xử lý empty state: hiện UI "chưa có dữ liệu"
```

### 4. UI — Theo Design System
```
□ Màu từ constants/colors.ts qua NativeWind class
□ Spacing từ constants/spacing.ts
□ Typography đúng scale (h1/h2/body/caption)
□ Border radius đúng (card: 16px, button: 12px)
□ Touch target tối thiểu 44×44px
```

### 5. Dark Mode
```
□ Mỗi View/Text có cả class sáng và dark:
   bg-white dark:bg-[#1A1A1A]
   text-[#111] dark:text-[#F0F0F0]
□ Bật Dark mode trên simulator — kiểm tra visual
```

### 6. i18n
```
□ Không có string hardcode hiển thị cho người dùng
□ Thêm keys vào locales/vi.json
□ Thêm keys vào locales/en.json
□ Dùng const { t } = useTranslation() trong component
```

### 7. TypeScript
```
□ Không có `any`
□ Props có interface rõ ràng
□ npx tsc --noEmit — pass
```

### 8. Kiểm tra
```
□ Navigate đến màn hình được
□ Back/close hoạt động đúng
□ Loading state đẹp (không flash trắng)
□ Dark mode đúng
□ Text tiếng Việt hiển thị đúng
```

### 9. Commit
```
git commit -m "feat(<scope>): add <screen name> screen"
```

### 10. Cập nhật MEMORY.md
```
□ Đánh dấu [x] bước tương ứng
□ Ghi commit hash vào bảng Log
```
