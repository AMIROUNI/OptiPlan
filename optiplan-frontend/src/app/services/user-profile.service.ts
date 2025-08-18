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

  InitializeProfile(profile: UserProfile, avatarFile?: File, backgroundFile?: File) {
    const formData = new FormData();
  
    // Normal fields
    formData.append("Bio", profile.bio || "");
    formData.append("FullName", profile.fullName || "");
    formData.append("JobTitle", profile.jobTitle || "");
    formData.append("PhoneNumber", profile.phoneNumber || "");
    formData.append("CompanyName", profile.companyName || "");
    formData.append("Department", profile.department || "");
    formData.append("Country", profile.country || "");
  
    // Skills → convert to JSON string (because backend expects List<SkillDto>)
    if (profile.skills && profile.skills.length > 0) {
      formData.append("Skills", JSON.stringify(profile.skills));
    }
  
    // Files
    if (avatarFile) {
      formData.append("Avatar", avatarFile);
    }
    if (backgroundFile) {
      formData.append("Background", backgroundFile);
    }


    console.log(formData)
  
    return this.http.post(`${this.apiUrl}/initialize-profile`, formData);
  }
  

}
