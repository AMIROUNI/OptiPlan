export interface ProjectTaskDto {
  title: string;
  description?: string;

  projectId: string;
  assignedUserId?: string;
  reporterId?: string;

  status?: 'ToDo' | 'InProgress' | 'InReview' | 'Done' | 'Rejected' | 'Archived';
  priority?: 'Low' | 'Medium' | 'High' | 'Critical';
  type?: 'Task' | 'Bug' | 'Feature' | 'Improvement' | 'Epic';

  dueDate?: Date;
  startDate?: Date;

  estimatedHours: number;
  storyPoints?: number;
  completionPercentage?: number;

  labels?: string;

  isBlocked?: boolean;
  blockReason?: string;

  sprintId?: string;
}
