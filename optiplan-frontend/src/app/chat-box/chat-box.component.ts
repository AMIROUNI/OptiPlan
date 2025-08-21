import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHubService } from '../services/chat-hub.service';
import { ChatApiService } from '../services/chat-api.service';
import { Message } from '../models/message';
import { Subscription } from 'rxjs';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class ChatBoxComponent implements OnInit, OnDestroy {
  @Input() selectedUserUsername!: string;
  @Input() selectedUserId!: string;

  messages: Message[] = [];
  newMessage = '';
  isConnected = false;
  isLoading = false;
  errorMessage = '';

  private connectionSubscription!: Subscription;
  private currentUserId = '';
  private currentUserUsername = '';

  constructor(
    private chatService: ChatHubService,
    private chatApiService: ChatApiService
  ) {}

  async ngOnInit() {
    this.currentUserId = this.getCurrentUserId();
    this.currentUserUsername = this.getCurrentUserUsername();

    await this.loadMessageHistory();
    await this.connectToChat();
  }

  private async loadMessageHistory() {
    this.isLoading = true;
    this.errorMessage = '';

    try {
      const history = await this.chatApiService.getMessageHistory(this.selectedUserUsername);
      this.messages = history.map(msg => ({
        ...msg,
        id: msg.id || uuidv4(),
        sentAt: new Date(msg.sentAt), // Convert to Date object
        displaySender: msg.senderId === this.currentUserId ? 'me' : msg.senderUsername
      }));
      // Sort messages by sentAt ascending
      this.messages.sort((a, b) => a.sentAt.getTime() - b.sentAt.getTime());
    } catch (error) {
      console.error('Failed to load message history:', error);
      this.errorMessage = 'Failed to load conversation history';
    } finally {
      this.isLoading = false;
    }
  }

  private async connectToChat() {
    const token = localStorage.getItem('token'); // Fixed to 'access_token' as in getCurrentUserId
    if (!token) {
      this.errorMessage = 'No authentication token found';
      return;
    }

    try {
      await this.chatService.connect(token);

      // Subscribe to connection status
      this.connectionSubscription = this.chatService.connectionStatus$.subscribe(
        status => (this.isConnected = status)
      );

      // Join chat group
      await this.chatService.joinChat(this.selectedUserUsername);

      // Listen for incoming messages
      this.chatService.onMessageReceived((msg: any) => {
        // Transform and prepare to add
        const transformed: Message = {
          id: msg.id || uuidv4(),
          senderId: msg.senderId,
          senderUsername: msg.senderUsername,
          content: msg.content,
          sentAt: new Date(msg.sentAt),
          chatId: msg.chatId,
          displaySender: msg.senderId === this.currentUserId ? 'me' : msg.senderUsername
        };

        // Avoid duplicates based on content + timestamp
        const exists = this.messages.some(
          m => m.content === transformed.content &&
               Math.abs(m.sentAt.getTime() - transformed.sentAt.getTime()) < 5000
        );

        if (!exists) {
          this.messages.push(transformed);
          // Sort messages after adding new one
          this.messages.sort((a, b) => a.sentAt.getTime() - b.sentAt.getTime());
        }
      });

      this.isConnected = true;
    } catch (error: any) {
      console.error('SignalR connection failed:', error);
      this.errorMessage = error.message || 'Failed to connect to chat';
      this.isConnected = false;
    }
  }

  async sendMessage() {
    if (!this.newMessage.trim() || !this.isConnected) return;

    const contentToSend = this.newMessage;
    this.newMessage = '';

    try {
      await this.chatService.sendMessage(this.selectedUserUsername, contentToSend);
      // No local push here — rely on SignalR
    } catch (error) {
      console.error('Failed to send message:', error);
      this.errorMessage = 'Failed to send message';
    }
  }

  private getCurrentUserId(): string {
    try {
      const token = localStorage.getItem('access_token');
      if (!token) return '';
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.sub || payload.userId || payload.nameid || '';
    } catch {
      return '';
    }
  }

  private getCurrentUserUsername(): string {
    try {
      const token = localStorage.getItem('access_token');
      if (!token) return 'You';
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.username || payload.unique_name || payload.preferred_username || 'You';
    } catch {
      return 'You';
    }
  }

  retryConnection() {
    this.ngOnInit();
  }

  ngOnDestroy() {
    if (this.connectionSubscription) this.connectionSubscription.unsubscribe();
    this.chatService.disconnect();
  }
}