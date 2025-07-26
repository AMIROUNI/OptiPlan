import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { WorkItem } from '../models/work-item';
import { ProjectTaskDto } from '../models/dto/projectTask.dto';
import { Observable } from 'rxjs';
import { UpdateWorkItemStatusDto } from '../models/dto/updateWorkItemStatus.dto';

@Injectable({
  providedIn: 'root'
})
export class WorkItemService {

 private apiUrl = `${environment.apiUrl}/WorkItem`; 
  constructor(private http : HttpClient ) { }

  getWorkItemsByProject(projectId: string) {

    console.log('Fetching work items for project:', projectId);
    return this.http.get<any>(`${this.apiUrl}/get-by-project-id/${projectId}`);
  }


  createWorkItem(projectId: string, task: ProjectTaskDto) {
    console.log('Sending task data:', task);
    return this.http.post<any>(`${this.apiUrl}/create/${projectId}`, task);
  }

  updateTask(task: any, projectId: string) {
    console.log('Updating task with ID:', 'Data:');
    return this.http.put<any>(`${this.apiUrl}/update`, task);
  }



  getTasksForProject(projectId: string) {
    return this.http.get<WorkItem[]>(`${this.apiUrl}/get-tasks-for-project/${projectId}`);
  }
 
    createWorkItem2(workItem: Partial<WorkItem>): Observable<WorkItem> {
    return this.http.post<WorkItem>(this.apiUrl, workItem);
  }

  updateWorkItem(workItem: WorkItem): Observable<WorkItem> {
  const url = `${this.apiUrl}/${workItem.id}`;
  return this.http.put<WorkItem>(url, workItem);
}

  updateStatus(dto: UpdateWorkItemStatusDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update-status`, dto);
  }



}
