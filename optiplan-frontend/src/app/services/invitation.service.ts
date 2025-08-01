import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { InvitationDto } from '../models/dto/invitation.dto';
import { Observable } from 'rxjs';
import { Invitation } from '../models/invitation';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {

  private apiUrl = `${environment.apiUrl}/invitation`;

  constructor(private http: HttpClient) { }


  sendInvitation(invitationDto: InvitationDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/send`, invitationDto);
  }


  AccepteInitation(invitationId : string ){
    return this.http.post<any>(`${this.apiUrl}/accept/${invitationId}`, {});
  }


  RejectInvitation(invitationId : string ){
    return this.http.post<any>(`${this.apiUrl}/reject/${invitationId}`, {});
  }


  GetAllInvitationForUser(){
    return this.http.get<Invitation[]>(`${this.apiUrl}/get`)
  }
}
