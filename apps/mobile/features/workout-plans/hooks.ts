import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { workoutPlansApi } from './api';
import { workoutPlanKeys } from './query-keys';
import type { CreateWorkoutPlanDto, UpdateWorkoutPlanDto } from './types';

export function useWorkoutPlans() {
  return useQuery({
    queryKey: workoutPlanKeys.list(),
    queryFn: () => workoutPlansApi.getMyPlans().then((r) => r.data),
  });
}

export function useCreateWorkoutPlan() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateWorkoutPlanDto) => workoutPlansApi.create(data).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: workoutPlanKeys.all }),
  });
}

export function useUpdateWorkoutPlan() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateWorkoutPlanDto }) =>
      workoutPlansApi.update(id, data).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: workoutPlanKeys.all }),
  });
}

export function useDeleteWorkoutPlan() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => workoutPlansApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: workoutPlanKeys.all }),
  });
}
