import { useCallback, useEffect, useRef, useState } from 'react';
import { Platform } from 'react-native';
import { RewardedAd, RewardedAdEventType, TestIds } from 'react-native-google-mobile-ads';

const AD_UNIT_ID = __DEV__
  ? TestIds.REWARDED
  : Platform.OS === 'ios'
    ? 'ca-app-pub-REPLACE_ME/REPLACE_ME_IOS'
    : 'ca-app-pub-REPLACE_ME/REPLACE_ME_ANDROID';

export function useRewardedAd(onRewarded: (transactionId: string) => void) {
  const [loaded, setLoaded] = useState(false);
  const [loading, setLoading] = useState(false);
  const adRef = useRef<RewardedAd | null>(null);
  const onRewardedRef = useRef(onRewarded);
  onRewardedRef.current = onRewarded;

  const loadAd = useCallback(() => {
    const ad = RewardedAd.createForAdRequest(AD_UNIT_ID, {
      requestNonPersonalizedAdsOnly: true,
    });
    adRef.current = ad;
    setLoaded(false);
    setLoading(true);

    const unsubscribeLoaded = ad.addAdEventListener(RewardedAdEventType.LOADED, () => {
      setLoaded(true);
      setLoading(false);
    });

    const unsubscribeEarned = ad.addAdEventListener(RewardedAdEventType.EARNED_REWARD, () => {
      const txId = `admob_${Date.now()}_${Math.random().toString(36).slice(2, 9)}`;
      onRewardedRef.current(txId);
      setLoaded(false);
      loadAd();
    });

    ad.load();

    return () => {
      unsubscribeLoaded();
      unsubscribeEarned();
    };
  }, []);

  useEffect(() => {
    const cleanup = loadAd();
    return cleanup;
  }, [loadAd]);

  const showAd = useCallback(() => {
    if (loaded && adRef.current) {
      adRef.current.show();
    }
  }, [loaded]);

  return { loaded, loading, showAd };
}
