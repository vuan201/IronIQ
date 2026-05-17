import { useEffect, useRef, useState } from 'react';
import { Text, TouchableOpacity, View } from 'react-native';
import { useTranslation } from 'react-i18next';
import { useTheme } from '@/hooks/useTheme';

interface RestTimerProps {
  seconds: number;
  onDismiss: () => void;
}

export function RestTimer({ seconds, onDismiss }: RestTimerProps) {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const [remaining, setRemaining] = useState(seconds);
  const intervalRef = useRef<ReturnType<typeof setInterval> | null>(null);

  useEffect(() => {
    intervalRef.current = setInterval(() => {
      setRemaining((prev) => {
        if (prev <= 1) {
          clearInterval(intervalRef.current!);
          onDismiss();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, []);

  const mins = Math.floor(remaining / 60);
  const secs = remaining % 60;
  const display = mins > 0 ? `${mins}:${String(secs).padStart(2, '0')}` : `${secs}s`;

  return (
    <View
      style={{
        backgroundColor: colors.surface2,
        borderRadius: 16,
        padding: 16,
        alignItems: 'center',
        gap: 10,
        marginVertical: 8,
      }}
    >
      <Text style={{ color: colors.textSecondary, fontSize: 12, fontWeight: '500', textTransform: 'uppercase', letterSpacing: 1 }}>
        {t('session.restTimer')}
      </Text>
      <Text style={{ color: colors.primary, fontSize: 44, fontWeight: '700' }}>{display}</Text>
      <TouchableOpacity
        onPress={onDismiss}
        activeOpacity={0.7}
        style={{
          backgroundColor: colors.border,
          borderRadius: 8,
          paddingHorizontal: 20,
          paddingVertical: 8,
        }}
      >
        <Text style={{ color: colors.text, fontSize: 14, fontWeight: '500' }}>{t('session.skipRest')}</Text>
      </TouchableOpacity>
    </View>
  );
}
