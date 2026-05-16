# Quy tắc Commit — Conventional Commits

Mọi commit **bắt buộc** theo [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).

## Format

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

- **type**: loại thay đổi (xem bảng bên dưới)
- **scope**: phần của hệ thống bị ảnh hưởng (xem danh sách scope)
- **description**: câu mô tả ngắn, viết thường, không dấu chấm cuối, tiếng Anh
- **body**: giải thích lý do thay đổi (không phải cái đã làm)
- **footer**: breaking change hoặc issue reference

## Types

| Type | Khi nào dùng |
|---|---|
| `feat` | Thêm tính năng mới |
| `fix` | Sửa bug |
| `refactor` | Tái cấu trúc, không thêm tính năng, không sửa bug |
| `style` | Thay đổi UI/styling thuần túy (không logic) |
| `test` | Thêm hoặc sửa test |
| `chore` | Cập nhật dependency, config, tooling, build |
| `docs` | Chỉ thay đổi tài liệu |
| `perf` | Cải thiện hiệu năng |

## Scopes — Backend

| Scope | Phạm vi |
|---|---|
| `auth` | Đăng nhập, đăng ký, JWT, refresh token |
| `exercises` | Thư viện bài tập |
| `workout-plans` | Lịch tập, workout builder |
| `sessions` | Buổi tập, set logging |
| `users` | Hồ sơ người dùng |
| `coins` | Hệ thống xu, AdMob reward |
| `ai` | Claude API, prompt, AI plan generation |
| `db` | Migration, seeding, schema |
| `infra` | Pipeline behaviors, DI, middleware |

## Scopes — Frontend

| Scope | Phạm vi |
|---|---|
| `auth` | Màn hình login/register, auth store |
| `exercises` | Thư viện bài tập, explore screen |
| `workout-plans` | Builder, home screen |
| `sessions` | Active session, set row, timer |
| `progress` | Biểu đồ, progress screen |
| `ai-coach` | Chat AI, generate plan screen |
| `coins` | Coin display, rewarded ad |
| `ui` | Shared components (Button, Card, Input…) |
| `theme` | Dark/Light mode, color tokens |
| `i18n` | Dịch ngôn ngữ, locale files |
| `navigation` | Routing, tab bar, layout |

## Scopes — Chung

`setup` · `config` · `deps`

## Ví dụ hợp lệ

```
feat(auth): add JWT refresh token endpoint
fix(sessions): correct set count when exercise is skipped
feat(ui): add RestTimer component with haptic feedback
chore(deps): upgrade expo to 52.x
style(theme): apply dark mode tokens to ExerciseCard
refactor(ai): extract PromptBuilder from ClaudeAIService
feat(coins): integrate AdMob rewarded ad for coin earning
docs(setup): update MEMORY.md step 0.2 as complete
```

## Breaking Change

```
feat(auth)!: change token response format to include userId

BREAKING CHANGE: token response now returns { accessToken, refreshToken, userId }
instead of { token, refresh }
```

## Không được làm

```
# Quá chung chung
git commit -m "fix bug"
git commit -m "update code"
git commit -m "changes"

# Sai type
git commit -m "feat: fix null pointer in session handler"  # nên là fix

# Thiếu scope
git commit -m "feat: add login screen"  # nên có (auth)
```
