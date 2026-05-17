import { api } from '@/lib/api';
import type { ProgressData } from './types';

export const progressApi = {
  get: (weeksBack = 8) =>
    api.get<ProgressData>('/progress', { params: { weeksBack } }),
};
