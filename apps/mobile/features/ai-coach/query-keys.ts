export const aiCoachKeys = {
  progressionSuggestions: (sessionId: string) =>
    ['ai-coach', 'progression-suggestions', sessionId] as const,
};
