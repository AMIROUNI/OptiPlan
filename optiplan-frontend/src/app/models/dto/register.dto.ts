export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  fullName: string;
  jobTitle: string;
  phoneNumber: string;
  avatarUrl: string;
  companyName?: string;
  department?: string;
  country?: string;
}