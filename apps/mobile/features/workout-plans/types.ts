export interface PlanExercise {
  id: string;
  exerciseId: string;
  exerciseName: string;
  order: number;
  sets: number;
  reps: number | null;
  durationSeconds: number | null;
  restSeconds: number;
  notes: string | null;
}

export interface WorkoutDay {
  id: string;
  dayOfWeek: number; // 0=Mon … 6=Sun
  name: string | null;
  exercises: PlanExercise[];
}

export interface WorkoutPlan {
  id: string;
  name: string;
  description: string | null;
  isActive: boolean;
  createdAt: string;
  days: WorkoutDay[];
}

export interface CreatePlanExerciseDto {
  exerciseId: string;
  order: number;
  sets: number;
  reps?: number | null;
  durationSeconds?: number | null;
  restSeconds: number;
  notes?: string | null;
}

export interface CreateWorkoutDayDto {
  dayOfWeek: number;
  name?: string | null;
  exercises: CreatePlanExerciseDto[];
}

export interface CreateWorkoutPlanDto {
  name: string;
  description?: string | null;
  days: CreateWorkoutDayDto[];
}

export interface UpdateWorkoutPlanDto {
  name: string;
  description?: string | null;
  days: CreateWorkoutDayDto[];
}
