export const achievementKeys = {
  all: ['achievements'] as const,
  my: () => [...achievementKeys.all, 'my'] as const,
};
