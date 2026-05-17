import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { workoutPlanKeys } from '@/features/workout-plans/query-keys';
import { aiCoachApi } from './api';
import { aiCoachKeys } from './query-keys';
import type { AskCoachDto, GeneratePlanDto } from './types';

export function useGeneratePlan() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: GeneratePlanDto) => aiCoachApi.generatePlan(data).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: workoutPlanKeys.all }),
  });
}

export function useAskCoach() {
  return useMutation({
    mutationFn: (data: AskCoachDto) => aiCoachApi.ask(data).then((r) => r.data),
  });
}

export function useProgressionSuggestions(sessionId: string | undefined) {
  return useQuery({
    queryKey: aiCoachKeys.progressionSuggestions(sessionId ?? ''),
    queryFn: () => aiCoachApi.getProgressionSuggestions(sessionId!).then((r) => r.data),
    enabled: !!sessionId,
  });
}
