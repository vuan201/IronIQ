export type SubscriptionTier = 'Free' | 'Premium';

export interface SubscriptionPackageInfo {
  identifier: string;
  priceString: string;
  period: 'monthly' | 'annual';
  rawPackage: unknown;
}
