export const aiCoachKeys = {
  progressionSuggestions: (sessionId: string) =>
    ['ai-coach', 'progression-suggestions', sessionId] as const,
  sessionReview: (sessionId: string) =>
    ['ai-coach', 'session-review', sessionId] as const,
};
