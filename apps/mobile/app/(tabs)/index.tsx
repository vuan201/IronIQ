import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { Pressable, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { WorkoutDayCard } from '@/components/workout/WorkoutDayCard';
import { Skeleton } from '@/components/ui/Skeleton';
import { useWorkoutPlans } from '@/features/workout-plans/hooks';
import { useMyProfile } from '@/features/users/hooks';
import type { WorkoutDay } from '@/features/workout-plans/types';
import { useTheme } from '@/hooks/useTheme';

const TODAY_DOW = new Date().getDay() === 0 ? 6 : new Date().getDay() - 1; // ISO 0=Mon

export default function HomeScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const { data: plans, isLoading } = useWorkoutPlans();
  const { data: profile } = useMyProfile();

  const todayDays = plans?.flatMap((p) =>
    p.days.filter((d) => d.dayOfWeek === TODAY_DOW)
  ) ?? [];

  const handleDayPress = (day: WorkoutDay) => {
    router.push(`/workout/session?dayId=${day.id}`);
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <ScrollView style={{ flex: 1, paddingHorizontal: 16 }} showsVerticalScrollIndicator={false}>
        <View style={{ paddingTop: 24, paddingBottom: 16, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }}>
          <View>
            <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text }}>
              {t('home.title')}
            </Text>
            <Text style={{ color: colors.textSecondary, marginTop: 4 }}>
              {t('home.subtitle')}
            </Text>
          </View>
          {(profile?.currentStreak ?? 0) > 0 && (
            <View style={{ alignItems: 'center', backgroundColor: colors.surface, borderRadius: 14, paddingHorizontal: 14, paddingVertical: 8, borderWidth: 1, borderColor: colors.border }}>
              <Text style={{ fontSize: 22 }}>🔥</Text>
              <Text style={{ fontSize: 18, fontWeight: '800', color: '#FF6B35' }}>{profile?.currentStreak}</Text>
              <Text style={{ fontSize: 11, color: colors.textSecondary }}>{t('home.streak')}</Text>
            </View>
          )}
        </View>

        <Text style={{ fontSize: 18, fontWeight: '600', color: colors.text, marginBottom: 12 }}>
          {t('home.todaySection')}
        </Text>

        {isLoading ? (
          <>
            <Skeleton height={100} />
            <View style={{ height: 12 }} />
            <Skeleton height={100} />
          </>
        ) : todayDays.length > 0 ? (
          todayDays.map((day) => (
            <WorkoutDayCard key={day.id} day={day} onPress={handleDayPress} />
          ))
        ) : (
          <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 24, alignItems: 'center', marginBottom: 16 }}>
            <Text style={{ color: colors.textSecondary, textAlign: 'center' }}>
              {t('home.noWorkoutToday')}
            </Text>
          </View>
        )}

        <Text style={{ fontSize: 18, fontWeight: '600', color: colors.text, marginBottom: 12, marginTop: 16 }}>
          {t('home.myPlans')}
        </Text>

        {isLoading ? (
          <Skeleton height={80} />
        ) : plans && plans.length > 0 ? (
          plans.map((plan) => (
            <TouchableOpacity
              key={plan.id}
              onPress={() => router.push(`/workout/plan?id=${plan.id}`)}
              activeOpacity={0.7}
              style={{
                backgroundColor: colors.surface,
                borderWidth: 1,
                borderColor: colors.border,
                borderRadius: 16,
                padding: 16,
                marginBottom: 12,
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'space-between',
              }}
            >
              <View style={{ flex: 1 }}>
                <Text style={{ color: colors.text, fontWeight: '600' }}>{plan.name}</Text>
                <Text style={{ color: colors.textSecondary, fontSize: 13, marginTop: 2 }}>
                  {t('home.planDays', { count: plan.days.length })}
                </Text>
              </View>
              <Text style={{ color: colors.primary, fontSize: 20 }}>›</Text>
            </TouchableOpacity>
          ))
        ) : (
          <View style={{ backgroundColor: colors.surface, borderRadius: 16, padding: 24, alignItems: 'center' }}>
            <Text style={{ color: colors.textSecondary, textAlign: 'center', marginBottom: 12 }}>
              {t('home.noPlans')}
            </Text>
            <Pressable
              onPress={() => router.push('/workout/create')}
              style={{ backgroundColor: colors.primary, borderRadius: 12, paddingHorizontal: 20, paddingVertical: 10 }}
            >
              <Text style={{ color: '#FFFFFF', fontWeight: '600' }}>{t('home.createPlan')}</Text>
            </Pressable>
          </View>
        )}

        <View style={{ flexDirection: 'row', gap: 10, marginTop: 16, marginBottom: 8 }}>
          <TouchableOpacity
            onPress={() => router.push('/workout/create')}
            activeOpacity={0.8}
            style={{ flex: 1, backgroundColor: colors.surface, borderRadius: 12, paddingVertical: 14, alignItems: 'center', borderWidth: 1, borderColor: colors.border }}
          >
            <Text style={{ color: colors.text, fontWeight: '700', fontSize: 15 }}>{t('home.newPlan')}</Text>
          </TouchableOpacity>
          <TouchableOpacity
            onPress={() => router.push('/ai-coach/generate')}
            activeOpacity={0.8}
            style={{ flex: 1, backgroundColor: colors.primary, borderRadius: 12, paddingVertical: 14, alignItems: 'center' }}
          >
            <Text style={{ color: '#FFFFFF', fontWeight: '700', fontSize: 15 }}>{t('aiCoach.generateButton')}</Text>
          </TouchableOpacity>
        </View>
        <TouchableOpacity
          onPress={() => router.push('/ai-coach')}
          activeOpacity={0.8}
          style={{ backgroundColor: colors.surface, borderRadius: 12, paddingVertical: 14, alignItems: 'center', marginBottom: 32, borderWidth: 1, borderColor: colors.border }}
        >
          <Text style={{ color: colors.primary, fontWeight: '700', fontSize: 15 }}>{t('aiCoach.chatButton')}</Text>
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
}
