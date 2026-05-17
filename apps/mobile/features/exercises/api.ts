import { api } from '@/lib/api';
import type { CreateExerciseDto, Exercise, ExerciseFilters, ExercisesPage } from './types';

export const exercisesApi = {
  getAll: (filters: ExerciseFilters = {}) =>
    api.get<ExercisesPage>('/exercises', { params: filters }),

  getById: (id: string) =>
    api.get<Exercise>(`/exercises/${id}`),

  create: (data: CreateExerciseDto) =>
    api.post<Exercise>('/exercises', data),
};
