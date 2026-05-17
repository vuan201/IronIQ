import { TouchableOpacity, View, Text } from 'react-native';
import { useTheme } from '@/hooks/useTheme';
import type { Exercise } from '@/features/exercises/types';
import { Colors } from '@/constants/colors';

const MUSCLE_COLORS: Record<string, string> = {
  Chest: Colors.muscle.chest,
  Back: Colors.muscle.back,
  Shoulders: Colors.muscle.shoulders,
  Biceps: Colors.muscle.biceps,
  Triceps: Colors.muscle.triceps,
  Abs: Colors.muscle.abs,
  Legs: Colors.muscle.legs,
  Glutes: Colors.muscle.legs,
  Calves: Colors.muscle.legs,
  FullBody: Colors.muscle.fullBody,
  Cardio: Colors.muscle.cardio,
};

const DIFFICULTY_COLORS: Record<string, string> = {
  Beginner: Colors.semantic.success,
  Intermediate: Colors.semantic.warning,
  Advanced: Colors.semantic.danger,
};

interface ExerciseCardProps {
  exercise: Exercise;
  onPress: (id: string) => void;
}

export function ExerciseCard({ exercise, onPress }: ExerciseCardProps) {
  const { colors } = useTheme();
  const primaryMuscle = exercise.muscleGroups[0] ?? 'FullBody';
  const muscleColor = MUSCLE_COLORS[primaryMuscle] ?? colors.textSecondary;
  const difficultyColor = DIFFICULTY_COLORS[exercise.difficulty] ?? colors.textSecondary;

  return (
    <TouchableOpacity
      onPress={() => onPress(exercise.id)}
      activeOpacity={0.75}
      style={{
        backgroundColor: colors.surface,
        borderRadius: 16,
        padding: 16,
        marginBottom: 12,
        flexDirection: 'row',
        alignItems: 'center',
        gap: 12,
      }}
    >
      <View
        style={{
          width: 8,
          height: 40,
          borderRadius: 4,
          backgroundColor: muscleColor,
        }}
      />

      <View style={{ flex: 1 }}>
        <Text
          style={{ fontSize: 15, fontWeight: '600', color: colors.text, marginBottom: 4 }}
          numberOfLines={1}
        >
          {exercise.name}
        </Text>
        <View style={{ flexDirection: 'row', gap: 8, flexWrap: 'wrap' }}>
          <Text style={{ fontSize: 11, color: muscleColor, fontWeight: '500' }}>
            {exercise.muscleGroups.join(' · ')}
          </Text>
        </View>
      </View>

      <View
        style={{
          paddingHorizontal: 8,
          paddingVertical: 4,
          borderRadius: 8,
          backgroundColor: difficultyColor + '22',
        }}
      >
        <Text style={{ fontSize: 11, color: difficultyColor, fontWeight: '600' }}>
          {exercise.difficulty}
        </Text>
      </View>
    </TouchableOpacity>
  );
}
