import { useRouter } from 'expo-router';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Alert, Pressable, ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useGeneratePlan } from '@/features/ai-coach/hooks';
import { useTheme } from '@/hooks/useTheme';

const GOALS = ['build_muscle', 'lose_weight', 'maintain', 'increase_strength', 'improve_endurance'];
const LEVELS = ['beginner', 'intermediate', 'advanced'];
const DAYS_OPTIONS = [2, 3, 4, 5, 6];
const EQUIPMENT_OPTIONS = ['barbell', 'dumbbell', 'cables', 'machines', 'resistance_bands', 'pull_up_bar'];
const FOCUS_OPTIONS = ['chest', 'back', 'shoulders', 'arms', 'legs', 'core', 'full_body'];

export default function GenerateScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();
  const { mutateAsync: generate, isPending } = useGeneratePlan();

  const [goal, setGoal] = useState('build_muscle');
  const [level, setLevel] = useState('intermediate');
  const [days, setDays] = useState(3);
  const [equipment, setEquipment] = useState<string[]>(['barbell', 'dumbbell']);
  const [focus, setFocus] = useState<string[]>([]);

  const toggle = <T extends string>(list: T[], item: T, set: (v: T[]) => void) => {
    set(list.includes(item) ? list.filter((x) => x !== item) : [...list, item]);
  };

  const handleGenerate = async () => {
    try {
      await generate({
        goal,
        fitnessLevel: level,
        daysPerWeek: days,
        availableEquipment: equipment,
        focusAreas: focus,
      });
      Alert.alert(t('aiCoach.successTitle'), t('aiCoach.successMessage'), [
        { text: 'OK', onPress: () => router.replace('/(tabs)') },
      ]);
    } catch {
      Alert.alert(t('common.error'));
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingTop: 12, paddingBottom: 8 }}>
        <Pressable onPress={() => router.back()} style={{ padding: 4, marginRight: 12 }}>
          <Text style={{ color: colors.primary }}>{t('common.cancel')}</Text>
        </Pressable>
        <Text style={{ color: colors.text, fontWeight: '700', fontSize: 18, flex: 1 }}>
          {t('aiCoach.title')}
        </Text>
      </View>

      <ScrollView style={{ flex: 1, paddingHorizontal: 16 }} showsVerticalScrollIndicator={false}>

        <SectionLabel label={t('aiCoach.goal')} colors={colors} />
        <ChipGroup
          options={GOALS}
          selected={[goal]}
          onToggle={(v) => setGoal(v)}
          labelKey={(v) => t(`aiCoach.goals.${v}`)}
          single
          colors={colors}
        />

        <SectionLabel label={t('aiCoach.level')} colors={colors} />
        <ChipGroup
          options={LEVELS}
          selected={[level]}
          onToggle={(v) => setLevel(v)}
          labelKey={(v) => t(`aiCoach.levels.${v}`)}
          single
          colors={colors}
        />

        <SectionLabel label={t('aiCoach.daysPerWeek')} colors={colors} />
        <View style={{ flexDirection: 'row', gap: 8, marginBottom: 20 }}>
          {DAYS_OPTIONS.map((d) => (
            <TouchableOpacity
              key={d}
              onPress={() => setDays(d)}
              style={{
                flex: 1,
                paddingVertical: 10,
                borderRadius: 10,
                alignItems: 'center',
                backgroundColor: days === d ? colors.primary : colors.surface,
                borderWidth: 1,
                borderColor: days === d ? colors.primary : colors.border,
              }}
            >
              <Text style={{ color: days === d ? '#FFFFFF' : colors.text, fontWeight: '600' }}>{d}</Text>
            </TouchableOpacity>
          ))}
        </View>

        <SectionLabel label={t('aiCoach.equipment')} colors={colors} />
        <ChipGroup
          options={EQUIPMENT_OPTIONS}
          selected={equipment}
          onToggle={(v) => toggle(equipment, v, setEquipment)}
          labelKey={(v) => t(`aiCoach.equipmentOptions.${v}`)}
          colors={colors}
        />

        <SectionLabel label={t('aiCoach.focusAreas')} colors={colors} />
        <ChipGroup
          options={FOCUS_OPTIONS}
          selected={focus}
          onToggle={(v) => toggle(focus, v, setFocus)}
          labelKey={(v) => t(`aiCoach.focusOptions.${v}`)}
          colors={colors}
        />

        <TouchableOpacity
          onPress={handleGenerate}
          disabled={isPending}
          activeOpacity={0.85}
          style={{
            backgroundColor: colors.primary,
            borderRadius: 14,
            paddingVertical: 16,
            alignItems: 'center',
            marginTop: 8,
            marginBottom: 32,
            opacity: isPending ? 0.7 : 1,
          }}
        >
          {isPending ? (
            <ActivityIndicator color="#FFFFFF" />
          ) : (
            <Text style={{ color: '#FFFFFF', fontWeight: '700', fontSize: 16 }}>
              {t('aiCoach.generate')}
            </Text>
          )}
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
}

function SectionLabel({ label, colors }: { label: string; colors: ReturnType<typeof import('@/hooks/useTheme').useTheme>['colors'] }) {
  return (
    <Text style={{ color: colors.text, fontWeight: '600', fontSize: 15, marginBottom: 10, marginTop: 4 }}>
      {label}
    </Text>
  );
}

function ChipGroup({
  options,
  selected,
  onToggle,
  labelKey,
  single = false,
  colors,
}: {
  options: string[];
  selected: string[];
  onToggle: (v: string) => void;
  labelKey: (v: string) => string;
  single?: boolean;
  colors: ReturnType<typeof import('@/hooks/useTheme').useTheme>['colors'];
}) {
  return (
    <View style={{ flexDirection: 'row', flexWrap: 'wrap', gap: 8, marginBottom: 20 }}>
      {options.map((opt) => {
        const active = selected.includes(opt);
        return (
          <TouchableOpacity
            key={opt}
            onPress={() => onToggle(opt)}
            style={{
              paddingHorizontal: 14,
              paddingVertical: 8,
              borderRadius: 20,
              backgroundColor: active ? colors.primary : colors.surface,
              borderWidth: 1,
              borderColor: active ? colors.primary : colors.border,
            }}
          >
            <Text style={{ color: active ? '#FFFFFF' : colors.text, fontSize: 14, fontWeight: '500' }}>
              {labelKey(opt)}
            </Text>
          </TouchableOpacity>
        );
      })}
    </View>
  );
}
