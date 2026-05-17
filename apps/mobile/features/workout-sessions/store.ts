import { create } from 'zustand';
import type { WorkoutSession } from './types';

interface CompletedSetEntry {
  exerciseId: string;
  setNumber: number;
  weightKg: number | null;
  reps: number | null;
}

interface SessionStore {
  session: WorkoutSession | null;
  completedSets: Record<string, CompletedSetEntry[]>; // keyed by exerciseId
  setSession: (session: WorkoutSession) => void;
  markSetCompleted: (exerciseId: string, entry: CompletedSetEntry) => void;
  isSetCompleted: (exerciseId: string, setNumber: number) => boolean;
  clearSession: () => void;
}

export const useSessionStore = create<SessionStore>((set, get) => ({
  session: null,
  completedSets: {},

  setSession: (session) => set({ session, completedSets: {} }),

  markSetCompleted: (exerciseId, entry) =>
    set((state) => ({
      completedSets: {
        ...state.completedSets,
        [exerciseId]: [...(state.completedSets[exerciseId] ?? []), entry],
      },
    })),

  isSetCompleted: (exerciseId, setNumber) =>
    (get().completedSets[exerciseId] ?? []).some((s) => s.setNumber === setNumber),

  clearSession: () => set({ session: null, completedSets: {} }),
}));
