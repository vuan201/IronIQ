export interface WeeklySessionCount {
  weekLabel: string;
  count: number;
}

export interface VolumePoint {
  dateLabel: string;
  totalVolumeKg: number;
}

export interface ProgressData {
  weeklySessionCounts: WeeklySessionCount[];
  recentVolume: VolumePoint[];
  totalSessionsAllTime: number;
  totalSetsAllTime: number;
}
