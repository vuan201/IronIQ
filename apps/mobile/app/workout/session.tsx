import { useLocalSearchParams, useRouter } from 'expo-router';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Alert, Pressable, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Colors } from '@/constants/colors';
import { RestTimer } from '@/components/workout/RestTimer';
import { SetRow } from '@/components/workout/SetRow';
import { useWorkoutPlans } from '@/features/workout-plans/hooks';
import { useCompleteSession, useLogSet, useStartSession } from '@/features/workout-sessions/hooks';
import { useSessionStore } from '@/features/workout-sessions/store';
import { useTheme } from '@/hooks/useTheme';

interface RestTimerState {
  restSeconds: number;
}

export default function SessionScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const { dayId } = useLocalSearchParams<{ dayId: string }>();

  const { data: plans } = useWorkoutPlans();
  const { mutateAsync: startSession } = useStartSession();
  const { mutateAsync: completeSession, isPending: isCompleting } = useCompleteSession();

  const session = useSessionStore((s) => s.session);
  const completedSets = useSessionStore((s) => s.completedSets);
  const setSession = useSessionStore((s) => s.setSession);
  const markSetCompleted = useSessionStore((s) => s.markSetCompleted);
  const isSetCompleted = useSessionStore((s) => s.isSetCompleted);
  const clearSession = useSessionStore((s) => s.clearSession);

  const { mutate: logSet } = useLogSet(session?.id ?? '');
  const startedRef = useRef(false);
  const [restTimer, setRestTimer] = useState<RestTimerState | null>(null);

  const day = plans?.flatMap((p) => p.days).find((d) => d.id === dayId);
  const plan = plans?.find((p) => p.days.some((d) => d.id === dayId));

  useEffect(() => {
    if (!day || startedRef.current) return;
    startedRef.current = true;
    startSession({ workoutPlanId: plan?.id ?? null, workoutDayId: dayId })
      .then(setSession)
      .catch(() => Alert.alert(t('common.error')));
  }, [day]);

  const handleSetComplete = (
    exerciseId: string,
    setNumber: number,
    restSeconds: number,
    weightKg: number | null,
    reps: number | null
  ) => {
    if (!session) return;
    logSet({ exerciseId, setNumber, weightKg, reps });
    markSetCompleted(exerciseId, { exerciseId, setNumber, weightKg, reps });
    setRestTimer({ restSeconds });
  };

  const handleFinish = async () => {
    if (!session) return;
    try {
      const currentSessionId = session.id;
      await completeSession({ sessionId: currentSessionId });
      clearSession();
      router.replace(`/workout/summary?sessionId=${currentSessionId}`);
    } catch {
      Alert.alert(t('common.error'));
    }
  };

  const handleAbandon = () => {
    Alert.alert(t('session.abandonTitle'), t('session.abandonMessage'), [
      { text: t('common.cancel'), style: 'cancel' },
      {
        text: t('session.abandon'),
        style: 'destructive',
        onPress: () => {
          clearSession();
          router.back();
        },
      },
    ]);
  };

  if (!day) {
    return (
      <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg, justifyContent: 'center', alignItems: 'center' }}>
        <Text style={{ color: colors.textSecondary }}>{t('common.loading')}</Text>
      </SafeAreaView>
    );
  }

  const totalSets = day.exercises.reduce((sum, ex) => sum + ex.sets, 0);
  const doneCount = Object.values(completedSets).reduce((sum, arr) => sum + arr.length, 0);

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingTop: 12, paddingBottom: 8 }}>
        <Pressable onPress={handleAbandon} style={{ padding: 4, marginRight: 12 }}>
          <Text style={{ color: Colors.semantic.danger, fontSize: 15 }}>{t('session.abandon')}</Text>
        </Pressable>
        <View style={{ flex: 1 }}>
          <Text style={{ color: colors.text, fontWeight: '700', fontSize: 17 }} numberOfLines={1}>
            {day.name ?? plan?.name ?? t('session.title')}
          </Text>
          <Text style={{ color: colors.textSecondary, fontSize: 12 }}>
            {t('session.progress', { done: doneCount, total: totalSets })}
          </Text>
        </View>
        <TouchableOpacity
          onPress={handleFinish}
          disabled={isCompleting || !session}
          activeOpacity={0.8}
          style={{
            backgroundColor: colors.primary,
            borderRadius: 10,
            paddingHorizontal: 14,
            paddingVertical: 8,
            opacity: isCompleting || !session ? 0.5 : 1,
          }}
        >
          <Text style={{ color: '#FFFFFF', fontWeight: '700', fontSize: 14 }}>{t('session.finish')}</Text>
        </TouchableOpacity>
      </View>

      <ScrollView
        style={{ flex: 1, paddingHorizontal: 16 }}
        showsVerticalScrollIndicator={false}
        keyboardShouldPersistTaps="handled"
      >
        {restTimer && (
          <RestTimer seconds={restTimer.restSeconds} onDismiss={() => setRestTimer(null)} />
        )}

        {day.exercises.map((exercise) => (
          <View
            key={exercise.id}
            style={{
              backgroundColor: colors.surface,
              borderRadius: 16,
              padding: 16,
              marginBottom: 12,
              borderWidth: 1,
              borderColor: colors.border,
            }}
          >
            <Text style={{ color: colors.text, fontWeight: '700', fontSize: 16, marginBottom: 10 }}>
              {exercise.exerciseName}
            </Text>

            <View style={{ flexDirection: 'row', marginBottom: 4 }}>
              <Text style={{ width: 28, color: colors.textDisabled, fontSize: 11, textAlign: 'center' }}>#</Text>
              <Text style={{ flex: 1, color: colors.textDisabled, fontSize: 11, textAlign: 'center' }}>kg</Text>
              <Text style={{ flex: 1, color: colors.textDisabled, fontSize: 11, textAlign: 'center' }}>reps</Text>
              <View style={{ width: 40 }} />
            </View>

            {Array.from({ length: exercise.sets }, (_, i) => i + 1).map((setNum) => (
              <SetRow
                key={setNum}
                setNumber={setNum}
                prevReps={exercise.reps}
                isCompleted={isSetCompleted(exercise.exerciseId, setNum)}
                onComplete={(weightKg, reps) =>
                  handleSetComplete(exercise.exerciseId, setNum, exercise.restSeconds, weightKg, reps)
                }
              />
            ))}
          </View>
        ))}

        <View style={{ height: 32 }} />
      </ScrollView>
    </SafeAreaView>
  );
}
