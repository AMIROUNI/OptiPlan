import { ProjectTask } from "./projectTask";

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
  tasks?: ProjectTask[]; // À définir dans une autre interface
}
