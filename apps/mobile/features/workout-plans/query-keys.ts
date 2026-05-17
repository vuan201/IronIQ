export const workoutPlanKeys = {
  all: ['workout-plans'] as const,
  list: () => [...workoutPlanKeys.all, 'list'] as const,
  detail: (id: string) => [...workoutPlanKeys.all, 'detail', id] as const,
};
