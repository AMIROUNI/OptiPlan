import { Skill } from './skill';

export interface UserProfile {
  id: string;
  userId: string;
  bio: string;
  createdAt: string;
  updatedAt?: string;
  skills: Skill[];
}
