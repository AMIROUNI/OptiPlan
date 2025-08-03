import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WorkItemHistoryService {

  private apiUrl = `${environment.apiUrl}/WorkItem`; 
  constructor(private http : HttpClient ) { }

  GetWorkItemHistoryForWorkItem(workItemId : string){
  return this.http.get
  }
}
