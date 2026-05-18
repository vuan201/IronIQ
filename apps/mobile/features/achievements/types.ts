export interface Achievement {
  id: string;
  key: string;
  name: string;
  description: string;
  iconEmoji: string;
  type: 'Sessions' | 'Streak' | 'Sets';
  threshold: number;
  displayOrder: number;
  isUnlocked: boolean;
  unlockedAt: string | null;
}
