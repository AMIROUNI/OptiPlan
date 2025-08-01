import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../models/project';
import { ProjectDto } from '../models/dto/project.dto';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = `${environment.apiUrl}/project`; 
  constructor(private http : HttpClient ) { }


GetProjectsForUserAsync(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-projects-for-user`);
  }


  Create( projectDto:ProjectDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/user-create`, projectDto);
  }

  getProject(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }



  getTeamByProjectId(projectId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-team/${projectId}`);
  }


getTeamMemberShips(projectId: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/get-team-memberships/${projectId}`);
}


}
