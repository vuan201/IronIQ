# Quy tắc Workflow

## Trước khi bắt đầu một bước

1. Đọc `.claude/MEMORY.md` — xác định bước tiếp theo (`[ ]`)
2. Đánh dấu `[~]` cho bước đang làm
3. Đọc file liên quan: `rules/backend.md` hoặc `rules/frontend.md`
4. Nếu không chắc về cách làm → hỏi trước, không đoán

## Sau khi hoàn thành một bước

```
1. Kiểm tra  →  chạy lại tính năng, đảm bảo không break gì
2. Commit    →  theo rules/commits.md, scope khớp với bước vừa làm
3. Cập nhật  →  đánh dấu [x] trong .claude/MEMORY.md, ghi commit hash vào bảng Log
4. Checkpoint → tóm tắt: đã làm gì | đã xác minh gì | còn lại gì
```

## Checkpoint format

```
✅ Hoàn thành: <tên bước>
🔍 Đã xác minh: <những gì đã test/chạy>
⏭️ Tiếp theo: <bước kế tiếp trong MEMORY.md>
```

## Cập nhật MEMORY.md

Khi đánh dấu hoàn thành, cập nhật đồng thời 2 chỗ:

```markdown
# Trong checklist — đổi [~] → [x]
- [x] Tạo RegisterCommand + RegisterCommandHandler + RegisterValidator

# Trong bảng Log — thêm dòng mới
| 2026-05-17 | 1.1 Auth Backend — Register | abc1234 |
```

## Khi gặp lỗi / bị chặn

- **Dừng lại** — không tiếp tục sang bước khác khi đang có lỗi
- Mô tả rõ: lỗi gì, ở đâu, đã thử gì
- Nếu cần thay đổi kế hoạch → cập nhật MEMORY.md trước khi làm

## Quy tắc không làm

- Không commit code chưa chạy được
- Không bỏ qua bước validation / test
- Không "cải thiện" code lân cận khi chỉ được giao một bước cụ thể
- Không tạo file ngoài thư mục dự án trừ khi được yêu cầu rõ ràng
- Không push lên remote trừ khi được yêu cầu

## Khi thêm dependency mới

1. Ghi rõ lý do cần dependency đó
2. Kiểm tra xem có thứ gì đã có sẵn giải quyết được không
3. Cập nhật commit với type `chore(deps):`
