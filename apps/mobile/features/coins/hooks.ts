import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { coinsApi } from './api';
import { coinKeys } from './query-keys';

export function useCoinBalance() {
  return useQuery({
    queryKey: coinKeys.balance(),
    queryFn: () => coinsApi.getBalance().then((r) => r.data),
  });
}

export function useEarnFromAd() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (externalTransactionId: string) =>
      coinsApi.earnFromAd(externalTransactionId).then((r) => r.data),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: coinKeys.all }),
  });
}
