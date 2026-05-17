import { create } from 'zustand';
import { typedStorage, StorageKeys } from '@/lib/storage';
import type { AuthUser } from './types';

interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  user: AuthUser | null;
  isAuthenticated: boolean;
  setAuth: (accessToken: string, refreshToken: string, user: AuthUser) => void;
  updateAccessToken: (token: string) => void;
  clearAuth: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  accessToken: typedStorage.getString(StorageKeys.ACCESS_TOKEN) ?? null,
  refreshToken: typedStorage.getString(StorageKeys.REFRESH_TOKEN) ?? null,
  user: null,
  isAuthenticated: !!typedStorage.getString(StorageKeys.ACCESS_TOKEN),

  setAuth: (accessToken, refreshToken, user) => {
    typedStorage.setString(StorageKeys.ACCESS_TOKEN, accessToken);
    typedStorage.setString(StorageKeys.REFRESH_TOKEN, refreshToken);
    typedStorage.setString(StorageKeys.USER_ID, user.userId);
    set({ accessToken, refreshToken, user, isAuthenticated: true });
  },

  updateAccessToken: (token) => {
    typedStorage.setString(StorageKeys.ACCESS_TOKEN, token);
    set({ accessToken: token });
  },

  clearAuth: () => {
    typedStorage.delete(StorageKeys.ACCESS_TOKEN);
    typedStorage.delete(StorageKeys.REFRESH_TOKEN);
    typedStorage.delete(StorageKeys.USER_ID);
    set({ accessToken: null, refreshToken: null, user: null, isAuthenticated: false });
  },
}));
