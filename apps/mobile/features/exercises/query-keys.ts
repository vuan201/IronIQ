import type { ExerciseFilters } from './types';

export const exerciseKeys = {
  all: ['exercises'] as const,
  list: (filters: ExerciseFilters) => [...exerciseKeys.all, 'list', filters] as const,
  detail: (id: string) => [...exerciseKeys.all, 'detail', id] as const,
};
