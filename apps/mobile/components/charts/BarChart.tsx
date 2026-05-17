import { Text, View } from 'react-native';
import { useTheme } from '@/hooks/useTheme';

interface BarChartItem {
  label: string;
  value: number;
}

interface BarChartProps {
  data: BarChartItem[];
  height?: number;
  barColor?: string;
  unit?: string;
}

export function BarChart({ data, height = 120, barColor, unit }: BarChartProps) {
  const { colors } = useTheme();
  const color = barColor ?? colors.primary;
  const maxValue = Math.max(...data.map((d) => d.value), 1);

  return (
    <View style={{ flexDirection: 'row', alignItems: 'flex-end', height, gap: 4 }}>
      {data.map((item, i) => {
        const barHeight = Math.max((item.value / maxValue) * (height - 24), item.value > 0 ? 4 : 0);
        return (
          <View key={i} style={{ flex: 1, alignItems: 'center', justifyContent: 'flex-end', height }}>
            {item.value > 0 && (
              <Text style={{ color: colors.textSecondary, fontSize: 10, marginBottom: 2 }}>
                {unit ? `${item.value}${unit}` : item.value}
              </Text>
            )}
            <View
              style={{
                width: '80%',
                height: barHeight,
                backgroundColor: color,
                borderRadius: 4,
              }}
            />
            <Text
              style={{ color: colors.textDisabled, fontSize: 9, marginTop: 4, textAlign: 'center' }}
              numberOfLines={1}
            >
              {item.label}
            </Text>
          </View>
        );
      })}
    </View>
  );
}
