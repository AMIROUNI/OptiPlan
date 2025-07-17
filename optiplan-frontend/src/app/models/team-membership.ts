import { MembershipStatus } from "./enums/membership-status";
import { TeamRole } from "./enums/team-role";
import { Team } from "./team";
import { User } from "./user";


export interface TeamMembership {
  id: string;
  userId: string;
  user: User;
  teamId: string;
  team: Team;
  role: TeamRole;
  status: MembershipStatus;
  joinedAt: string;
}
