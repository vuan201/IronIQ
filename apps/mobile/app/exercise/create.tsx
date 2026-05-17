import { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  ActivityIndicator,
  Alert,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { useCreateExercise } from '@/features/exercises/hooks';
import { useTheme } from '@/hooks/useTheme';

const MUSCLES = ['Chest', 'Back', 'Shoulders', 'Biceps', 'Triceps', 'Abs', 'Legs', 'Glutes', 'Calves', 'FullBody', 'Cardio'];
const EQUIPMENTS = ['None', 'Barbell', 'Dumbbell', 'Kettlebell', 'Cable', 'Machine', 'Bodyweight', 'Bands', 'PullUpBar', 'Bench'];
const DIFFICULTIES = ['Beginner', 'Intermediate', 'Advanced'];

function MultiSelectChips({
  options,
  selected,
  onToggle,
  colors,
}: {
  options: string[];
  selected: string[];
  onToggle: (item: string) => void;
  colors: ReturnType<typeof useTheme>['colors'];
}) {
  return (
    <View style={{ flexDirection: 'row', flexWrap: 'wrap', gap: 8 }}>
      {options.map((item) => {
        const active = selected.includes(item);
        return (
          <TouchableOpacity
            key={item}
            onPress={() => onToggle(item)}
            style={{
              paddingHorizontal: 12,
              paddingVertical: 6,
              borderRadius: 20,
              backgroundColor: active ? '#FF6B35' : colors.surface2,
            }}
          >
            <Text style={{ color: active ? '#fff' : colors.textSecondary, fontSize: 13 }}>{item}</Text>
          </TouchableOpacity>
        );
      })}
    </View>
  );
}

export default function CreateExerciseScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const createExercise = useCreateExercise();

  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [muscles, setMuscles] = useState<string[]>([]);
  const [equipment, setEquipment] = useState<string[]>(['None']);
  const [difficulty, setDifficulty] = useState('Beginner');

  const toggle = (list: string[], setList: (v: string[]) => void, item: string) => {
    setList(list.includes(item) ? list.filter((i) => i !== item) : [...list, item]);
  };

  const handleSubmit = async () => {
    if (!name.trim() || muscles.length === 0) {
      Alert.alert(t('common.error'), t('exercises.create.validationError'));
      return;
    }
    try {
      await createExercise.mutateAsync({ name, description: description || null, muscleGroups: muscles, equipment, difficulty });
      router.back();
    } catch {
      Alert.alert(t('common.error'));
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <ScrollView contentContainerStyle={{ padding: 16 }}>
        <TouchableOpacity onPress={() => router.back()} style={{ marginBottom: 16 }}>
          <Text style={{ color: '#FF6B35', fontSize: 15 }}>← {t('common.cancel')}</Text>
        </TouchableOpacity>

        <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text, marginBottom: 24 }}>
          {t('exercises.create.title')}
        </Text>

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 6 }}>{t('exercises.create.name')}</Text>
        <TextInput
          value={name}
          onChangeText={setName}
          placeholder={t('exercises.create.namePlaceholder')}
          placeholderTextColor={colors.textSecondary}
          style={{ backgroundColor: colors.surface, borderRadius: 12, padding: 14, color: colors.text, marginBottom: 16 }}
        />

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 6 }}>{t('exercises.create.description')}</Text>
        <TextInput
          value={description}
          onChangeText={setDescription}
          placeholder={t('exercises.create.descriptionPlaceholder')}
          placeholderTextColor={colors.textSecondary}
          multiline
          numberOfLines={3}
          style={{ backgroundColor: colors.surface, borderRadius: 12, padding: 14, color: colors.text, marginBottom: 16, textAlignVertical: 'top' }}
        />

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 8 }}>{t('exercises.card.muscleGroup')}</Text>
        <MultiSelectChips options={MUSCLES} selected={muscles} onToggle={(m) => toggle(muscles, setMuscles, m)} colors={colors} />
        <View style={{ marginBottom: 16 }} />

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 8 }}>{t('exercises.card.equipment')}</Text>
        <MultiSelectChips options={EQUIPMENTS} selected={equipment} onToggle={(e) => toggle(equipment, setEquipment, e)} colors={colors} />
        <View style={{ marginBottom: 16 }} />

        <Text style={{ fontSize: 13, color: colors.textSecondary, marginBottom: 8 }}>{t('exercises.card.difficulty')}</Text>
        <View style={{ flexDirection: 'row', gap: 8, marginBottom: 32 }}>
          {DIFFICULTIES.map((d) => (
            <TouchableOpacity
              key={d}
              onPress={() => setDifficulty(d)}
              style={{
                flex: 1,
                paddingVertical: 10,
                borderRadius: 10,
                backgroundColor: difficulty === d ? '#FF6B35' : colors.surface,
                alignItems: 'center',
              }}
            >
              <Text style={{ color: difficulty === d ? '#fff' : colors.textSecondary, fontWeight: '600' }}>{d}</Text>
            </TouchableOpacity>
          ))}
        </View>

        <TouchableOpacity
          onPress={handleSubmit}
          disabled={createExercise.isPending}
          style={{ backgroundColor: '#FF6B35', borderRadius: 12, paddingVertical: 16, alignItems: 'center' }}
        >
          {createExercise.isPending ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={{ color: '#fff', fontWeight: '600', fontSize: 15 }}>{t('exercises.create.submit')}</Text>
          )}
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
}
