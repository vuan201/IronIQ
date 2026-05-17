import * as SQLite from 'expo-sqlite';

let db: SQLite.SQLiteDatabase | null = null;

export function getOfflineDb(): SQLite.SQLiteDatabase {
  if (!db) db = SQLite.openDatabaseSync('ironiq_guest.db');
  return db;
}

export async function initOfflineDb(): Promise<void> {
  const database = getOfflineDb();

  await database.execAsync(`
    PRAGMA journal_mode = WAL;

    CREATE TABLE IF NOT EXISTS guest_workout_sessions (
      id TEXT PRIMARY KEY,
      name TEXT NOT NULL,
      started_at TEXT NOT NULL,
      completed_at TEXT,
      notes TEXT
    );

    CREATE TABLE IF NOT EXISTS guest_exercise_logs (
      id TEXT PRIMARY KEY,
      session_id TEXT NOT NULL,
      exercise_id TEXT NOT NULL,
      exercise_name TEXT NOT NULL,
      order_index INTEGER NOT NULL,
      FOREIGN KEY (session_id) REFERENCES guest_workout_sessions(id) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS guest_set_logs (
      id TEXT PRIMARY KEY,
      exercise_log_id TEXT NOT NULL,
      set_number INTEGER NOT NULL,
      weight_kg REAL,
      reps INTEGER,
      completed INTEGER NOT NULL DEFAULT 0,
      FOREIGN KEY (exercise_log_id) REFERENCES guest_exercise_logs(id) ON DELETE CASCADE
    );
  `);
}
