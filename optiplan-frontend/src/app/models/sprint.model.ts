import { WorkItem } from "./work-item";

export interface Sprint {
  id: string;
  name: string;
  description?: string;
  startDate: Date;
  endDate: Date;
  isCompleted: boolean;
  projectId: string;
  // Optionnel : tu peux aussi inclure le projet complet si besoin
  // project?: Project;
  tasks?: WorkItem[]; // À définir dans une autre interface
}
