import { useLocalSearchParams, useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { Modal, Pressable, ScrollView, Text, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Colors } from '@/constants/colors';
import { ProgressionSuggestionCard } from '@/components/workout/ProgressionSuggestionCard';
import { SessionReviewCard } from '@/components/workout/SessionReviewCard';
import { useProgressionSuggestions, useSessionReview } from '@/features/ai-coach/hooks';
import { useNewlyUnlockedAchievements } from '@/features/achievements/hooks';
import { useSessionHistory } from '@/features/workout-sessions/hooks';
import { useTheme } from '@/hooks/useTheme';

function formatDuration(startedAt: string, completedAt: string | null): string {
  if (!completedAt) return '—';
  const diffMs = new Date(completedAt).getTime() - new Date(startedAt).getTime();
  const mins = Math.floor(diffMs / 60000);
  const secs = Math.floor((diffMs % 60000) / 1000);
  return mins > 0 ? `${mins} min ${secs}s` : `${secs}s`;
}

export default function SummaryScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const { sessionId } = useLocalSearchParams<{ sessionId?: string }>();
  const { data: history } = useSessionHistory(1);
  const { data: suggestions } = useProgressionSuggestions(sessionId);
  const { data: reviewData, isLoading: isReviewLoading } = useSessionReview(sessionId);
  const newAchievements = useNewlyUnlockedAchievements();

  const latest = history?.items[0];

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <Modal visible={newAchievements.length > 0} transparent animationType="fade">
        <View style={{ flex: 1, backgroundColor: 'rgba(0,0,0,0.6)', justifyContent: 'center', alignItems: 'center', padding: 32 }}>
          <View style={{ backgroundColor: colors.surface, borderRadius: 24, padding: 28, width: '100%', alignItems: 'center', gap: 12 }}>
            <Text style={{ fontSize: 40 }}>🎉</Text>
            <Text style={{ color: colors.text, fontSize: 20, fontWeight: '800' }}>
              {t('achievements.unlocked')}
            </Text>
            {newAchievements.map((a) => (
              <View key={a.id} style={{ flexDirection: 'row', alignItems: 'center', gap: 10 }}>
                <Text style={{ fontSize: 28 }}>{a.iconEmoji}</Text>
                <View>
                  <Text style={{ color: colors.text, fontWeight: '700' }}>{a.name}</Text>
                  <Text style={{ color: colors.textSecondary, fontSize: 12 }}>{a.description}</Text>
                </View>
              </View>
            ))}
            <Pressable
              onPress={() => router.replace('/(tabs)')}
              style={{ backgroundColor: '#FF6B35', borderRadius: 12, paddingHorizontal: 28, paddingVertical: 12, marginTop: 4 }}
            >
              <Text style={{ color: '#FFF', fontWeight: '700' }}>{t('common.ok')}</Text>
            </Pressable>
          </View>
        </View>
      </Modal>
      <ScrollView contentContainerStyle={{ flex: 1, paddingHorizontal: 16, paddingTop: 32 }}>
        <View style={{ alignItems: 'center', marginBottom: 32 }}>
          <View
            style={{
              width: 72,
              height: 72,
              borderRadius: 36,
              backgroundColor: Colors.semantic.success,
              alignItems: 'center',
              justifyContent: 'center',
              marginBottom: 16,
            }}
          >
            <Text style={{ fontSize: 36 }}>✓</Text>
          </View>
          <Text style={{ color: colors.text, fontSize: 24, fontWeight: '700', marginBottom: 4 }}>
            {t('session.summary.title')}
          </Text>
          <Text style={{ color: colors.textSecondary, fontSize: 14 }}>
            {t('session.summary.subtitle')}
          </Text>
        </View>

        {latest && (
          <View
            style={{
              backgroundColor: colors.surface,
              borderRadius: 20,
              padding: 20,
              marginBottom: 24,
              borderWidth: 1,
              borderColor: colors.border,
            }}
          >
            <View style={{ flexDirection: 'row', justifyContent: 'space-around' }}>
              <StatBlock
                label={t('session.summary.duration')}
                value={formatDuration(latest.startedAt, latest.completedAt)}
                colors={colors}
              />
              <StatBlock
                label={t('session.summary.exercises')}
                value={String(latest.totalExercises)}
                colors={colors}
              />
              <StatBlock
                label={t('session.summary.sets')}
                value={String(latest.totalSets)}
                colors={colors}
              />
            </View>
          </View>
        )}

        <SessionReviewCard review={reviewData?.review} isLoading={isReviewLoading && !!sessionId} />

        <ProgressionSuggestionCard suggestions={suggestions ?? []} />

        <Pressable
          onPress={() => router.replace('/(tabs)')}
          style={{
            backgroundColor: colors.primary,
            borderRadius: 14,
            paddingVertical: 16,
            alignItems: 'center',
          }}
        >
          <Text style={{ color: '#FFFFFF', fontWeight: '700', fontSize: 16 }}>
            {t('session.summary.done')}
          </Text>
        </Pressable>
      </ScrollView>
    </SafeAreaView>
  );
}

function StatBlock({
  label,
  value,
  colors,
}: {
  label: string;
  value: string;
  colors: ReturnType<typeof import('@/hooks/useTheme').useTheme>['colors'];
}) {
  return (
    <View style={{ alignItems: 'center', gap: 4 }}>
      <Text style={{ color: colors.text, fontSize: 28, fontWeight: '700' }}>{value}</Text>
      <Text style={{ color: colors.textSecondary, fontSize: 13 }}>{label}</Text>
    </View>
  );
}
