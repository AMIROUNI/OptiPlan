import { User } from "./user";

export interface WorkItemHistory {
  id: string;
  fieldChanged: string;
  oldValue: string;
  newValue: string;
  changedAt: Date;
  changedBy: User;
}
