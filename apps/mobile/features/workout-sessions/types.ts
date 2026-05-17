export interface WorkoutSession {
  id: string;
  workoutPlanId: string | null;
  workoutDayId: string | null;
  startedAt: string;
  completedAt: string | null;
  status: 'Active' | 'Completed' | 'Cancelled';
  notes: string | null;
  exerciseLogs: ExerciseLog[];
}

export interface ExerciseLog {
  id: string;
  exerciseId: string;
  exerciseName: string;
  order: number;
  sets: SetLog[];
}

export interface SetLog {
  id: string;
  setNumber: number;
  weightKg: number | null;
  reps: number | null;
  durationSeconds: number | null;
  isCompleted: boolean;
}

export interface WorkoutSessionSummary {
  id: string;
  workoutPlanId: string | null;
  startedAt: string;
  completedAt: string | null;
  status: string;
  totalExercises: number;
  totalSets: number;
}

export interface SessionHistoryPage {
  items: WorkoutSessionSummary[];
  total: number;
  page: number;
  pageSize: number;
}

export interface StartSessionDto {
  workoutPlanId?: string | null;
  workoutDayId?: string | null;
}

export interface LogSetDto {
  exerciseId: string;
  setNumber: number;
  weightKg?: number | null;
  reps?: number | null;
  durationSeconds?: number | null;
}
