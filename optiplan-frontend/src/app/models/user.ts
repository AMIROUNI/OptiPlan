import { Role } from "./enums/role";


export interface User {
  id: string;
  username: string;
  email: string;
  passwordHash: string;
  role: Role;

  fullName: string;
  jobTitle: string;
  phoneNumber: string;
  avatarUrl: string;

  companyName?: string;
  department?: string;
  country?: string;
  firstLogin? : boolean

  refreshToken?: string;
  refreshTokenExpiryTime?: string;
}
