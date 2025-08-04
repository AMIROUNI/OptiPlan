import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChatBotService {

  private apiUrl = `${environment.apiUrl}/chatbot`;

  constructor(private http: HttpClient) { }
  

  AskBot(message: string, conversationId: string | null) {
    
  
    const body = {
      message: message,
      conversationId: conversationId
    };
  
    return this.http.post(`${this.apiUrl}/ask`, body);
  }


  DeleteConversation(conversationId : string){
    return this.http.delete(`${this.apiUrl}/delete-conversation/${conversationId}`)
  }


  GetConversation(conversationId : string){
    return this.http.get(`${this.apiUrl}/get-conversation/${conversationId}`)
  }


  GetConversationsByUserId(userId :string ){
    return this.http.get(`${this.apiUrl}/get-conversations/user/${userId}`)
  }


  NewConversation(title: string | null) {
    return this.http.post(`${this.apiUrl}/new-conversation`, {
      title: title ?? ''
    });
  }
  

}

