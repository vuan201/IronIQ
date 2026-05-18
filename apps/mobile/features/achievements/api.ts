import { api } from '@/lib/api';
import type { Achievement } from './types';

export const achievementsApi = {
  getMyAchievements: () => api.get<Achievement[]>('/achievements'),
};
