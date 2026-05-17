import { View, Text, TouchableOpacity } from 'react-native';
import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { useTheme } from '@/hooks/useTheme';

export default function OnboardingScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();

  return (
    <View style={{ flex: 1, backgroundColor: colors.bg, paddingHorizontal: 24 }}>
      <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
        <Text style={{ fontSize: 48, fontWeight: '800', color: '#FF6B35', marginBottom: 8 }}>
          IronIQ
        </Text>
        <Text style={{ fontSize: 17, color: colors.textSecondary, textAlign: 'center' }}>
          {t('onboarding.subtitle')}
        </Text>
      </View>

      <View style={{ paddingBottom: 48, gap: 12 }}>
        <TouchableOpacity
          onPress={() => router.replace('/(auth)/login')}
          style={{
            backgroundColor: '#FF6B35',
            borderRadius: 12,
            paddingVertical: 16,
            alignItems: 'center',
          }}
        >
          <Text style={{ color: '#fff', fontWeight: '600', fontSize: 15 }}>
            {t('onboarding.loginButton')}
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={() => router.replace('/(tabs)')}
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingVertical: 16,
            alignItems: 'center',
          }}
        >
          <Text style={{ color: colors.text, fontWeight: '600', fontSize: 15 }}>
            {t('onboarding.guestButton')}
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}
