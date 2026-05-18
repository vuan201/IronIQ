import { Text, View } from 'react-native';
import type { Achievement } from '@/features/achievements/types';
import { useTheme } from '@/hooks/useTheme';

interface AchievementBadgeProps {
  achievement: Achievement;
}

export function AchievementBadge({ achievement }: AchievementBadgeProps) {
  const { colors } = useTheme();
  const locked = !achievement.isUnlocked;

  return (
    <View
      style={{
        backgroundColor: colors.surface,
        borderRadius: 16,
        padding: 16,
        borderWidth: 1,
        borderColor: locked ? colors.border : '#FF6B35',
        opacity: locked ? 0.5 : 1,
        flexDirection: 'row',
        alignItems: 'center',
        gap: 14,
      }}
    >
      <Text style={{ fontSize: 32 }}>{locked ? '🔒' : achievement.iconEmoji}</Text>
      <View style={{ flex: 1 }}>
        <Text style={{ color: colors.text, fontWeight: '700', fontSize: 15 }}>
          {achievement.name}
        </Text>
        <Text style={{ color: colors.textSecondary, fontSize: 13, marginTop: 2 }}>
          {achievement.description}
        </Text>
      </View>
      {achievement.isUnlocked && (
        <Text style={{ fontSize: 11, color: '#FF6B35', fontWeight: '600' }}>✓</Text>
      )}
    </View>
  );
}
