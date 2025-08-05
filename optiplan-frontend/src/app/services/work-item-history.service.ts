import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkItemHistory } from '../models/work-item-history';

@Injectable({
  providedIn: 'root'
})
export class WorkItemHistoryService {

  private apiUrl = `${environment.apiUrl}/WorkItem`; 
  constructor(private http : HttpClient ) { }

  GetWorkItemHistoryForWorkItem(workItemId : string):Observable<WorkItemHistory[]>{
  return this.http.get<WorkItemHistory[]>(`${this.apiUrl}/get/${workItemId}`)
  }




}
