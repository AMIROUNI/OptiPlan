import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class ChatHubService {

  private hubConnection!: signalR.HubConnection;

  connect(token: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7078/chathub?access_token=' + token)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(console.error);
  }

  joinChat(receiverId: string) {
    this.hubConnection.invoke('JoinChat', receiverId).catch(console.error);
  }

  sendMessage(receiverId: string, content: string) {
    this.hubConnection.invoke('SendMessage', receiverId, content).catch(console.error);
  }

  onMessageReceived(callback: (message: Message) => void) {
    this.hubConnection.on('ReceiveMessage', callback);
  }
}

