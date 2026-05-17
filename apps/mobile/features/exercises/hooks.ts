import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { exercisesApi } from './api';
import { exerciseKeys } from './query-keys';
import type { CreateExerciseDto, ExerciseFilters } from './types';

export function useExercises(filters: ExerciseFilters = {}) {
  return useQuery({
    queryKey: exerciseKeys.list(filters),
    queryFn: () => exercisesApi.getAll(filters).then((r) => r.data),
  });
}

export function useExercise(id: string) {
  return useQuery({
    queryKey: exerciseKeys.detail(id),
    queryFn: () => exercisesApi.getById(id).then((r) => r.data),
    enabled: !!id,
  });
}

export function useCreateExercise() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateExerciseDto) => exercisesApi.create(data).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: exerciseKeys.all }),
  });
}
