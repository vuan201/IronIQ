import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Text, View } from 'react-native';
import { useTheme } from '@/hooks/useTheme';

interface SessionReviewCardProps {
  review: string | undefined;
  isLoading: boolean;
}

export function SessionReviewCard({ review, isLoading }: SessionReviewCardProps) {
  const { t } = useTranslation();
  const { colors } = useTheme();

  if (!isLoading && !review) return null;

  return (
    <View
      style={{
        backgroundColor: colors.surface,
        borderRadius: 20,
        padding: 20,
        marginBottom: 24,
        borderWidth: 1,
        borderColor: colors.border,
      }}
    >
      <View style={{ flexDirection: 'row', alignItems: 'center', marginBottom: 12, gap: 8 }}>
        <Text style={{ fontSize: 20 }}>🤖</Text>
        <Text style={{ color: colors.text, fontWeight: '700', fontSize: 15 }}>
          {t('session.review.title')}
        </Text>
      </View>

      {isLoading ? (
        <View style={{ flexDirection: 'row', alignItems: 'center', gap: 8 }}>
          <ActivityIndicator size="small" color={colors.primary} />
          <Text style={{ color: colors.textSecondary, fontSize: 14 }}>
            {t('session.review.loading')}
          </Text>
        </View>
      ) : (
        <Text style={{ color: colors.textSecondary, fontSize: 14, lineHeight: 22 }}>
          {review}
        </Text>
      )}
    </View>
  );
}
