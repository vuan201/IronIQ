# Design System — IronIQ

## Triết lý Thiết kế
- **Energetic but clean** — giao diện gym phải có năng lượng, nhưng không rối mắt
- **Content-first** — data (sets, reps, cân nặng) luôn là trung tâm, UI là nền
- **Dark mode ưu tiên** — người tập trong phòng gym thường dùng dark mode
- **Tối thiểu thao tác** — trong lúc tập tay đang bẩn/mệt, số tap phải ít nhất có thể

---

## Màu sắc (Color Tokens)

### Primary Palette

| Token | Light | Dark | Dùng cho |
|---|---|---|---|
| `primary` | `#FF6B35` | `#FF6B35` | CTA, active state, accent |
| `primary-dim` | `#FF8C5A` | `#CC4A1A` | Pressed state |
| `primary-subtle` | `#FFF0EB` | `#2A1A13` | Background của primary element |

### Neutral Palette

| Token | Light | Dark | Dùng cho |
|---|---|---|---|
| `bg` | `#FFFFFF` | `#0F0F0F` | Nền màn hình chính |
| `surface` | `#F5F5F5` | `#1A1A1A` | Card, bottom sheet, input |
| `surface-2` | `#EBEBEB` | `#242424` | Nested surface, hover state |
| `border` | `#E0E0E0` | `#2E2E2E` | Divider, input border |
| `text` | `#111111` | `#F0F0F0` | Body text |
| `text-secondary` | `#6B6B6B` | `#9A9A9A` | Placeholder, label phụ |
| `text-disabled` | `#BBBBBB` | `#4A4A4A` | Disabled state |

### Semantic Palette

| Token | Value | Dùng cho |
|---|---|---|
| `success` | `#22C55E` | Hoàn thành set, streak active |
| `warning` | `#F59E0B` | Cảnh báo nhẹ, gần hết xu |
| `danger` | `#EF4444` | Xóa, hành động nguy hiểm |
| `info` | `#3B82F6` | AI tip, thông báo trung tính |

### Màu nhóm cơ (cho Exercise tags)
```
Chest     #EF4444   Lưng      #3B82F6
Vai       #8B5CF6   Tay trước #F59E0B
Tay sau   #EC4899   Bụng      #10B981
Chân      #F97316   Toàn thân #6366F1
Cardio    #14B8A6
```

---

## Typography

**Font chính:** `Inter` (system sans-serif fallback)
**Font số/dữ liệu:** `Inter` với `font-variant-numeric: tabular-nums` — số không nhảy khi đếm

### Scale

| Token | Size | Weight | Line Height | Dùng cho |
|---|---|---|---|---|
| `display` | 32px | 800 | 1.1 | Số lớn (timer, tổng volume) |
| `h1` | 24px | 700 | 1.2 | Tiêu đề màn hình |
| `h2` | 20px | 600 | 1.3 | Section header |
| `h3` | 17px | 600 | 1.4 | Card title, bài tập |
| `body` | 15px | 400 | 1.5 | Nội dung chính |
| `body-sm` | 13px | 400 | 1.5 | Mô tả, label phụ |
| `caption` | 11px | 500 | 1.4 | Tag, badge, metadata |

---

## Spacing System

Base unit: **4px**

```
space-1  =  4px
space-2  =  8px
space-3  = 12px
space-4  = 16px   ← margin/padding màn hình
space-5  = 20px
space-6  = 24px   ← khoảng cách section
space-8  = 32px
space-10 = 40px
space-12 = 48px   ← bottom safe area + tab bar
```

**Safe area padding:** 16px ngang, 24px dọc (trừ bottom dành cho tab bar)

---

## Border Radius

```
radius-sm  =  6px   ← Badge, tag nhỏ
radius-md  = 12px   ← Button, Input, Card nhỏ
radius-lg  = 16px   ← Card lớn, bottom sheet
radius-xl  = 24px   ← Modal, sheet toàn màn hình
radius-full = 9999px ← Avatar, pill button
```

---

## Shadow (chỉ dùng ở Light mode)

```
shadow-sm:  0 1px 3px rgba(0,0,0,0.08)   ← Card nhỏ
shadow-md:  0 4px 12px rgba(0,0,0,0.10)  ← Floating button, modal
shadow-lg:  0 8px 24px rgba(0,0,0,0.12)  ← Bottom sheet
```

Dark mode: không dùng shadow, thay bằng `border` màu `#2E2E2E`

---

## Components

### Button

```
Variant    │ Background      │ Text         │ Border
───────────┼─────────────────┼──────────────┼──────────────
primary    │ primary         │ white        │ none
secondary  │ surface-2       │ text         │ none
outline    │ transparent     │ primary      │ primary (1px)
ghost      │ transparent     │ text         │ none
danger     │ danger          │ white        │ none
```

**Kích thước:**
- `sm` — height 36px, padding H 12px, text body-sm
- `md` — height 48px, padding H 20px, text body (default)
- `lg` — height 56px, padding H 24px, text h3

**Trạng thái:**
- Pressed: `opacity 0.8` + scale `0.97` (animation 100ms)
- Disabled: `opacity 0.4`, không cho press
- Loading: spinner thay icon/text

---

### Input

```
height: 52px
background: surface
border: 1px solid border
border-radius: radius-md
padding: 0 16px
font: body

Focus:  border-color = primary, border-width = 1.5px
Error:  border-color = danger
        helper text màu danger bên dưới
```

---

### Card

```
background: surface
border-radius: radius-lg
padding: 16px
shadow-sm (light) / border 1px solid border (dark)
```

**Exercise Card** (trong thư viện):
```
┌─────────────────────────────────┐
│  [Icon cơ]  Bench Press    [>]  │
│             Ngực · Trung cấp    │
│  ████ Ngực  ██ Vai  █ Tay sau  │
└─────────────────────────────────┘
```

**Workout Day Card** (trong lịch tập):
```
┌──────────────────────────────────┐
│  Thứ 2  ·  Push Day         [+] │
│  ─────────────────────────────  │
│  ○  Bench Press    4×8   60kg   │
│  ○  Overhead Press 3×10  40kg   │
│  ○  Lateral Raise  3×15  10kg   │
│                                  │
│  [Bắt đầu tập]                  │
└──────────────────────────────────┘
```

**Set Row** (trong session đang tập):
```
Set │  Kg   │  Reps  │  ✓
 1  │  60   │   8    │  ✓  (completed — màu success)
 2  │  60   │   8    │  ○  (active)
 3  │  60   │   8    │  ○
```

---

### Rest Timer

Hiển thị toàn màn hình khi đang nghỉ:
```
         2:30 nghỉ còn lại
    ╔══════════════════════╗
    ║                      ║
    ║        1:47          ║  ← countdown lớn, display font
    ║    ████████████░░    ║  ← progress bar tròn
    ║                      ║
    ╚══════════════════════╝
      [Bỏ qua]   [+30s]
```

---

### Bottom Tab Bar

5 tab: **Hôm nay · Khám phá · Tập · Tiến trình · Tôi**

```
[🏠]    [🔍]    [⊕]    [📈]    [👤]
Today  Explore  Start  Progress  Me
```
- Tab giữa `⊕` lớn hơn, màu primary, hơi nổi lên (floating effect)
- Active tab: icon + label màu primary
- Inactive: màu text-secondary

---

### Streak Badge
```
🔥 12 ngày  ← số + flame icon, màu warning
```

### Coin Display
```
⬡ 250 xu   ← hexagon icon + số, màu #F59E0B
```

### Achievement Badge
```
┌──────────┐
│    🏆    │
│ 100 buổi │
│ tập gym  │
└──────────┘
Locked: grayscale + opacity 0.4
```

---

## Icon Library

**Dùng:** `@expo/vector-icons` → `Ionicons` (phổ biến, đầy đủ, React Native)

| Màn hình | Icon chính |
|---|---|
| Home | `home` / `home-outline` |
| Explore | `search` |
| Start Workout | `add-circle` |
| Progress | `stats-chart` |
| Profile | `person` |
| Exercise | `barbell` |
| AI Coach | `sparkles` |
| Timer | `timer` |
| Streak | `flame` |
| Settings | `settings` |
| Dark mode | `moon` / `sunny` |
| Premium | `star` |
| Coin | `hexagon` |

---

## Navigation & Layout

### Tab Navigator (main)
```
Root
 ├── (auth) — Stack: Login → Register
 ├── (guest) — Stack: Onboarding
 └── (tabs) — Bottom Tabs
      ├── Home (index)
      ├── Explore
      ├── [Quick Start — Modal fullscreen]
      ├── Progress
      └── Profile
           ├── Settings
           ├── Language
           └── Theme

Workout session → Modal fullscreen (không thấy tab bar khi đang tập)
AI Coach → Modal slide-up
```

---

## Màn hình chính — Layout mô tả

### Home Screen
```
┌─────────────────────────────────┐
│  Chào buổi sáng, Vũ  🔥12      │  ← greeting + streak
│  ─────────────────────────────  │
│  ╔═══════════════════════════╗  │
│  ║  HÔM NAY: Push Day        ║  │  ← today's workout card
│  ║  5 bài · ~45 phút         ║  │
│  ║  [Bắt đầu tập]            ║  │
│  ╚═══════════════════════════╝  │
│                                  │
│  Tiến trình tuần này             │
│  M  T  W  T  F  S  S            │
│  ✓  ✓  ✓  ○  ○  ○  ○           │
│                                  │
│  Gợi ý từ AI Coach              │
│  ┌──────────────────────────┐   │
│  │ 💡 Bạn tăng 5kg bench    │   │
│  │    press tuần này, tiếp  │   │
│  │    tục phát huy!         │   │
│  └──────────────────────────┘   │
└─────────────────────────────────┘
```

### Active Session Screen
```
┌─────────────────────────────────┐
│  ← Push Day          00:32:15  │  ← elapsed time
│  ─────────────────────────────  │
│  Bench Press (2/5)              │  ← exercise progress
│                                  │
│  Set │  Kg   │ Reps │           │
│   1  │  60   │   8  │  ✓       │
│   2  │  60   │   8  │  ◉       │  ← active set
│   3  │  60   │   8  │  ○       │
│   4  │  60   │   6  │  ○       │
│                                  │
│  [  Hoàn thành set  ]           │  ← primary CTA
│                                  │
│  ─────────────────────────────  │
│  Bài tiếp: Overhead Press       │
└─────────────────────────────────┘
```

---

## Animation & Motion

- **Duration ngắn:** 150ms — hover, press, toggle
- **Duration vừa:** 300ms — transition màn hình, expand/collapse
- **Duration dài:** 500ms — celebration, achievement unlock
- **Easing:** `ease-out` cho mọi transition (cảm giác responsive)
- **Spring animation** cho Bottom Sheet (react-native-reanimated)
- **Celebration:** confetti nhỏ khi unlock achievement hoặc hoàn thành workout

**Tránh:** animation phức tạp trong màn hình session (người dùng cần tập trung)

---

## Dark / Light Mode

- Mặc định theo system
- User có thể override trong Settings (lưu MMKV)
- Toggle trong Profile screen
- **Không** có "auto by time" (quá phức tạp, không cần)

NativeWind class mẫu:
```tsx
// Surface card
<View className="bg-[#F5F5F5] dark:bg-[#1A1A1A] rounded-2xl p-4">

// Primary text
<Text className="text-[#111111] dark:text-[#F0F0F0] font-semibold">

// Primary button
<TouchableOpacity className="bg-[#FF6B35] active:opacity-80 rounded-xl h-12 items-center justify-center">
```

---

## Responsive & Accessibility

- Minimum touch target: **44×44px** (Apple HIG)
- Font không scale theo system (tránh layout vỡ) — dùng `allowFontScaling={false}` cho số quan trọng
- Contrast ratio tối thiểu: 4.5:1 (WCAG AA)
- Haptic feedback: nhẹ khi tap, mạnh hơn khi hoàn thành set

---

## Câu hỏi còn mở về Design

- [x] Tên app → **IronIQ**
- [ ] Có custom font (VD: Geist, DM Sans) hay dùng Inter/system?
- [ ] Splash screen style: full-color hay minimal white/black?
