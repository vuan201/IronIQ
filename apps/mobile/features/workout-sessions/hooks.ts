import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { workoutSessionsApi } from './api';
import { sessionKeys } from './query-keys';
import type { LogSetDto, StartSessionDto } from './types';

export function useSessionHistory(page = 1) {
  return useQuery({
    queryKey: sessionKeys.history(page),
    queryFn: () => workoutSessionsApi.getHistory(page).then((r) => r.data),
  });
}

export function useStartSession() {
  return useMutation({
    mutationFn: (data: StartSessionDto) => workoutSessionsApi.start(data).then((r) => r.data),
  });
}

export function useLogSet(sessionId: string) {
  return useMutation({
    mutationFn: (data: LogSetDto) => workoutSessionsApi.logSet(sessionId, data).then((r) => r.data),
  });
}

export function useCompleteSession() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ sessionId, notes }: { sessionId: string; notes?: string | null }) =>
      workoutSessionsApi.complete(sessionId, notes).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: sessionKeys.all }),
  });
}
