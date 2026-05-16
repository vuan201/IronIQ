# Quy tắc Frontend — React Native + Expo

## Kiến trúc

```
app/          ← Expo Router screens (file = route)
features/     ← Business logic theo feature
components/   ← Shared UI (không chứa business logic)
lib/          ← Utilities thuần túy
constants/    ← Colors, typography, spacing
hooks/        ← Global hooks
locales/      ← i18n translations
```

## Quy tắc Screen (app/)

- Screen chỉ được import từ `features/` và `components/` — không gọi API trực tiếp
- Không chứa business logic — chỉ layout + event handlers
- Mọi data fetching qua hooks từ `features/`

```tsx
// ✅ Đúng
export default function ExploreScreen() {
  const { exercises, isLoading } = useExercises({ muscle: 'chest' })
  return <FlatList data={exercises} renderItem={({ item }) => <ExerciseCard exercise={item} />} />
}

// ❌ Sai — gọi API trực tiếp trong screen
export default function ExploreScreen() {
  const [exercises, setExercises] = useState([])
  useEffect(() => {
    axios.get('/exercises').then(res => setExercises(res.data))
  }, [])
}
```

## Quy tắc Feature (features/<name>/)

Mỗi feature có đủ 4 file:

```
features/exercises/
├── api.ts          ← Axios calls — chỉ HTTP, không state
├── hooks.ts        ← TanStack Query hooks (useQuery, useMutation)
├── query-keys.ts   ← Key factory cho cache invalidation
├── store.ts        ← Zustand (chỉ khi cần client state)
└── types.ts        ← TypeScript types/interfaces
```

### api.ts — chỉ HTTP calls
```ts
// ✅ Đúng — thuần HTTP, không state
export const exerciseApi = {
  getAll: (filters: ExerciseFilters) =>
    api.get<ExercisesResponse>('/exercises', { params: filters }),
  getById: (id: string) =>
    api.get<Exercise>(`/exercises/${id}`),
  create: (data: CreateExerciseDto) =>
    api.post<Exercise>('/exercises', data),
}
```

### hooks.ts — TanStack Query
```ts
export function useExercises(filters: ExerciseFilters) {
  return useQuery({
    queryKey: exerciseKeys.list(filters),
    queryFn: () => exerciseApi.getAll(filters).then(r => r.data),
  })
}

export function useCreateExercise() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: exerciseApi.create,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: exerciseKeys.all }),
  })
}
```

### query-keys.ts — Key factory
```ts
export const exerciseKeys = {
  all: ['exercises'] as const,
  list: (filters: ExerciseFilters) => [...exerciseKeys.all, 'list', filters] as const,
  detail: (id: string) => [...exerciseKeys.all, 'detail', id] as const,
}
```

## Quy tắc Component (components/)

- Component không import từ `features/` — chỉ nhận props
- Không gọi API, không đọc store Zustand
- Nếu cần data → nhận qua props từ screen/hook

```tsx
// ✅ Đúng — pure component nhận props
interface ExerciseCardProps {
  exercise: Exercise
  onPress: (id: string) => void
}
export function ExerciseCard({ exercise, onPress }: ExerciseCardProps) { ... }

// ❌ Sai — component tự fetch data
export function ExerciseCard({ id }: { id: string }) {
  const { data } = useExercises()  // vi phạm — component không được dùng hooks business
}
```

## TypeScript

- **Không dùng `any`** — dùng `unknown` nếu không biết type, rồi narrow
- Mọi props phải có type rõ ràng (`interface` hoặc `type`)
- API response types định nghĩa trong `features/<name>/types.ts`
- Không dùng `!` (non-null assertion) trừ khi chắc chắn 100%

## Internationalisation (i18n)

- **Không hardcode text hiển thị** — mọi string dùng `t('key')`
- Key format: `feature.component.label` — ví dụ `exercises.card.muscleGroup`
- Thêm key vào cả `vi.json` và `en.json` cùng lúc

```tsx
// ✅ Đúng
const { t } = useTranslation()
<Text>{t('exercises.card.difficulty')}</Text>

// ❌ Sai
<Text>Độ khó</Text>
<Text>Difficulty</Text>
```

## Dark / Light Mode

- **Luôn dùng NativeWind `dark:` class** — không dùng conditional style
- Màu sắc chỉ lấy từ `constants/colors.ts` — không hardcode hex trong component
- Theme state đọc từ `hooks/useTheme.ts`

```tsx
// ✅ Đúng
<View className="bg-white dark:bg-[#1A1A1A] rounded-2xl p-4">
  <Text className="text-[#111111] dark:text-[#F0F0F0]">Bench Press</Text>
</View>

// ❌ Sai — conditional style
<View style={{ backgroundColor: isDark ? '#1A1A1A' : '#FFFFFF' }}>
```

## State Management

| Loại state | Dùng gì |
|---|---|
| Server data (API) | TanStack Query |
| Active session (in-progress workout) | Zustand |
| Auth tokens | Zustand + MMKV persist |
| AI conversation history | Zustand |
| UI state local (modal open/close) | `useState` |
| Form state | `useState` hoặc `react-hook-form` |

**Không dùng Zustand cho server data** — đó là việc của TanStack Query.

## Naming Conventions

| Thành phần | Convention | Ví dụ |
|---|---|---|
| Screen file | kebab-case | `workout-create.tsx` |
| Component file | PascalCase | `ExerciseCard.tsx` |
| Hook | `use` prefix | `useExercises`, `useTheme` |
| Store | `use{Feature}Store` | `useSessionStore` |
| Type/Interface | PascalCase | `Exercise`, `WorkoutPlan` |
| Constant | SCREAMING_SNAKE | `MAX_SETS_PER_EXERCISE` |
| Locales key | dot.notation | `auth.login.emailPlaceholder` |
