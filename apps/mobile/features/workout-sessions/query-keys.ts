export const sessionKeys = {
  all: ['workout-sessions'] as const,
  history: (page: number) => [...sessionKeys.all, 'history', page] as const,
};
