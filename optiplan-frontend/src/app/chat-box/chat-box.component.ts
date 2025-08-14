import { Component, Input, OnInit } from '@angular/core';

import { Message } from '../models/message';

import { CommonModule } from '@angular/common';
import { ChatHubService } from '../services/chat-hub.service';


@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.css'],
  imports:[CommonModule,]
})
export class ChatBoxComponent implements OnInit {
  @Input() selectedUserId!: string;
  messages: Message[] = [];
  newMessage = '';

  constructor(private chatService: ChatHubService) {}

  ngOnInit() {
    const token = localStorage.getItem('access_token'); // ou où tu stockes le JWT
  if (token) {
    this.chatService.connect(token);
  }
    this.chatService.joinChat(this.selectedUserId);
    this.chatService.onMessageReceived((msg: Message) => {
      this.messages.push(msg);
    });
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      this.chatService.sendMessage(this.selectedUserId, this.newMessage);
      this.messages.push({
        senderId: 'me', // optionally replace with real user ID
        content: this.newMessage,
        sentAt: new Date()
      });
      this.newMessage = '';
    }
  }
}