import { api } from '@/lib/api';
import type { CoinBalance } from './types';

export const coinsApi = {
  getBalance: () => api.get<CoinBalance>('/coins/balance'),

  earnFromAd: (externalTransactionId: string) =>
    api.post<CoinBalance>('/coins/earn-from-ad', { externalTransactionId }),
};
