export interface GeneratePlanDto {
  goal: string;
  fitnessLevel: string;
  daysPerWeek: number;
  availableEquipment: string[];
  focusAreas: string[];
}

export interface CoachMessage {
  role: 'user' | 'assistant';
  content: string;
}

export interface AskCoachDto {
  message: string;
  history: CoachMessage[];
}

export interface CoachResponseDto {
  reply: string;
}

export interface ProgressionSuggestion {
  exerciseId: string;
  exerciseName: string;
  currentWeightKg: number;
  suggestedWeightKg: number;
}

export interface SessionReview {
  review: string;
}
