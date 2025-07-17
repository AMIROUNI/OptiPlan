import { Team } from "./team";


export interface Invitation {
  id: string;
  teamId: string;
  team: Team;
  email: string;
  sentAt: string;
  isAccepted: boolean;
}
