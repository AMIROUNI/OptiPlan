import { User } from "./user";

export interface ProjectTask {
  id: string;  // Changed from number to string to match Guid
  title: string;  // Changed from Name to Title
  description: string;
  status: 'ToDo' | 'InProgress' | 'InReview' | 'Done' | 'Rejected' | 'Archived';
  type: 'Task' | 'Bug' | 'Feature' | 'Improvement' | 'Epic';  
  priority: 'Low' | 'Medium' | 'High' | 'Critical'; 
  projectId: string;
  AssignedUser?: User;
  reporter?: User;
  createdAt: Date;
  updatedAt: Date;
  dueDate?: Date;
  startDate?: Date;
  completedAt?: Date;
  estimatedHours: number;
  storyPoints?: number;
  completionPercentage: number;
  labels?: string;
  sprintId?: string;
  isBlocked: boolean;
  blockReason?: string;
  isCompleted: boolean;
}