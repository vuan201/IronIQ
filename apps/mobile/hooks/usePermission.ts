import { useMyProfile } from '@/features/users/hooks';

export function usePermission() {
  const { data: profile } = useMyProfile();
  const isPremium = profile?.subscriptionTier === 'Premium';
  return { isPremium };
}
