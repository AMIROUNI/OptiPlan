// services/chat-api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Message } from '../models/message';
import { environment } from '../../environments/environment';
import { firstValueFrom, Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class ChatApiService {
  private apiUrl = `${environment.apiUrl}/messages`; 

  constructor(private http: HttpClient) {}

  // Get message history
  async getMessageHistory(receiverUsername: string): Promise<Message[]> {
    try {
      return await firstValueFrom(
        this.http.get<Message[]>(`${this.apiUrl}/history/${receiverUsername}`)
      );
    } catch (error) {
      console.error('Error fetching message history:', error);
      return [];
    }
  }
 
  getMessageHistoryHttp(receiverUsername: string): Promise<Message[]> {
    return this.http.get<Message[]>(`${this.apiUrl}/history/${receiverUsername}`)
      .toPromise()
      .then(messages => messages ?? [])
      .catch(error => {
        console.error('Error fetching message history (HttpClient):', error);
        return [];
      });
  }


  getUsersIHaveChatWithIt():Observable<User[]>{
    return this.http.get<User[]>(`${this.apiUrl}/uesrs-i-have-chat`)
  }
}