import { useQuery } from '@tanstack/react-query';
import { achievementsApi } from './api';
import { achievementKeys } from './query-keys';

export function useMyAchievements() {
  return useQuery({
    queryKey: achievementKeys.my(),
    queryFn: () => achievementsApi.getMyAchievements().then((r) => r.data),
  });
}

export function useNewlyUnlockedAchievements(windowMs = 5 * 60 * 1000) {
  const { data } = useMyAchievements();
  if (!data) return [];
  const cutoff = Date.now() - windowMs;
  return data.filter(
    (a) => a.isUnlocked && a.unlockedAt && new Date(a.unlockedAt).getTime() >= cutoff,
  );
}
