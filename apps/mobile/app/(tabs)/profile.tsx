import { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  ActivityIndicator,
  Alert,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTranslation } from 'react-i18next';
import { useRouter } from 'expo-router';
import { useMyProfile, useUpdateProfile } from '@/features/users/hooks';
import { useAuthStore } from '@/features/auth/store';
import { useTheme } from '@/hooks/useTheme';
import { FITNESS_GOALS, FITNESS_LEVELS } from '@/features/users/types';
import { useCoinBalance } from '@/features/coins/hooks';

export default function ProfileScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const router = useRouter();
  const clearAuth = useAuthStore((s) => s.clearAuth);
  const { data: profile, isLoading } = useMyProfile();
  const updateProfile = useUpdateProfile();
  const { data: coinData } = useCoinBalance();

  const [name, setName] = useState('');
  const [editing, setEditing] = useState(false);

  const startEdit = () => {
    setName(profile?.name ?? '');
    setEditing(true);
  };

  const saveEdit = async () => {
    try {
      await updateProfile.mutateAsync({ name: name || null });
      setEditing(false);
    } catch {
      Alert.alert(t('common.error'));
    }
  };

  if (isLoading) {
    return (
      <View style={{ flex: 1, backgroundColor: colors.bg, justifyContent: 'center', alignItems: 'center' }}>
        <ActivityIndicator color="#FF6B35" />
      </View>
    );
  }

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <ScrollView contentContainerStyle={{ padding: 16 }}>
        <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text, marginBottom: 24 }}>
          {t('profile.title')}
        </Text>

        <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 16, marginBottom: 16 }}>
          <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 4 }}>Email</Text>
          <Text style={{ fontSize: 15, color: colors.text }}>{profile?.email}</Text>
        </View>

        <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 16, marginBottom: 16 }}>
          <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 }}>
            <Text style={{ fontSize: 17, fontWeight: '600', color: colors.text }}>
              {t('profile.personalInfo')}
            </Text>
            {!editing && (
              <TouchableOpacity onPress={startEdit}>
                <Text style={{ color: '#FF6B35', fontSize: 14 }}>{t('common.edit')}</Text>
              </TouchableOpacity>
            )}
          </View>

          <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 4 }}>
            {t('profile.name')}
          </Text>
          {editing ? (
            <TextInput
              value={name}
              onChangeText={setName}
              placeholder={t('profile.namePlaceholder')}
              placeholderTextColor={colors.textSecondary}
              style={{
                backgroundColor: colors.surface2,
                borderRadius: 8,
                padding: 10,
                color: colors.text,
                marginBottom: 12,
              }}
            />
          ) : (
            <Text style={{ fontSize: 15, color: colors.text, marginBottom: 12 }}>
              {profile?.name ?? '—'}
            </Text>
          )}

          {profile && (
            <View style={{ flexDirection: 'row', gap: 16 }}>
              <View style={{ flex: 1 }}>
                <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 2 }}>
                  {t('profile.goal')}
                </Text>
                <Text style={{ fontSize: 14, color: colors.text }}>{profile.goal ?? '—'}</Text>
              </View>
              <View style={{ flex: 1 }}>
                <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 2 }}>
                  {t('profile.level')}
                </Text>
                <Text style={{ fontSize: 14, color: colors.text }}>{profile.level ?? '—'}</Text>
              </View>
            </View>
          )}

          {editing && (
            <View style={{ flexDirection: 'row', gap: 8, marginTop: 8 }}>
              <TouchableOpacity
                onPress={() => setEditing(false)}
                style={{ flex: 1, backgroundColor: colors.surface2, borderRadius: 8, padding: 12, alignItems: 'center' }}
              >
                <Text style={{ color: colors.textSecondary }}>{t('common.cancel')}</Text>
              </TouchableOpacity>
              <TouchableOpacity
                onPress={saveEdit}
                disabled={updateProfile.isPending}
                style={{ flex: 1, backgroundColor: '#FF6B35', borderRadius: 8, padding: 12, alignItems: 'center' }}
              >
                {updateProfile.isPending ? (
                  <ActivityIndicator color="#fff" size="small" />
                ) : (
                  <Text style={{ color: '#fff', fontWeight: '600' }}>{t('common.save')}</Text>
                )}
              </TouchableOpacity>
            </View>
          )}
        </View>

        <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 16, marginBottom: 16 }}>
          <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 4 }}>
            {t('profile.subscription')}
          </Text>
          <Text style={{ fontSize: 15, color: colors.text }}>{profile?.subscriptionTier}</Text>
        </View>

        <TouchableOpacity
          onPress={() => router.push('/coins')}
          style={{
            backgroundColor: colors.surface,
            borderRadius: 16,
            padding: 16,
            marginBottom: 16,
            flexDirection: 'row',
            justifyContent: 'space-between',
            alignItems: 'center',
          }}
        >
          <View>
            <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 2 }}>
              {t('coins.title')}
            </Text>
            <Text style={{ fontSize: 22, fontWeight: '700', color: colors.primary }}>
              🪙 {coinData?.balance ?? 0}
            </Text>
          </View>
          <Text style={{ fontSize: 14, color: colors.primary, fontWeight: '600' }}>
            {t('coins.watchAd')} →
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={clearAuth}
          style={{ backgroundColor: colors.surface, borderRadius: 12, padding: 16, alignItems: 'center' }}
        >
          <Text style={{ color: '#EF4444', fontSize: 15, fontWeight: '600' }}>
            {t('profile.logout')}
          </Text>
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
}
