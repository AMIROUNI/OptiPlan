import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AttachmentService {

  private apiUrl = `${environment.apiUrl}/attachment`;

  constructor(private http: HttpClient) { }



  UploadAttachment(file: File, workItemId: string): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('workItemId', workItemId);
    console.log("formData", formData);
    return this.http.post<any>(`${this.apiUrl}/create`, formData);
  }



  GetAttachmentsByWorkItemId(workItemId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get/${workItemId}`);
  }




}
