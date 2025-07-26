import { Attachment } from "./attachmen";
import { Comment } from "./comment";
import { WorkItemPriority } from "./enums/work-item-priority";
import { WorkItemStatus } from "./enums/work-item-status";
import { WorkItemType } from "./enums/work-item-type";
import { User } from "./user";
import { WorkItemHistory } from "./work-item-history";


export interface WorkItem {
  updatedAt: string | number | Date;
  id: string;
  title: string;
  description?: string;

  type: WorkItemType;
  status: WorkItemStatus;
  priority: WorkItemPriority;

  projectId: string;
  sprintId?: string;

  parentId?: string;
  parent?: WorkItem;

  subItems?: WorkItem[];

  assignedUserId?: string;
  assignedUser?: User;

  reporterId?: string;
  reporter?: User;

  startDate?: Date;
  estimatedHours?: number;
  labels?: string;

  comments?: Comment  [];
  attachments?: Attachment[];

  createdAt: Date;
  dueDate?: Date;
  completedAt?: Date;

  storyPoints?: number;
  completionPercentage: number;

  isBlocked: boolean;
  blockReason?: string;

  // Calculated in backend
  isCompleted: boolean;

  history?: WorkItemHistory[];  // Define this interface if not already done
}
