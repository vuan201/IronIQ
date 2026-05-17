import { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ActivityIndicator,
  KeyboardAvoidingView,
  Platform,
  Alert,
} from 'react-native';
import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { authApi } from '@/features/auth/api';
import { useAuthStore } from '@/features/auth/store';
import { useTheme } from '@/hooks/useTheme';

export default function RegisterScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const setAuth = useAuthStore((s) => s.setAuth);

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleRegister = async () => {
    if (!email || !password) return;
    if (password.length < 8) {
      Alert.alert(t('common.error'), t('auth.register.passwordTooShort'));
      return;
    }
    setLoading(true);
    try {
      const { data } = await authApi.register({ email, password });
      setAuth(data.accessToken, data.refreshToken, {
        userId: data.userId,
        email: data.email,
      });
      router.replace('/(tabs)');
    } catch {
      Alert.alert(t('common.error'), t('auth.register.emailTaken'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={{ flex: 1, backgroundColor: colors.bg }}
    >
      <View style={{ flex: 1, justifyContent: 'center', paddingHorizontal: 24 }}>
        <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text, marginBottom: 8 }}>
          {t('auth.register.title')}
        </Text>
        <Text style={{ fontSize: 15, color: colors.textSecondary, marginBottom: 32 }}>
          IronIQ
        </Text>

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 6 }}>
          {t('auth.login.emailLabel')}
        </Text>
        <TextInput
          value={email}
          onChangeText={setEmail}
          placeholder={t('auth.login.emailPlaceholder')}
          placeholderTextColor={colors.textSecondary}
          keyboardType="email-address"
          autoCapitalize="none"
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingHorizontal: 16,
            paddingVertical: 14,
            color: colors.text,
            fontSize: 15,
            marginBottom: 16,
          }}
        />

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 6 }}>
          {t('auth.login.passwordLabel')}
        </Text>
        <TextInput
          value={password}
          onChangeText={setPassword}
          placeholder={t('auth.login.passwordPlaceholder')}
          placeholderTextColor={colors.textSecondary}
          secureTextEntry
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingHorizontal: 16,
            paddingVertical: 14,
            color: colors.text,
            fontSize: 15,
            marginBottom: 24,
          }}
        />

        <TouchableOpacity
          onPress={handleRegister}
          disabled={loading}
          style={{
            backgroundColor: '#FF6B35',
            borderRadius: 12,
            paddingVertical: 16,
            alignItems: 'center',
            marginBottom: 16,
          }}
        >
          {loading ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={{ color: '#fff', fontWeight: '600', fontSize: 15 }}>
              {t('auth.register.submitButton')}
            </Text>
          )}
        </TouchableOpacity>

        <View style={{ flexDirection: 'row', justifyContent: 'center', gap: 4 }}>
          <Text style={{ color: colors.textSecondary, fontSize: 14 }}>
            {t('auth.register.hasAccount')}
          </Text>
          <TouchableOpacity onPress={() => router.back()}>
            <Text style={{ color: '#FF6B35', fontSize: 14, fontWeight: '600' }}>
              {t('auth.register.loginLink')}
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}
