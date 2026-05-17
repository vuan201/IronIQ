import { useTranslation } from 'react-i18next';
import { Text, TouchableOpacity, View } from 'react-native';
import { useTheme } from '@/hooks/useTheme';
import type { WorkoutDay } from '@/features/workout-plans/types';

const DAY_KEYS = [
  'workoutPlans.days.mon',
  'workoutPlans.days.tue',
  'workoutPlans.days.wed',
  'workoutPlans.days.thu',
  'workoutPlans.days.fri',
  'workoutPlans.days.sat',
  'workoutPlans.days.sun',
] as const;

interface WorkoutDayCardProps {
  day: WorkoutDay;
  onPress?: (day: WorkoutDay) => void;
}

export function WorkoutDayCard({ day, onPress }: WorkoutDayCardProps) {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const dayLabel = t(DAY_KEYS[day.dayOfWeek]);

  return (
    <TouchableOpacity
      onPress={() => onPress?.(day)}
      activeOpacity={onPress ? 0.7 : 1}
      style={{
        backgroundColor: colors.surface,
        borderRadius: 16,
        padding: 16,
        marginBottom: 12,
        borderWidth: 1,
        borderColor: colors.border,
      }}
    >
      <View style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginBottom: 12 }}>
        <View style={{ flexDirection: 'row', alignItems: 'center', gap: 8 }}>
          <View style={{ backgroundColor: colors.primary, borderRadius: 8, paddingHorizontal: 8, paddingVertical: 4 }}>
            <Text style={{ color: '#FFFFFF', fontSize: 12, fontWeight: '700' }}>{dayLabel}</Text>
          </View>
          {day.name ? (
            <Text style={{ color: colors.text, fontWeight: '600' }}>{day.name}</Text>
          ) : null}
        </View>
        <Text style={{ color: colors.textSecondary, fontSize: 13 }}>
          {t('workoutPlans.dayCard.exerciseCount', { count: day.exercises.length })}
        </Text>
      </View>

      {day.exercises.slice(0, 3).map((ex) => (
        <View key={ex.id} style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingVertical: 4 }}>
          <Text style={{ color: colors.text, fontSize: 14, flex: 1 }} numberOfLines={1}>
            {ex.exerciseName}
          </Text>
          <Text style={{ color: colors.textSecondary, fontSize: 12, marginLeft: 8 }}>
            {ex.sets}×{ex.reps ?? `${ex.durationSeconds}s`}
          </Text>
        </View>
      ))}

      {day.exercises.length > 3 && (
        <Text style={{ color: colors.primary, fontSize: 12, marginTop: 4 }}>
          +{day.exercises.length - 3} {t('workoutPlans.dayCard.more')}
        </Text>
      )}
    </TouchableOpacity>
  );
}
