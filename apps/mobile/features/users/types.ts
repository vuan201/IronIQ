export interface UserProfile {
  id: string;
  email: string;
  name: string | null;
  age: number | null;
  heightCm: number | null;
  weightKg: number | null;
  goal: string | null;
  level: string | null;
  coinBalance: number;
  subscriptionTier: string;
  createdAt: string;
  currentStreak: number;
  longestStreak: number;
  isLeaderboardOptIn: boolean;
}

export interface UpdateProfileDto {
  name?: string | null;
  age?: number | null;
  heightCm?: number | null;
  weightKg?: number | null;
  goal?: string | null;
  level?: string | null;
  isLeaderboardOptIn?: boolean;
}

export const FITNESS_GOALS = [
  'WeightLoss',
  'MuscleGain',
  'Strength',
  'Endurance',
  'Flexibility',
  'GeneralFitness',
] as const;

export const FITNESS_LEVELS = ['Beginner', 'Intermediate', 'Advanced'] as const;
