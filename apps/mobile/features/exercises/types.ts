export interface Exercise {
  id: string;
  name: string;
  description: string | null;
  muscleGroups: string[];
  equipment: string[];
  difficulty: string;
  isSystem: boolean;
  createdByUserId: string | null;
}

export interface ExercisesPage {
  items: Exercise[];
  total: number;
  page: number;
  pageSize: number;
}

export interface ExerciseFilters {
  search?: string;
  muscle?: string;
  equipment?: string;
  difficulty?: string;
  page?: number;
  pageSize?: number;
}

export interface CreateExerciseDto {
  name: string;
  description?: string | null;
  muscleGroups: string[];
  equipment: string[];
  difficulty: string;
}
