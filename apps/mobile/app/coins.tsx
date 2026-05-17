import { useCallback, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Alert, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useCoinBalance, useEarnFromAd } from '@/features/coins/hooks';
import { useTheme } from '@/hooks/useTheme';
import { useRewardedAd } from '@/hooks/useRewardedAd';

export default function CoinsScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const { data, isLoading } = useCoinBalance();
  const earnFromAd = useEarnFromAd();
  const [claiming, setClaiming] = useState(false);

  const handleRewarded = useCallback(
    async (transactionId: string) => {
      setClaiming(true);
      try {
        await earnFromAd.mutateAsync(transactionId);
        Alert.alert('', t('coins.earnSuccess'));
      } catch (err: unknown) {
        const message =
          err instanceof Error && err.message.includes('409')
            ? t('coins.alreadyClaimed')
            : t('coins.earnError');
        Alert.alert('', message);
      } finally {
        setClaiming(false);
      }
    },
    [earnFromAd, t]
  );

  const { loaded: adLoaded, loading: adLoading, showAd } = useRewardedAd(handleRewarded);

  const canWatch = adLoaded && !claiming;

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ flex: 1, paddingHorizontal: 24, paddingTop: 24 }}>
        <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text, marginBottom: 32 }}>
          {t('coins.title')}
        </Text>

        {/* Balance card */}
        <View
          style={{
            backgroundColor: colors.surface,
            borderRadius: 20,
            padding: 28,
            alignItems: 'center',
            marginBottom: 24,
            borderWidth: 1,
            borderColor: colors.border,
          }}
        >
          <Text style={{ fontSize: 14, color: colors.textSecondary, marginBottom: 8 }}>
            {t('coins.balance')}
          </Text>
          {isLoading ? (
            <ActivityIndicator color={colors.primary} />
          ) : (
            <Text style={{ fontSize: 56, fontWeight: '800', color: colors.primary }}>
              {data?.balance ?? 0}
            </Text>
          )}
          <Text style={{ fontSize: 20, color: colors.textSecondary, marginTop: 4 }}>🪙</Text>
        </View>

        {/* Info text */}
        <Text
          style={{
            fontSize: 14,
            color: colors.textSecondary,
            textAlign: 'center',
            marginBottom: 20,
            lineHeight: 20,
          }}
        >
          {t('coins.earnInfo')}
        </Text>

        {/* Watch ad button */}
        <TouchableOpacity
          onPress={showAd}
          disabled={!canWatch}
          style={{
            backgroundColor: canWatch ? colors.primary : colors.surface2,
            borderRadius: 14,
            paddingVertical: 16,
            alignItems: 'center',
          }}
        >
          {adLoading || claiming ? (
            <ActivityIndicator color={canWatch ? '#fff' : colors.textSecondary} />
          ) : (
            <Text
              style={{
                color: canWatch ? '#fff' : colors.textSecondary,
                fontSize: 16,
                fontWeight: '700',
              }}
            >
              {adLoaded ? t('coins.watchAd') : t('coins.adNotReady')}
            </Text>
          )}
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}
