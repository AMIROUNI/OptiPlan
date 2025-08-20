import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHubService } from '../services/chat-hub.service';
import { Message } from '../models/message';
import { Subscription } from 'rxjs';

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

  constructor(private chatService: ChatHubService) {}

  async ngOnInit() {
    console.log("this is the selectedId", this.selectedUserId);
    console.log("this is the selected username", this.selectedUserUsername);

    // Subscribe to connection status changes
    this.connectionSubscription = this.chatService.connectionStatus$.subscribe(
      status => {
        this.isConnected = status;
        console.log('Connection status changed:', status);
      }
    );

    const token = localStorage.getItem('token');
    if (!token) {
      this.errorMessage = 'No authentication token found';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    
    try {
      // Wait for connection to be established
      await this.chatService.connect(token);
      
      // ✅ CORRECT: Send the USERNAME to backend, not the ID
      await this.chatService.joinChat(this.selectedUserUsername);
      
      // Subscribe to incoming messages
      this.chatService.onMessageReceived(msg => {
        console.log('Message received:', msg);
        this.messages.push(msg);
      });
      
    } catch (error: any) {
      console.error('SignalR connection failed', error);
      this.errorMessage = error.message || 'Failed to connect to chat service';
    } finally {
      this.isLoading = false;
    }
  }

  async sendMessage() {
    if (!this.newMessage.trim()) return;
    
    if (!this.isConnected) {
      this.errorMessage = 'Not connected to chat service';
      console.error('Cannot send message: not connected');
      return;
    }

    let messageToSend: string = this.newMessage;

    try {
      // Create optimistic UI update
      const tempMessage: Message = {
        senderId:this.selectedUserId ,
        displaySender:'me',
        content: this.newMessage,
        sentAt: new Date()
      };
      this.messages.push(tempMessage);
      
      this.newMessage = '';
      this.errorMessage = '';
      
      // ✅ CORRECT: Send the USERNAME to backend, not the ID
      await this.chatService.sendMessage(this.selectedUserUsername, messageToSend);
      
      console.log('Message sent successfully');
      
    } catch (error: any) {
      console.error('Failed to send message:', error);
      this.errorMessage = error.message || 'Failed to send message';
      
      // Remove the optimistic update if sending failed
      if (this.messages.length > 0) {
        this.messages.pop();
      }
      this.newMessage = messageToSend;
    }
  }

  ngOnDestroy() {
    if (this.connectionSubscription) {
      this.connectionSubscription.unsubscribe();
    }
    // Clean up if needed
    this.chatService.disconnect();
  }
}