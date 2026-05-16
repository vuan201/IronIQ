# Quy tắc Backend — ASP.NET Core 9

## Kiến trúc

```
IronIQ.Domain          ← không phụ thuộc gì
IronIQ.Application     ← phụ thuộc Domain
IronIQ.Infrastructure  ← phụ thuộc Application
IronIQ.API             ← phụ thuộc Infrastructure
```

## Cấu trúc một Feature (CQRS)

Mỗi feature trong `Application/Features/<FeatureName>/` có cấu trúc:

```
Features/
└── WorkoutPlans/
    ├── Commands/
    │   └── CreateWorkoutPlan/
    │       ├── CreateWorkoutPlanCommand.cs      ← record với input
    │       ├── CreateWorkoutPlanHandler.cs      ← business logic
    │       └── CreateWorkoutPlanValidator.cs    ← FluentValidation
    └── Queries/
        └── GetMyPlans/
            ├── GetMyPlansQuery.cs
            ├── GetMyPlansHandler.cs
            └── WorkoutPlanDto.cs               ← response shape
```

## Quy tắc bắt buộc

### Controller
- Chỉ được gọi `await _mediator.Send(command)` — không viết logic ở đây
- Map HTTP request → Command/Query
- Map Result<T> → HTTP response (200 / 400 / 401 / 404)

```csharp
// ✅ Đúng
[HttpPost]
public async Task<IActionResult> Register(RegisterRequest request)
{
    var command = new RegisterCommand(request.Email, request.Password);
    var result = await _mediator.Send(command);
    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
}

// ❌ Sai — logic trong controller
[HttpPost]
public async Task<IActionResult> Register(RegisterRequest request)
{
    var user = await _userRepository.FindByEmail(request.Email);
    if (user != null) return BadRequest("Email exists");
    ...
}
```

### Handler
- Nhận `Command`/`Query` và `CancellationToken`
- Trả về `Result<T>` hoặc `Result` — không throw exception cho business error
- Không validate input (FluentValidation Behavior làm trước đó)
- Gọi `repository` hoặc `domain service`, không gọi trực tiếp `DbContext`

```csharp
// ✅ Đúng
public async Task<Result<WorkoutPlanDto>> Handle(CreateWorkoutPlanCommand command, CancellationToken ct)
{
    var plan = WorkoutPlan.Create(command.UserId, command.Name, command.Days);
    await _repository.AddAsync(plan, ct);
    return Result.Success(_mapper.Map<WorkoutPlanDto>(plan));
}
```

### Entity / Domain
- Entity tự validate invariants của mình qua factory method hoặc property setter
- Raise Domain Event trong phương thức thay đổi state
- Không inject service vào Entity

```csharp
// ✅ Đúng — factory method validate
public static WorkoutPlan Create(Guid userId, string name, IList<WorkoutDay> days)
{
    if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name required");
    if (days.Count == 0 || days.Count > 7) throw new DomainException("1-7 days required");
    var plan = new WorkoutPlan { ... };
    plan.AddDomainEvent(new WorkoutPlanCreatedEvent(plan.Id));
    return plan;
}
```

### Validator (FluentValidation)
- Validate chỉ format/presence của input — không validate business rule
- Business rule (vd: email đã tồn tại) → validate trong Handler, trả về `Result.Failure`

```csharp
public class CreateWorkoutPlanValidator : AbstractValidator<CreateWorkoutPlanCommand>
{
    public CreateWorkoutPlanValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Days).NotEmpty().Must(d => d.Count <= 7);
    }
}
```

### Repository
- Generic `IRepository<T>` cho CRUD cơ bản
- Feature-specific interface khi cần query phức tạp

```csharp
public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
{
    Task<IList<WorkoutPlan>> GetByUserIdAsync(Guid userId, CancellationToken ct);
}
```

## MediatR Pipeline (thứ tự cố định)

```
1. LoggingBehavior       — log request name + duration
2. ValidationBehavior    — chạy FluentValidation, return lỗi nếu invalid
3. TransactionBehavior   — wrap Command (không phải Query) trong DB transaction
4. Handler               — business logic
```

## Result Pattern

```csharp
// Thành công
return Result.Success(dto);
return Result<string>.Success("value");

// Thất bại — business error (không throw)
return Result.Failure(Error.NotFound("User", userId));
return Result.Failure(Error.Conflict("Email already exists"));

// Map trong Controller
return result.IsSuccess
    ? Ok(result.Value)
    : result.Error.Type switch
    {
        ErrorType.NotFound => NotFound(result.Error.Message),
        ErrorType.Conflict => Conflict(result.Error.Message),
        _ => BadRequest(result.Error.Message)
    };
```

## Naming Conventions

| Thành phần | Convention | Ví dụ |
|---|---|---|
| Command | `{Action}{Feature}Command` | `CreateWorkoutPlanCommand` |
| Query | `Get{Feature}Query` | `GetMyPlansQuery` |
| Handler | `{Command/Query}Handler` | `CreateWorkoutPlanCommandHandler` |
| DTO | `{Feature}Dto` | `WorkoutPlanDto` |
| Controller | `{Feature}Controller` | `WorkoutPlansController` |
| Repository | `I{Feature}Repository` | `IWorkoutPlanRepository` |
| Domain Event | `{Entity}{Action}Event` | `WorkoutSessionCompletedEvent` |
