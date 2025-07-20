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

  GetById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }





}
