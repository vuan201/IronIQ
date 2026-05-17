import { useRef, useState } from 'react';
import {
  ActivityIndicator,
  FlatList,
  KeyboardAvoidingView,
  Platform,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useTranslation } from 'react-i18next';
import { useRouter } from 'expo-router';
import { useTheme } from '@/hooks/useTheme';
import { useAiCoachStore } from '@/features/ai-coach/store';
import { useAskCoach } from '@/features/ai-coach/hooks';
import type { CoachMessage } from '@/features/ai-coach/types';

export default function AiCoachChatScreen() {
  const { t } = useTranslation();
  const { colors } = useTheme();
  const router = useRouter();
  const [input, setInput] = useState('');
  const flatListRef = useRef<FlatList>(null);

  const { messages, addUserMessage, addAssistantMessage, clearHistory } = useAiCoachStore();
  const askCoach = useAskCoach();

  const handleSend = async () => {
    const text = input.trim();
    if (!text || askCoach.isPending) return;

    addUserMessage(text);
    setInput('');

    const history = messages.map<CoachMessage>((m) => ({ role: m.role, content: m.content }));

    try {
      const result = await askCoach.mutateAsync({ message: text, history });
      addAssistantMessage(result.reply);
    } catch {
      addAssistantMessage('Sorry, something went wrong. Please try again.');
    }

    setTimeout(() => flatListRef.current?.scrollToEnd({ animated: true }), 100);
  };

  const renderMessage = ({ item }: { item: CoachMessage }) => {
    const isUser = item.role === 'user';
    return (
      <View
        style={{
          flexDirection: 'row',
          justifyContent: isUser ? 'flex-end' : 'flex-start',
          marginBottom: 12,
          paddingHorizontal: 16,
        }}
      >
        <View
          style={{
            maxWidth: '80%',
            backgroundColor: isUser ? colors.primary : colors.surface,
            borderRadius: 18,
            borderBottomRightRadius: isUser ? 4 : 18,
            borderBottomLeftRadius: isUser ? 18 : 4,
            paddingHorizontal: 14,
            paddingVertical: 10,
          }}
        >
          <Text style={{ color: isUser ? '#FFFFFF' : colors.text, fontSize: 15, lineHeight: 22 }}>
            {item.content}
          </Text>
        </View>
      </View>
    );
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.bg }}>
      <View
        style={{
          flexDirection: 'row',
          alignItems: 'center',
          justifyContent: 'space-between',
          paddingHorizontal: 16,
          paddingVertical: 12,
          borderBottomWidth: 1,
          borderBottomColor: colors.border,
        }}
      >
        <TouchableOpacity onPress={() => router.back()}>
          <Text style={{ color: colors.primary, fontSize: 16 }}>‹ Back</Text>
        </TouchableOpacity>
        <Text style={{ fontSize: 17, fontWeight: '700', color: colors.text }}>
          {t('aiCoach.chatTitle')}
        </Text>
        <TouchableOpacity onPress={clearHistory} disabled={messages.length === 0}>
          <Text style={{ color: messages.length > 0 ? colors.primary : colors.textSecondary, fontSize: 14 }}>
            {t('aiCoach.clearHistory')}
          </Text>
        </TouchableOpacity>
      </View>

      <FlatList
        ref={flatListRef}
        data={messages}
        keyExtractor={(_, i) => i.toString()}
        renderItem={renderMessage}
        contentContainerStyle={{ paddingVertical: 16, flexGrow: 1 }}
        onContentSizeChange={() => flatListRef.current?.scrollToEnd({ animated: false })}
        ListEmptyComponent={
          <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', padding: 32 }}>
            <Text style={{ fontSize: 36, marginBottom: 12 }}>🏋️</Text>
            <Text style={{ color: colors.textSecondary, textAlign: 'center', fontSize: 15 }}>
              Ask me anything about your training, nutrition, or recovery.
            </Text>
          </View>
        }
      />

      {askCoach.isPending && (
        <View style={{ flexDirection: 'row', alignItems: 'center', gap: 8, paddingHorizontal: 20, paddingBottom: 8 }}>
          <ActivityIndicator size="small" color={colors.primary} />
          <Text style={{ color: colors.textSecondary, fontSize: 13 }}>{t('aiCoach.thinking')}</Text>
        </View>
      )}

      <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View
          style={{
            flexDirection: 'row',
            alignItems: 'flex-end',
            gap: 10,
            paddingHorizontal: 16,
            paddingVertical: 12,
            borderTopWidth: 1,
            borderTopColor: colors.border,
          }}
        >
          <TextInput
            value={input}
            onChangeText={setInput}
            placeholder={t('aiCoach.chatPlaceholder')}
            placeholderTextColor={colors.textSecondary}
            multiline
            style={{
              flex: 1,
              backgroundColor: colors.surface,
              borderRadius: 20,
              paddingHorizontal: 16,
              paddingVertical: 10,
              color: colors.text,
              fontSize: 15,
              maxHeight: 120,
            }}
            onSubmitEditing={handleSend}
          />
          <TouchableOpacity
            onPress={handleSend}
            disabled={!input.trim() || askCoach.isPending}
            style={{
              backgroundColor: input.trim() && !askCoach.isPending ? colors.primary : colors.surface,
              borderRadius: 22,
              width: 44,
              height: 44,
              alignItems: 'center',
              justifyContent: 'center',
            }}
          >
            <Text style={{ fontSize: 18, color: input.trim() && !askCoach.isPending ? '#FFFFFF' : colors.textSecondary }}>
              ↑
            </Text>
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}
