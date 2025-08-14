import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import { Observable } from 'rxjs';
import { UserProfile } from '../models/dto/profile.dto';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  private apiUrl = `${environment.apiUrl}/userProfile`; 
  constructor(private http : HttpClient ) { }



  GetProfile():Observable<UserProfile>{
    return this.http.get<UserProfile>(`${this.apiUrl}`)
  }

  UpdateProfile(newDataProfile : UserProfile){
    return this.http.put(`${this.apiUrl}`,newDataProfile)
  }

  InitializeProfile(init : UserProfile){
    return this.http.post(`${this.apiUrl}/initialize-profile`,init)
  }


}
