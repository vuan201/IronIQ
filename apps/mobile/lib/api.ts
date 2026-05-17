import axios from 'axios';
import { typedStorage, StorageKeys } from './storage';

const BASE_URL = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5000';

export const api = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'application/json' },
  timeout: 10000,
});

api.interceptors.request.use((config) => {
  const token = typedStorage.getString(StorageKeys.ACCESS_TOKEN);
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const original = error.config;
    if (error.response?.status !== 401 || original._retry) {
      return Promise.reject(error);
    }
    original._retry = true;

    const userId = typedStorage.getString(StorageKeys.USER_ID);
    const refreshToken = typedStorage.getString(StorageKeys.REFRESH_TOKEN);
    if (!userId || !refreshToken) return Promise.reject(error);

    try {
      const { data } = await axios.post(`${BASE_URL}/auth/refresh`, {
        userId,
        refreshToken,
      });
      typedStorage.setString(StorageKeys.ACCESS_TOKEN, data.accessToken);
      typedStorage.setString(StorageKeys.REFRESH_TOKEN, data.refreshToken);
      original.headers.Authorization = `Bearer ${data.accessToken}`;
      return api(original);
    } catch {
      typedStorage.delete(StorageKeys.ACCESS_TOKEN);
      typedStorage.delete(StorageKeys.REFRESH_TOKEN);
      return Promise.reject(error);
    }
  }
);
