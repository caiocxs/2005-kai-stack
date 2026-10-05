export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginRequestWithEmail {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  username: string;
  email: string;
  password: string;
  permissions: number;
}

export interface UserDto {
  id: string;
  name: string;
  username: string;
  email: string;
  permissions: number;
}

export interface UserSummaryDto {
  id: string;
  name: string;
  username: string;
}

export interface UserDetailDto {
  id: string;
  name: string;
  username: string;
  email: string;
  permissions: number;
  isLocked: boolean;
  acessFailedCound: number;
}

export interface AuthResult {
  success: boolean;
  message?: string;
  user?: UserDto;
  token?: string;
  expiresat?: Date; //in need to format using dayjs
}
