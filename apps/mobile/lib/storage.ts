import { MMKV } from 'react-native-mmkv';

export const storage = new MMKV({ id: 'ironiq-storage' });

export const StorageKeys = {
  THEME: 'theme',
  LANGUAGE: 'language',
  ACCESS_TOKEN: 'access_token',
  REFRESH_TOKEN: 'refresh_token',
  USER_ID: 'user_id',
} as const;

type StorageKey = (typeof StorageKeys)[keyof typeof StorageKeys];

export const typedStorage = {
  getString: (key: StorageKey) => storage.getString(key),
  setString: (key: StorageKey, value: string) => storage.set(key, value),
  delete: (key: StorageKey) => storage.delete(key),
};
