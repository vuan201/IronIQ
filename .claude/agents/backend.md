# Agent Context — Backend

Bạn đang làm việc trên **backend ASP.NET Core 9** của dự án IronIQ.

## Tài liệu cần đọc ngay
- `.claude/rules/backend.md` — quy tắc code bắt buộc
- `.claude/rules/commits.md` — scopes backend hợp lệ
- `.claude/MEMORY.md` — bước hiện tại đang làm

## Cấu trúc solution

```
apps/backend/src/
├── IronIQ.Domain/          ← Entities, ValueObjects, Events, Enums, Interfaces
├── IronIQ.Application/     ← Features (CQRS), Behaviors, Common
├── IronIQ.Infrastructure/  ← EF Core, External services (Claude, RevenueCat, AdMob)
└── IronIQ.API/             ← Controllers, Middleware, Program.cs
```

## Checklist trước khi code

- [ ] Bước này thuộc layer nào? (Domain / Application / Infrastructure / API)
- [ ] Đã đọc entities liên quan chưa?
- [ ] Feature cần Command hay Query? (thay đổi state → Command, chỉ đọc → Query)
- [ ] Cần migration mới không?
- [ ] Đã có interface trong Application chưa? (tránh tạo lại)

## Lệnh thường dùng

```bash
# Build
dotnet build apps/backend/IronIQ.sln

# Run API
dotnet run --project apps/backend/src/IronIQ.API

# Add migration
dotnet ef migrations add <MigrationName> --project apps/backend/src/IronIQ.Infrastructure --startup-project apps/backend/src/IronIQ.API

# Apply migration
dotnet ef database update --project apps/backend/src/IronIQ.Infrastructure --startup-project apps/backend/src/IronIQ.API

# Add package vào đúng project
dotnet add apps/backend/src/IronIQ.Application package MediatR
```

## External Services

| Service | Interface | Implementation | Config key |
|---|---|---|---|
| AI (Claude) | `IAIService` | `ClaudeAIService` | `ANTHROPIC_API_KEY` |
| Subscription | `ISubscriptionService` | `RevenueCatService` | `REVENUE_CAT_API_KEY` |
| Current User | `ICurrentUserService` | `CurrentUserService` | JWT claim |

## Khi thêm một feature mới

1. Tạo thư mục `Application/Features/<Feature>/Commands/<Action>/` hoặc `Queries/<Name>/`
2. Tạo đủ 3 file: `Command.cs`, `Handler.cs`, `Validator.cs`
3. Thêm endpoint vào Controller (hoặc tạo Controller mới)
4. Tạo migration nếu có entity mới
5. Commit với scope đúng từ `rules/commits.md`
