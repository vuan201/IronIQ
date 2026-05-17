import { api } from '@/lib/api';
import type { WorkoutPlan } from '@/features/workout-plans/types';
import type { AskCoachDto, CoachResponseDto, GeneratePlanDto, ProgressionSuggestion, SessionReview } from './types';

export const aiCoachApi = {
  generatePlan: (data: GeneratePlanDto) =>
    api.post<WorkoutPlan>('/workout-plans/generate', data),
  ask: (data: AskCoachDto) =>
    api.post<CoachResponseDto>('/ai-coach/ask', data),
  getProgressionSuggestions: (sessionId: string) =>
    api.get<ProgressionSuggestion[]>(`/ai-coach/progression-suggestions/${sessionId}`),
  getSessionReview: (sessionId: string) =>
    api.get<SessionReview>(`/ai-coach/session-review/${sessionId}`),
};
