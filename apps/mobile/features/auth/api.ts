import { api } from '@/lib/api';
import type { AuthResponse, LoginDto, RefreshTokenDto, RegisterDto } from './types';

export const authApi = {
  register: (data: RegisterDto) =>
    api.post<AuthResponse>('/auth/register', data),

  login: (data: LoginDto) =>
    api.post<AuthResponse>('/auth/login', data),

  refreshToken: (data: RefreshTokenDto) =>
    api.post<AuthResponse>('/auth/refresh', data),
};
