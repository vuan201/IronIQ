import { api } from '@/lib/api';
import type { UpdateProfileDto, UserProfile } from './types';

export const usersApi = {
  getMyProfile: () => api.get<UserProfile>('/users/me'),
  updateProfile: (data: UpdateProfileDto) => api.put<UserProfile>('/users/me', data),
};
