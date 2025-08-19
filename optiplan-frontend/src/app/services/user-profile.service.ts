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



  GetProfile(username : string ):Observable<UserProfile>{
    return this.http.get<UserProfile>(`${this.apiUrl}/${username}`)
  }

  UpdateProfile(newDataProfile : UserProfile){
    return this.http.put(`${this.apiUrl}`,newDataProfile)
  }

  InitializeProfile(profile: { bio: any; }, avatarFile?: File, backgroundFile?: File) {
    const formData = new FormData();
  
    formData.append("Bio", profile.bio || "");
   
    if (avatarFile) {
      formData.append("Avatar", avatarFile, avatarFile.name);
    }
    if (backgroundFile) {
      formData.append("Background", backgroundFile, backgroundFile.name);
    }
  
    return this.http.post(`${this.apiUrl}/initialize-profile`, formData);
  }
  
  

}
