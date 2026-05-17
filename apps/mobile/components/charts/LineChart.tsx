import { Text, View } from 'react-native';
import { useTheme } from '@/hooks/useTheme';

interface LineChartItem {
  label: string;
  value: number;
}

interface LineChartProps {
  data: LineChartItem[];
  height?: number;
  lineColor?: string;
}

export function LineChart({ data, height = 120, lineColor }: LineChartProps) {
  const { colors } = useTheme();
  const color = lineColor ?? colors.primary;
  const chartHeight = height - 28;
  const maxValue = Math.max(...data.map((d) => d.value), 1);

  if (data.length === 0) {
    return (
      <View style={{ height, justifyContent: 'center', alignItems: 'center' }}>
        <Text style={{ color: colors.textDisabled }}>—</Text>
      </View>
    );
  }

  return (
    <View>
      <View style={{ flexDirection: 'row', alignItems: 'flex-end', height: chartHeight, gap: 4 }}>
        {data.map((item, i) => {
          const fillHeight = Math.max((item.value / maxValue) * chartHeight, item.value > 0 ? 6 : 0);
          return (
            <View key={i} style={{ flex: 1, alignItems: 'center', justifyContent: 'flex-end', height: chartHeight }}>
              <View style={{ width: '80%', height: fillHeight, borderRadius: 4, backgroundColor: color, opacity: 0.85 }}>
                <View
                  style={{
                    width: '100%',
                    height: 6,
                    borderRadius: 3,
                    backgroundColor: color,
                    position: 'absolute',
                    top: 0,
                  }}
                />
              </View>
            </View>
          );
        })}
      </View>

      <View style={{ flexDirection: 'row', marginTop: 6, gap: 4 }}>
        {data.map((item, i) => (
          <Text
            key={i}
            style={{ flex: 1, color: colors.textDisabled, fontSize: 9, textAlign: 'center' }}
            numberOfLines={1}
          >
            {item.label}
          </Text>
        ))}
      </View>
    </View>
  );
}
