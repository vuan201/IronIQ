import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { BarChart } from '@/components/charts/BarChart';
import { LineChart } from '@/components/charts/LineChart';
import { Skeleton } from '@/components/ui/Skeleton';
import { useProgress } from '@/features/progress/hooks';
import { useTheme } from '@/hooks/useTheme';

const PERIOD_OPTIONS = [
  { label: '4W', weeksBack: 4 },
  { label: '8W', weeksBack: 8 },
  { label: '12W', weeksBack: 12 },
];

export default function ProgressScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const [weeksBack, setWeeksBack] = useState(8);
  const { data, isLoading } = useProgress(weeksBack);

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <ScrollView style={{ flex: 1, paddingHorizontal: 16 }} showsVerticalScrollIndicator={false}>
        <View style={{ paddingTop: 24, paddingBottom: 8, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }}>
          <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text }}>{t('progress.title')}</Text>
          <View style={{ flexDirection: 'row', gap: 4 }}>
            {PERIOD_OPTIONS.map((opt) => (
              <TouchableOpacity
                key={opt.label}
                onPress={() => setWeeksBack(opt.weeksBack)}
                style={{
                  paddingHorizontal: 10,
                  paddingVertical: 5,
                  borderRadius: 8,
                  backgroundColor: weeksBack === opt.weeksBack ? colors.primary : colors.surface,
                }}
              >
                <Text style={{ color: weeksBack === opt.weeksBack ? '#FFFFFF' : colors.textSecondary, fontSize: 13, fontWeight: '600' }}>
                  {opt.label}
                </Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>

        {/* Summary stats */}
        {isLoading ? (
          <Skeleton height={80} style={{ marginBottom: 16 }} />
        ) : data && (
          <View style={{ flexDirection: 'row', gap: 12, marginBottom: 20 }}>
            <StatCard label={t('progress.totalSessions')} value={String(data.totalSessionsAllTime)} colors={colors} />
            <StatCard label={t('progress.totalSets')} value={String(data.totalSetsAllTime)} colors={colors} />
          </View>
        )}

        {/* Weekly frequency chart */}
        <Text style={{ fontSize: 16, fontWeight: '600', color: colors.text, marginBottom: 12 }}>
          {t('progress.weeklyFrequency')}
        </Text>
        {isLoading ? (
          <Skeleton height={160} style={{ marginBottom: 20 }} />
        ) : data && (
          <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 16, marginBottom: 20, borderWidth: 1, borderColor: colors.border }}>
            <BarChart
              data={data.weeklySessionCounts.map((w) => ({ label: w.weekLabel, value: w.count }))}
              height={140}
            />
          </View>
        )}

        {/* Volume per session chart */}
        <Text style={{ fontSize: 16, fontWeight: '600', color: colors.text, marginBottom: 12 }}>
          {t('progress.recentVolume')}
        </Text>
        {isLoading ? (
          <Skeleton height={160} style={{ marginBottom: 20 }} />
        ) : data && data.recentVolume.length > 0 ? (
          <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 16, marginBottom: 20, borderWidth: 1, borderColor: colors.border }}>
            <LineChart
              data={data.recentVolume.map((v) => ({ label: v.dateLabel, value: v.totalVolumeKg }))}
              height={140}
              lineColor={colors.primaryDim}
            />
          </View>
        ) : !isLoading && (
          <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 24, alignItems: 'center', marginBottom: 20, borderWidth: 1, borderColor: colors.border }}>
            <Text style={{ color: colors.textSecondary }}>{t('progress.noData')}</Text>
          </View>
        )}

        <View style={{ height: 32 }} />
      </ScrollView>
    </SafeAreaView>
  );
}

function StatCard({
  label,
  value,
  colors,
}: {
  label: string;
  value: string;
  colors: ReturnType<typeof import('@/hooks/useTheme').useTheme>['colors'];
}) {
  return (
    <View
      style={{
        flex: 1,
        backgroundColor: colors.surface,
        borderRadius: 16,
        padding: 16,
        alignItems: 'center',
        borderWidth: 1,
        borderColor: colors.border,
      }}
    >
      <Text style={{ color: colors.text, fontSize: 28, fontWeight: '700' }}>{value}</Text>
      <Text style={{ color: colors.textSecondary, fontSize: 12, marginTop: 2, textAlign: 'center' }}>{label}</Text>
    </View>
  );
}
