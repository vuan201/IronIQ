import { api } from '@/lib/api';
import type { WorkoutPlan } from '@/features/workout-plans/types';
import type { GeneratePlanDto } from './types';

export const aiCoachApi = {
  generatePlan: (data: GeneratePlanDto) =>
    api.post<WorkoutPlan>('/workout-plans/generate', data),
};
