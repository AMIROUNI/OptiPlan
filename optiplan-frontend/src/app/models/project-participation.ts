import { Project } from "./project";
import { User } from "./user";

export interface ProjectParticipation {
  id: string;
  projectId: string;
  project: Project;
  userId: string;
  user: User;
  contribution: string;
  roleInProject: string;
  estimatedHours: number;
  isActive: boolean;
  assignedAt: string;
}
