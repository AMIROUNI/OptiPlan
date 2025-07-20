import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Kpi } from '../models/dashboard/kpi';

@Injectable({
  providedIn: 'root'
})
export class UserDashboardService {

  private apiUrl = `${environment.apiUrl}/dashboard`; 

  constructor(private http : HttpClient) { }

  getKpis():Observable<Kpi[]> {
    return this.http.get<Kpi[]>(`${this.apiUrl}/kpis`);
  }


  GetUserTasksGroupedByProjectForMonth(userTasks :FormData):Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/tasks-grouped-by-project-for-month`);
  }
}
