import { useQuery } from '@tanstack/react-query';
import { progressApi } from './api';
import { progressKeys } from './query-keys';

export function useProgress(weeksBack = 8) {
  return useQuery({
    queryKey: progressKeys.data(weeksBack),
    queryFn: () => progressApi.get(weeksBack).then((r) => r.data),
  });
}
