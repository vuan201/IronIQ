import { create } from 'zustand';
import type { CoachMessage } from './types';

interface AiCoachState {
  messages: CoachMessage[];
  addUserMessage: (content: string) => void;
  addAssistantMessage: (content: string) => void;
  clearHistory: () => void;
}

export const useAiCoachStore = create<AiCoachState>((set) => ({
  messages: [],
  addUserMessage: (content) =>
    set((s) => ({ messages: [...s.messages, { role: 'user', content }] })),
  addAssistantMessage: (content) =>
    set((s) => ({ messages: [...s.messages, { role: 'assistant', content }] })),
  clearHistory: () => set({ messages: [] }),
}));
