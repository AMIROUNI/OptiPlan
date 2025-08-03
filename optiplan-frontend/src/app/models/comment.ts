import { User } from "./user";

export interface Comment {
  id?: string;
  content?: string;
  createdAt?: string;  
  workItemId?: string;
  authorId?: string;
  author?: User;      
  taskId?: string;
}