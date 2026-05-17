import { getOfflineDb } from './offline-db';
import { api } from './api';

export interface SyncResult {
  sessionsUploaded: number;
  errors: string[];
}

export async function syncGuestDataToServer(): Promise<SyncResult> {
  const db = getOfflineDb();
  const result: SyncResult = { sessionsUploaded: 0, errors: [] };

  const sessions = await db.getAllAsync<{
    id: string;
    name: string;
    started_at: string;
    completed_at: string | null;
    notes: string | null;
  }>('SELECT * FROM guest_workout_sessions WHERE completed_at IS NOT NULL');

  for (const session of sessions) {
    try {
      const logs = await db.getAllAsync<{
        id: string;
        exercise_id: string;
        exercise_name: string;
        order_index: number;
      }>('SELECT * FROM guest_exercise_logs WHERE session_id = ?', [session.id]);

      const logsWithSets = await Promise.all(
        logs.map(async (log) => {
          const sets = await db.getAllAsync<{
            set_number: number;
            weight_kg: number | null;
            reps: number | null;
            completed: number;
          }>('SELECT * FROM guest_set_logs WHERE exercise_log_id = ? ORDER BY set_number', [log.id]);
          return { ...log, sets };
        })
      );

      await api.post('/workout-sessions/import', {
        name: session.name,
        startedAt: session.started_at,
        completedAt: session.completed_at,
        notes: session.notes,
        logs: logsWithSets,
      });

      await db.runAsync('DELETE FROM guest_workout_sessions WHERE id = ?', [session.id]);
      result.sessionsUploaded++;
    } catch (e) {
      result.errors.push(`Session ${session.id}: ${String(e)}`);
    }
  }

  return result;
}
