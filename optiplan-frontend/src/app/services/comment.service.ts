import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comment } from '../models/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = `${environment.apiUrl}/comment`;

  constructor(private http: HttpClient) { }

  GetCommentsByWorkItemIdAsync(workItemId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/get/${workItemId}`);
  }


  CreateComment(comment: Comment): Observable<Comment> {
    console.log("comment", comment);
    return this.http.post<Comment>(`${this.apiUrl}/create`, comment);
  }

}
