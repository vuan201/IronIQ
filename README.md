# IronIQ

Personal gym tracking app with AI Coach — built for iOS and Android.

## Features

- **Workout Builder** — create custom weekly plans or generate them with AI
- **Session Logging** — log sets, reps, and weight in real time with a rest timer
- **AI Coach** — Claude-powered plan generation and workout feedback
- **Progress Charts** — track strength and volume over time
- **Streak & Achievements** — stay consistent with gamified milestones
- **Coin System** — earn coins via rewarded ads, spend on AI features
- **Guest Mode** — use offline without an account, sync later

## Tech Stack

| Layer | Technology |
|---|---|
| Mobile | React Native + Expo + TypeScript |
| Routing | Expo Router (file-based) |
| UI | NativeWind (Tailwind) + React Native Paper |
| State | Zustand + TanStack Query |
| Backend | ASP.NET Core 9 — Clean Architecture + CQRS |
| Database | PostgreSQL (EF Core) |
| AI | Claude API (Anthropic) |
| Auth | JWT + Refresh Token |
| Subscription | RevenueCat |
| Ads | Google AdMob (Rewarded) |
| i18n | i18next — Vietnamese + English |

## Project Structure

```
IronIQ/
├── apps/
│   ├── mobile/     ← Expo (React Native)
│   └── backend/    ← ASP.NET Core 9 solution
└── .claude/        ← Project docs, rules, memory
```

## Roadmap

- [ ] **Phase 1** — MVP Core (Auth, Exercise Library, Workout Builder, Session Logging, AI Plan)
- [ ] **Phase 2** — Monetization (Coins, AdMob, RevenueCat Premium)
- [ ] **Phase 3** — AI Coach (Chat, Auto-progression, Session Review)
- [ ] **Phase 4** — Social (Streak, Achievements, Sharing, Leaderboard)

## Getting Started

> Setup guide coming in Phase 1.

## License

MIT
