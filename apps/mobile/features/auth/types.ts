export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  userId: string;
  email: string;
}

export interface RegisterDto {
  email: string;
  password: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RefreshTokenDto {
  userId: string;
  refreshToken: string;
}

export interface AuthUser {
  userId: string;
  email: string;
}
