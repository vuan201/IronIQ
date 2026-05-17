export const coinKeys = {
  all: ['coins'] as const,
  balance: () => [...coinKeys.all, 'balance'] as const,
};
