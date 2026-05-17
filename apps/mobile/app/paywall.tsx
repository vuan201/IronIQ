import { useRouter } from 'expo-router';
import { ActivityIndicator, Alert, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTranslation } from 'react-i18next';
import { useQueryClient } from '@tanstack/react-query';
import { useTheme } from '@/hooks/useTheme';
import { usePurchaseSubscription, useRevenueCatOfferings } from '@/features/subscription/hooks';
import { userKeys } from '@/features/users/query-keys';

const BENEFITS = ['paywall.benefit1', 'paywall.benefit2', 'paywall.benefit3'] as const;

export default function PaywallScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const router = useRouter();
  const queryClient = useQueryClient();
  const { packages, loading } = useRevenueCatOfferings();
  const { purchase, restore, purchasing, error } = usePurchaseSubscription();

  const handlePurchase = async (rawPackage: unknown) => {
    const success = await purchase(rawPackage);
    if (success) {
      await queryClient.invalidateQueries({ queryKey: userKeys.me() });
      Alert.alert('', t('paywall.successMessage'));
      router.back();
    }
  };

  const handleRestore = async () => {
    const success = await restore();
    if (success) {
      await queryClient.invalidateQueries({ queryKey: userKeys.me() });
      Alert.alert('', t('paywall.restoreSuccess'));
      router.back();
    } else if (!error) {
      Alert.alert('', t('paywall.restoreNotFound'));
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <ScrollView contentContainerStyle={{ padding: 24 }}>
        <TouchableOpacity onPress={router.back} style={{ marginBottom: 16 }}>
          <Text style={{ color: colors.primary, fontSize: 15 }}>← {t('common.cancel')}</Text>
        </TouchableOpacity>

        <Text style={{ fontSize: 28, fontWeight: '800', color: colors.text, marginBottom: 8 }}>
          {t('paywall.title')}
        </Text>
        <Text style={{ fontSize: 15, color: colors.textSecondary, marginBottom: 32 }}>
          {t('paywall.subtitle')}
        </Text>

        <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 20, marginBottom: 32 }}>
          {BENEFITS.map((key) => (
            <View key={key} style={{ flexDirection: 'row', alignItems: 'center', marginBottom: 12 }}>
              <Text style={{ color: colors.primary, fontSize: 16, marginRight: 10 }}>✓</Text>
              <Text style={{ color: colors.text, fontSize: 15 }}>{t(key)}</Text>
            </View>
          ))}
        </View>

        {loading ? (
          <ActivityIndicator color={colors.primary} style={{ marginBottom: 24 }} />
        ) : packages.length === 0 ? (
          <Text style={{ color: colors.textSecondary, textAlign: 'center', marginBottom: 24 }}>
            {t('paywall.notAvailable')}
          </Text>
        ) : (
          <View style={{ gap: 12, marginBottom: 24 }}>
            {packages.map((pkg) => (
              <TouchableOpacity
                key={pkg.identifier}
                onPress={() => handlePurchase(pkg.rawPackage)}
                disabled={purchasing}
                style={{
                  backgroundColor: colors.primary,
                  borderRadius: 14,
                  padding: 18,
                  alignItems: 'center',
                }}
              >
                {purchasing ? (
                  <ActivityIndicator color="#fff" />
                ) : (
                  <>
                    <Text style={{ color: '#fff', fontSize: 16, fontWeight: '700' }}>
                      {t(`paywall.${pkg.period}`)}
                    </Text>
                    <Text style={{ color: 'rgba(255,255,255,0.85)', fontSize: 14, marginTop: 2 }}>
                      {pkg.priceString}
                    </Text>
                  </>
                )}
              </TouchableOpacity>
            ))}
          </View>
        )}

        {error && (
          <Text style={{ color: '#EF4444', textAlign: 'center', marginBottom: 12 }}>{error}</Text>
        )}

        <TouchableOpacity onPress={handleRestore} disabled={purchasing} style={{ alignItems: 'center' }}>
          <Text style={{ color: colors.textSecondary, fontSize: 14 }}>{t('paywall.restore')}</Text>
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
}
