import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ProjectTask } from '../models/projectTask';
import { ProjectTaskDto } from '../models/dto/projectTask.dto';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

 private apiUrl = `${environment.apiUrl}/projectTask`; 
  constructor(private http : HttpClient ) { }

  getTasksForProject(projectId: string) {
    return this.http.get<any>(`${this.apiUrl}/get-by-project-id/${projectId}`);
  }


  createTaskForProject(projectId: string, task: ProjectTaskDto) {
    console.log('Sending task data:', task);
    return this.http.post<any>(`${this.apiUrl}/create/${projectId}`, task);
  }

  updateTask(task: any, projectId: string) {
    console.log('Updating task with ID:', 'Data:');
    return this.http.put<any>(`${this.apiUrl}/update`, task);
  }
}
