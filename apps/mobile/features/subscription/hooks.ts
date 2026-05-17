import { useCallback, useEffect, useState } from 'react';
import Purchases, { LOG_LEVEL, PurchasesPackage } from 'react-native-purchases';
import { Platform } from 'react-native';
import type { SubscriptionPackageInfo } from './types';

const RC_API_KEY = Platform.select({
  ios: 'appl_YOUR_REVENUECAT_IOS_KEY',
  android: 'goog_YOUR_REVENUECAT_ANDROID_KEY',
}) ?? '';

export function useConfigureRevenueCat(userId: string | null) {
  useEffect(() => {
    if (!RC_API_KEY) return;
    Purchases.setLogLevel(LOG_LEVEL.DEBUG);
    Purchases.configure({ apiKey: RC_API_KEY });
  }, []);

  useEffect(() => {
    if (!userId) return;
    Purchases.logIn(userId).catch(() => {});
  }, [userId]);
}

export function useRevenueCatOfferings() {
  const [packages, setPackages] = useState<SubscriptionPackageInfo[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Purchases.getOfferings()
      .then((offerings) => {
        const current = offerings.current;
        if (!current) return;
        const mapped: SubscriptionPackageInfo[] = current.availablePackages.map((pkg) => ({
          identifier: pkg.identifier,
          priceString: pkg.product.priceString,
          period: pkg.identifier.toLowerCase().includes('annual') ? 'annual' : 'monthly',
          rawPackage: pkg,
        }));
        setPackages(mapped);
      })
      .catch(() => {})
      .finally(() => setLoading(false));
  }, []);

  return { packages, loading };
}

export function usePurchaseSubscription() {
  const [purchasing, setPurchasing] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const purchase = useCallback(async (rawPackage: unknown): Promise<boolean> => {
    setPurchasing(true);
    setError(null);
    try {
      const { customerInfo } = await Purchases.purchasePackage(rawPackage as PurchasesPackage);
      return !!customerInfo.entitlements.active['premium'];
    } catch (e: unknown) {
      if (e instanceof Error && 'userCancelled' in e) return false;
      setError('Purchase failed. Please try again.');
      return false;
    } finally {
      setPurchasing(false);
    }
  }, []);

  const restore = useCallback(async (): Promise<boolean> => {
    setPurchasing(true);
    setError(null);
    try {
      const customerInfo = await Purchases.restorePurchases();
      return !!customerInfo.entitlements.active['premium'];
    } catch {
      setError('Restore failed. Please try again.');
      return false;
    } finally {
      setPurchasing(false);
    }
  }, []);

  return { purchase, restore, purchasing, error };
}
