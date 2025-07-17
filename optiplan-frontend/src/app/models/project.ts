import { ProjectParticipation } from "./project-participation";
import { Team } from "./team";
import { User } from "./user";


export interface Project {
  id: string;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  ownerId: string;
  owner: User;
  team: Team;
  participants: ProjectParticipation[];
}
