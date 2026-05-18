import { api } from '@/lib/api';
import type { LeaderboardEntry } from './types';

export const socialApi = {
  getLeaderboard: (limit = 20) =>
    api.get<LeaderboardEntry[]>('/leaderboard', { params: { limit } }),
};
