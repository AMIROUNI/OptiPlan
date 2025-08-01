import { TeamRole } from "../enums/team-role";

export class InvitationDto {
    teamId?: string;
    inviteeId?: string;    
    email?: string;
    role: TeamRole = TeamRole.Guest;  
    inviterId?: string;
  }
