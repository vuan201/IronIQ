import { api } from '@/lib/api';
import type { CreateWorkoutPlanDto, UpdateWorkoutPlanDto, WorkoutPlan } from './types';

export const workoutPlansApi = {
  getMyPlans: () =>
    api.get<WorkoutPlan[]>('/workout-plans'),

  create: (data: CreateWorkoutPlanDto) =>
    api.post<WorkoutPlan>('/workout-plans', data),

  update: (id: string, data: UpdateWorkoutPlanDto) =>
    api.put<WorkoutPlan>(`/workout-plans/${id}`, data),

  delete: (id: string) =>
    api.delete(`/workout-plans/${id}`),
};
