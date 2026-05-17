import { api } from '@/lib/api';
import type { LogSetDto, SessionHistoryPage, SetLog, StartSessionDto, WorkoutSession } from './types';

export const workoutSessionsApi = {
  getHistory: (page = 1, pageSize = 20) =>
    api.get<SessionHistoryPage>('/workout-sessions', { params: { page, pageSize } }),

  start: (data: StartSessionDto) =>
    api.post<WorkoutSession>('/workout-sessions', data),

  logSet: (sessionId: string, data: LogSetDto) =>
    api.post<SetLog>(`/workout-sessions/${sessionId}/sets`, data),

  complete: (sessionId: string, notes?: string | null) =>
    api.post<WorkoutSession>(`/workout-sessions/${sessionId}/complete`, { notes: notes ?? null }),
};
