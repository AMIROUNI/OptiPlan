import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SprintDto } from '../models/dto/sprint.dto';
import { Observable } from 'rxjs';
import { Sprint } from '../models/sprint.model';

@Injectable({
  providedIn: 'root'
})
export class SprintService {

 private apiUrl = `${environment.apiUrl}/sprint`; 
  constructor(private http : HttpClient ) { }


  GetSprintsForProject(projectId: string): Observable<Sprint[]> {
    return this.http.get<Sprint[]>(`${this.apiUrl}/project/${projectId}`);
  }

  CreateSprint(projectId: string, sprintData: SprintDto) {
    return this.http.post<any>(`${this.apiUrl}/project/${projectId}`, sprintData);
  }
}
