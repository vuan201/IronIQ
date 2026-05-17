import { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  FlatList,
  TouchableOpacity,
  ActivityIndicator,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { useTranslation } from 'react-i18next';
import { useExercises } from '@/features/exercises/hooks';
import { ExerciseCard } from '@/components/workout/ExerciseCard';
import { ExerciseCardSkeleton } from '@/components/ui/Skeleton';
import { useTheme } from '@/hooks/useTheme';

const MUSCLE_FILTERS = ['Chest', 'Back', 'Shoulders', 'Biceps', 'Triceps', 'Abs', 'Legs', 'Cardio'];

export default function ExploreScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { colors } = useTheme();

  const [search, setSearch] = useState('');
  const [selectedMuscle, setSelectedMuscle] = useState<string | undefined>();

  const { data, isLoading, isFetching } = useExercises({
    search: search || undefined,
    muscle: selectedMuscle,
    pageSize: 50,
  });

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View style={{ paddingHorizontal: 16, paddingBottom: 8 }}>
        <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
          <Text style={{ fontSize: 24, fontWeight: '700', color: colors.text }}>
            {t('exercises.title')}
          </Text>
          <TouchableOpacity
            onPress={() => router.push('/exercise/create')}
            style={{ backgroundColor: '#FF6B35', paddingHorizontal: 12, paddingVertical: 6, borderRadius: 8 }}
          >
            <Text style={{ color: '#fff', fontWeight: '600', fontSize: 13 }}>+ {t('exercises.add')}</Text>
          </TouchableOpacity>
        </View>

        <TextInput
          value={search}
          onChangeText={setSearch}
          placeholder={t('exercises.search')}
          placeholderTextColor={colors.textSecondary}
          style={{
            backgroundColor: colors.surface,
            borderRadius: 12,
            paddingHorizontal: 16,
            paddingVertical: 12,
            color: colors.text,
            fontSize: 15,
            marginBottom: 12,
          }}
        />

        <FlatList
          horizontal
          showsHorizontalScrollIndicator={false}
          data={MUSCLE_FILTERS}
          keyExtractor={(item) => item}
          renderItem={({ item }) => {
            const active = selectedMuscle === item;
            return (
              <TouchableOpacity
                onPress={() => setSelectedMuscle(active ? undefined : item)}
                style={{
                  paddingHorizontal: 14,
                  paddingVertical: 7,
                  borderRadius: 20,
                  backgroundColor: active ? '#FF6B35' : colors.surface,
                  marginRight: 8,
                }}
              >
                <Text style={{ color: active ? '#fff' : colors.textSecondary, fontSize: 13, fontWeight: '500' }}>
                  {item}
                </Text>
              </TouchableOpacity>
            );
          }}
          style={{ marginBottom: 4 }}
        />
      </View>

      {isLoading ? (
        <View style={{ paddingHorizontal: 16 }}>
          {[...Array(6)].map((_, i) => <ExerciseCardSkeleton key={i} />)}
        </View>
      ) : (
        <FlatList
          data={data?.items ?? []}
          keyExtractor={(item) => item.id}
          contentContainerStyle={{ paddingHorizontal: 16, paddingBottom: 24 }}
          renderItem={({ item }) => (
            <ExerciseCard
              exercise={item}
              onPress={(id) => router.push(`/exercise/${id}`)}
            />
          )}
          ListFooterComponent={isFetching ? <ActivityIndicator color="#FF6B35" /> : null}
          ListEmptyComponent={
            <View style={{ alignItems: 'center', paddingTop: 48 }}>
              <Text style={{ color: colors.textSecondary }}>{t('exercises.empty')}</Text>
            </View>
          }
        />
      )}
    </SafeAreaView>
  );
}
