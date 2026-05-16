# Skill: Thêm Feature Mới (Full Stack)

Dùng checklist này mỗi khi thêm một feature end-to-end (backend + frontend).

## Thông tin cần xác định trước

- Tên feature: `___________` (vd: `WorkoutPlan`)
- Scope commit: `___________` (vd: `workout-plans`)
- Cần entities mới: `[ ] Có  [ ] Không`
- Cần migration: `[ ] Có  [ ] Không`

---

## BACKEND

### Bước 1 — Domain (nếu có entity mới)
```
□ Tạo Entity trong IronIQ.Domain/Entities/
□ Thêm ValueObjects nếu cần
□ Thêm DomainEvent nếu action quan trọng
□ Thêm Enum nếu cần
□ Thêm vào IRepository<T> hoặc tạo interface riêng
```

### Bước 2 — Persistence
```
□ Tạo EF Configuration trong Infrastructure/Persistence/Configurations/
□ Thêm DbSet vào AppDbContext
□ dotnet ef migrations add <TênMigration>
□ Kiểm tra migration file trước khi apply
□ dotnet ef database update
```

### Bước 3 — Application Layer
```
□ Tạo thư mục Application/Features/<FeatureName>/
□ Commands/<Action>/:
   □ <Action><Feature>Command.cs  (record)
   □ <Action><Feature>Handler.cs  (implement IRequestHandler)
   □ <Action><Feature>Validator.cs (FluentValidation)
□ Queries/<Name>/:
   □ <Name>Query.cs
   □ <Name>Handler.cs
   □ <Feature>Dto.cs  (response shape, dùng AutoMapper)
□ Thêm AutoMapper mapping vào MappingProfile.cs
```

### Bước 4 — API Layer
```
□ Tạo hoặc cập nhật <Feature>Controller.cs
□ Mỗi endpoint: [HttpGet/Post/Put/Delete] + route + [Authorize] nếu cần
□ Map Result<T> → ActionResult đúng status code
□ Kiểm tra Swagger chạy đúng endpoint
```

### Bước 5 — Backend commit
```
git commit -m "feat(<scope>): add <feature> <action> endpoint"
```

---

## FRONTEND

### Bước 6 — Types & API
```
□ Tạo features/<feature>/types.ts
   □ Interface cho entity (match backend DTO)
   □ Interface cho request body
   □ Interface cho filters nếu có
□ Tạo features/<feature>/api.ts
   □ Mỗi method gọi endpoint tương ứng
   □ Dùng api instance từ lib/api.ts
```

### Bước 7 — Query Keys & Hooks
```
□ Tạo features/<feature>/query-keys.ts
   □ all, list(filters), detail(id)
□ Tạo features/<feature>/hooks.ts
   □ useQuery hooks cho mỗi GET endpoint
   □ useMutation hooks cho mỗi POST/PUT/DELETE
   □ onSuccess: invalidateQueries đúng key
```

### Bước 8 — Store (nếu cần client state)
```
□ Tạo features/<feature>/store.ts  (Zustand)
□ Chỉ store state KHÔNG có trên server (vd: active session)
□ Không lưu server data vào Zustand
```

### Bước 9 — Components
```
□ Tạo component trong components/<category>/
□ Props interface rõ ràng, không any
□ Không tự fetch data trong component
□ NativeWind dark: class cho mọi màu
```

### Bước 10 — Screen
```
□ Tạo file trong app/ đúng vị trí route
□ Import hooks từ features/ — không gọi API trực tiếp
□ Import components từ components/
□ Mọi text dùng t('key') từ i18next
```

### Bước 11 — i18n
```
□ Thêm keys vào locales/vi.json
□ Thêm keys vào locales/en.json (cùng lúc)
□ Key format: feature.component.label
```

### Bước 12 — Kiểm tra
```
□ Chạy feature trên iOS simulator
□ Chạy feature trên Android emulator (hoặc thiết bị)
□ Bật Dark mode — kiểm tra visual
□ Bật Tiếng Việt — kiểm tra text
□ npx tsc --noEmit — không có TypeScript error
```

### Bước 13 — Frontend commit
```
git commit -m "feat(<scope>): add <feature> screen and hooks"
```

---

## Bước 14 — Cập nhật MEMORY.md
```
□ Đánh dấu [x] các bước liên quan
□ Ghi commit hash vào bảng Log
□ Checkpoint: đã làm gì | xác minh gì | còn lại gì
```
