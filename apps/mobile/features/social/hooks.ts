import { useQuery } from '@tanstack/react-query';
import { socialApi } from './api';
import { socialKeys } from './query-keys';

export function useLeaderboard(limit = 20) {
  return useQuery({
    queryKey: [...socialKeys.leaderboard, limit],
    queryFn: () => socialApi.getLeaderboard(limit).then((r) => r.data),
  });
}
