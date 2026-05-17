import { useState } from 'react';
import { Text, TextInput, TouchableOpacity, View } from 'react-native';
import { useTheme } from '@/hooks/useTheme';
import { Colors } from '@/constants/colors';

interface SetRowProps {
  setNumber: number;
  prevWeightKg?: number | null;
  prevReps?: number | null;
  isCompleted?: boolean;
  onComplete: (weightKg: number | null, reps: number | null) => void;
}

export function SetRow({ setNumber, prevWeightKg, prevReps, isCompleted = false, onComplete }: SetRowProps) {
  const { colors } = useTheme();
  const [weight, setWeight] = useState(prevWeightKg != null ? String(prevWeightKg) : '');
  const [reps, setReps] = useState(prevReps != null ? String(prevReps) : '');

  const handleComplete = () => {
    if (isCompleted) return;
    onComplete(weight ? parseFloat(weight) : null, reps ? parseInt(reps, 10) : null);
  };

  return (
    <View style={{ flexDirection: 'row', alignItems: 'center', gap: 8, paddingVertical: 6 }}>
      <View style={{ width: 28, alignItems: 'center' }}>
        <Text style={{ color: colors.textSecondary, fontSize: 14, fontWeight: '600' }}>{setNumber}</Text>
      </View>

      <TextInput
        value={weight}
        onChangeText={setWeight}
        placeholder="kg"
        placeholderTextColor={colors.textDisabled}
        keyboardType="decimal-pad"
        editable={!isCompleted}
        style={{
          flex: 1,
          backgroundColor: colors.surface,
          borderRadius: 8,
          paddingHorizontal: 10,
          paddingVertical: 8,
          color: colors.text,
          textAlign: 'center',
          fontSize: 14,
          opacity: isCompleted ? 0.6 : 1,
        }}
      />

      <TextInput
        value={reps}
        onChangeText={setReps}
        placeholder="reps"
        placeholderTextColor={colors.textDisabled}
        keyboardType="number-pad"
        editable={!isCompleted}
        style={{
          flex: 1,
          backgroundColor: colors.surface,
          borderRadius: 8,
          paddingHorizontal: 10,
          paddingVertical: 8,
          color: colors.text,
          textAlign: 'center',
          fontSize: 14,
          opacity: isCompleted ? 0.6 : 1,
        }}
      />

      <TouchableOpacity
        onPress={handleComplete}
        disabled={isCompleted}
        activeOpacity={0.7}
        style={{
          width: 40,
          height: 40,
          borderRadius: 20,
          backgroundColor: isCompleted ? Colors.semantic.success : colors.surface,
          borderWidth: isCompleted ? 0 : 1,
          borderColor: colors.border,
          alignItems: 'center',
          justifyContent: 'center',
        }}
      >
        <Text style={{ fontSize: isCompleted ? 18 : 16, color: isCompleted ? '#FFFFFF' : colors.textSecondary }}>
          {isCompleted ? '✓' : '○'}
        </Text>
      </TouchableOpacity>
    </View>
  );
}
