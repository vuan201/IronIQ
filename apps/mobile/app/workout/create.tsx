import { useRouter } from 'expo-router';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Alert, Pressable, ScrollView, Text, TextInput, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useCreateWorkoutPlan } from '@/features/workout-plans/hooks';
import type { CreateWorkoutDayDto } from '@/features/workout-plans/types';
import { useTheme } from '@/hooks/useTheme';

const DAY_LABELS = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

export default function WorkoutCreateScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const { mutateAsync: createPlan, isPending } = useCreateWorkoutPlan();

  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [selectedDays, setSelectedDays] = useState<number[]>([]);

  const toggleDay = (dow: number) => {
    setSelectedDays((prev) =>
      prev.includes(dow) ? prev.filter((d) => d !== dow) : [...prev, dow].sort()
    );
  };

  const handleCreate = async () => {
    if (!name.trim()) {
      Alert.alert(t('workoutPlans.create.validationError'));
      return;
    }
    const days: CreateWorkoutDayDto[] = selectedDays.map((dow) => ({
      dayOfWeek: dow,
      name: null,
      exercises: [],
    }));
    try {
      await createPlan({ name: name.trim(), description: description.trim() || null, days });
      router.back();
    } catch {
      Alert.alert(t('common.error'));
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingTop: 16, paddingBottom: 8 }}>
        <Pressable onPress={() => router.back()} style={{ marginRight: 12, padding: 4 }}>
          <Text style={{ color: colors.primary }}>{t('common.cancel')}</Text>
        </Pressable>
        <Text style={{ fontSize: 20, fontWeight: '700', color: colors.text, flex: 1 }}>
          {t('workoutPlans.create.title')}
        </Text>
        <Pressable
          onPress={handleCreate}
          disabled={isPending}
          style={{ backgroundColor: colors.primary, borderRadius: 12, paddingHorizontal: 16, paddingVertical: 8, opacity: isPending ? 0.6 : 1 }}
        >
          <Text style={{ color: '#FFFFFF', fontWeight: '600' }}>{t('common.save')}</Text>
        </Pressable>
      </View>

      <ScrollView style={{ flex: 1, paddingHorizontal: 16, paddingTop: 16 }} showsVerticalScrollIndicator={false}>
        <Text style={{ fontSize: 13, fontWeight: '500', color: colors.textSecondary, marginBottom: 6 }}>
          {t('workoutPlans.create.name')}
        </Text>
        <TextInput
          value={name}
          onChangeText={setName}
          placeholder={t('workoutPlans.create.namePlaceholder')}
          placeholderTextColor={colors.textSecondary}
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingHorizontal: 16,
            paddingVertical: 12,
            color: colors.text,
            marginBottom: 16,
          }}
        />

        <Text style={{ fontSize: 13, fontWeight: '500', color: colors.textSecondary, marginBottom: 6 }}>
          {t('workoutPlans.create.description')}
        </Text>
        <TextInput
          value={description}
          onChangeText={setDescription}
          placeholder={t('workoutPlans.create.descriptionPlaceholder')}
          placeholderTextColor={colors.textSecondary}
          multiline
          numberOfLines={3}
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingHorizontal: 16,
            paddingVertical: 12,
            color: colors.text,
            marginBottom: 16,
            minHeight: 80,
            textAlignVertical: 'top',
          }}
        />

        <Text style={{ fontSize: 13, fontWeight: '500', color: colors.textSecondary, marginBottom: 12 }}>
          {t('workoutPlans.create.selectDays')}
        </Text>
        <View style={{ flexDirection: 'row', flexWrap: 'wrap', gap: 8, marginBottom: 32 }}>
          {DAY_LABELS.map((label, dow) => {
            const selected = selectedDays.includes(dow);
            return (
              <TouchableOpacity
                key={dow}
                onPress={() => toggleDay(dow)}
                style={{
                  borderRadius: 12,
                  paddingHorizontal: 16,
                  paddingVertical: 10,
                  borderWidth: 1,
                  backgroundColor: selected ? colors.primary : colors.surface,
                  borderColor: selected ? colors.primary : colors.border,
                }}
              >
                <Text style={{ fontWeight: '600', color: selected ? '#FFFFFF' : colors.text }}>
                  {label}
                </Text>
              </TouchableOpacity>
            );
          })}
        </View>

        <Text style={{ color: colors.textSecondary, fontSize: 13, textAlign: 'center', marginBottom: 16 }}>
          {t('workoutPlans.create.addExercisesLater')}
        </Text>
      </ScrollView>
    </SafeAreaView>
  );
}
