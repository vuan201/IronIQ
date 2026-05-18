import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, FlatList, Pressable, Text, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useLeaderboard } from '@/features/social/hooks';
import type { LeaderboardEntry } from '@/features/social/types';
import { useTheme } from '@/hooks/useTheme';

function LeaderboardRow({ entry, colors }: { entry: LeaderboardEntry; colors: ReturnType<typeof import('@/hooks/useTheme').useTheme>['colors'] }) {
  const isTop3 = entry.rank <= 3;
  const medals = ['🥇', '🥈', '🥉'];

  return (
    <View style={{
      backgroundColor: colors.surface,
      borderRadius: 14,
      padding: 14,
      flexDirection: 'row',
      alignItems: 'center',
      gap: 12,
      borderWidth: isTop3 ? 1 : 0,
      borderColor: isTop3 ? colors.primary : 'transparent',
    }}>
      <Text style={{ fontSize: isTop3 ? 24 : 16, width: 32, textAlign: 'center', color: colors.textSecondary, fontWeight: '700' }}>
        {isTop3 ? medals[entry.rank - 1] : `#${entry.rank}`}
      </Text>
      <View style={{ flex: 1 }}>
        <Text style={{ fontSize: 15, fontWeight: '600', color: colors.text }}>{entry.displayName}</Text>
        <Text style={{ fontSize: 12, color: colors.textSecondary }}>
          🔥 {entry.currentStreak} · best {entry.longestStreak}
        </Text>
      </View>
      <Text style={{ fontSize: 22, fontWeight: '800', color: colors.primary }}>{entry.currentStreak}</Text>
    </View>
  );
}

export default function LeaderboardScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const router = useRouter();
  const { data: entries, isLoading } = useLeaderboard();

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ paddingHorizontal: 16, paddingTop: 16, paddingBottom: 8, flexDirection: 'row', alignItems: 'center', gap: 12 }}>
        <Pressable onPress={() => router.back()}>
          <Text style={{ color: colors.primary, fontSize: 17 }}>‹</Text>
        </Pressable>
        <Text style={{ fontSize: 22, fontWeight: '700', color: colors.text }}>
          {t('leaderboard.title')}
        </Text>
      </View>

      <Text style={{ fontSize: 12, color: colors.textSecondary, paddingHorizontal: 16, paddingBottom: 8 }}>
        {t('leaderboard.subtitle')}
      </Text>

      {isLoading ? (
        <ActivityIndicator color={colors.primary} style={{ marginTop: 48 }} />
      ) : !entries || entries.length === 0 ? (
        <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
          <Text style={{ color: colors.textSecondary, fontSize: 15 }}>{t('leaderboard.empty')}</Text>
        </View>
      ) : (
        <FlatList
          data={entries}
          keyExtractor={(e) => String(e.rank)}
          contentContainerStyle={{ padding: 16, gap: 10 }}
          renderItem={({ item }) => <LeaderboardRow entry={item} colors={colors} />}
        />
      )}
    </SafeAreaView>
  );
}
