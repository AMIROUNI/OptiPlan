import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Skill } from '../models/skill';

@Injectable({
  providedIn: 'root'
})
export class SkillService {
  private apiUrl = `${environment.apiUrl}/skill`; 
  constructor(private http : HttpClient ) { }


  Get():Observable<Skill>{
    return this.http.get<Skill>(`${this.apiUrl}/user-skills`)
  }


  AddSkill(skill: Skill){
    return this.http.post(`${this.apiUrl}/add-skill`,skill)
  }


  Delete(skillID : string){
    return this.http.delete(`${this.apiUrl}/${skillID}`)
  }

}
