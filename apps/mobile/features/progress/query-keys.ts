export const progressKeys = {
  all: ['progress'] as const,
  data: (weeksBack: number) => [...progressKeys.all, weeksBack] as const,
};
