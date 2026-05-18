import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { FlatList, Pressable, Text, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { AchievementBadge } from '@/components/ui/AchievementBadge';
import { Skeleton } from '@/components/ui/Skeleton';
import { useMyAchievements } from '@/features/achievements/hooks';
import { useTheme } from '@/hooks/useTheme';

export default function AchievementsScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const router = useRouter();
  const { data: achievements, isLoading } = useMyAchievements();

  const unlocked = achievements?.filter((a) => a.isUnlocked).length ?? 0;
  const total = achievements?.length ?? 0;

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ paddingHorizontal: 16, paddingTop: 16, paddingBottom: 8, flexDirection: 'row', alignItems: 'center', gap: 12 }}>
        <Pressable onPress={() => router.back()}>
          <Text style={{ color: colors.primary, fontSize: 17 }}>‹</Text>
        </Pressable>
        <Text style={{ fontSize: 22, fontWeight: '700', color: colors.text, flex: 1 }}>
          {t('achievements.title')}
        </Text>
        {total > 0 && (
          <Text style={{ color: colors.textSecondary, fontSize: 14 }}>
            {unlocked}/{total}
          </Text>
        )}
      </View>

      {isLoading ? (
        <View style={{ padding: 16, gap: 12 }}>
          {[1, 2, 3, 4, 5].map((i) => <Skeleton key={i} height={88} />)}
        </View>
      ) : (
        <FlatList
          data={achievements}
          keyExtractor={(a) => a.id}
          contentContainerStyle={{ padding: 16, gap: 10 }}
          renderItem={({ item }) => <AchievementBadge achievement={item} />}
        />
      )}
    </SafeAreaView>
  );
}
