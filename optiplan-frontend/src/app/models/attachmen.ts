import { User } from "./user";

export interface Attachment {
  id: string;
  fileName: string;
  url: string;
  uploadedAt: Date;
  uploadedBy: User;
}
