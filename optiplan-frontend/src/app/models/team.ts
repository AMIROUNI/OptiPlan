import { Invitation } from "./invitation";
import { Project } from "./project";
import { TeamMembership } from "./team-membership";

export interface Team {
  id: string;
  name: string;
  projectId: string;
  project: Project;
  members: TeamMembership[];
  invitations: Invitation[];
  createdAt: string; // ISO format, à convertir si tu veux une Date avec new Date(createdAt)
}
