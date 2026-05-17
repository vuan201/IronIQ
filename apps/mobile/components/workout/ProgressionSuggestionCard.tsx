import { Text, View } from 'react-native';
import { useTranslation } from 'react-i18next';
import { useTheme } from '@/hooks/useTheme';
import type { ProgressionSuggestion } from '@/features/ai-coach/types';

interface Props {
  suggestions: ProgressionSuggestion[];
}

export function ProgressionSuggestionCard({ suggestions }: Props) {
  const { t } = useTranslation();
  const { colors } = useTheme();

  if (suggestions.length === 0) return null;

  return (
    <View
      style={{
        backgroundColor: colors.surface,
        borderRadius: 20,
        padding: 20,
        marginBottom: 16,
        borderWidth: 1,
        borderColor: colors.border,
      }}
    >
      <Text style={{ color: colors.text, fontSize: 16, fontWeight: '700', marginBottom: 4 }}>
        {t('progression.title')}
      </Text>
      <Text style={{ color: colors.textSecondary, fontSize: 13, marginBottom: 14 }}>
        {t('progression.subtitle')}
      </Text>
      {suggestions.map((s, index) => (
        <View
          key={s.exerciseId}
          style={{
            flexDirection: 'row',
            justifyContent: 'space-between',
            alignItems: 'center',
            paddingVertical: 10,
            ...(index < suggestions.length - 1
              ? { borderBottomWidth: 1, borderBottomColor: colors.border }
              : {}),
          }}
        >
          <Text style={{ color: colors.text, fontWeight: '500', flex: 1 }} numberOfLines={1}>
            {s.exerciseName}
          </Text>
          <View style={{ flexDirection: 'row', alignItems: 'center', gap: 8 }}>
            <Text style={{ color: colors.textSecondary, fontSize: 14 }}>{s.currentWeightKg}kg</Text>
            <Text style={{ color: colors.primary, fontSize: 16 }}>→</Text>
            <Text style={{ color: colors.primary, fontWeight: '700', fontSize: 14 }}>
              {s.suggestedWeightKg}kg
            </Text>
          </View>
        </View>
      ))}
    </View>
  );
}
