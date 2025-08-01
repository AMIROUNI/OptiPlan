
import { InvitationStatus } from './enums/invitation-status';
import { TeamRole } from './enums/team-role';
import { Team } from './team';
import { User } from './user';

export interface Invitation {
  id: string;
  teamId: string;
  team: Team;

  // Invited user (nullable in C#)
  inviteeId?: string;
  invitee?: User;

  role: TeamRole;

  email: string;

  status: InvitationStatus;
  sentAt: string;
  respondedAt?: string;

  inviterId: string;
  inviter: User;

  /** @deprecated Use 'status' instead */
  isAccepted: boolean;
}
