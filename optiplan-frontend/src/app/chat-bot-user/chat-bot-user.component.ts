import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';

import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { ChatHubService } from '../services/chat-hub.service';
import { ChatBotService } from '../services/chat-bot.service';


interface Message {
  content: string;
  isUser: boolean;
  timestamp: Date;
}

interface Conversation {
  id: string;
  title: string;
  createdAt: Date;
}

@Component({
  selector: 'app-chat-bot-user',
  templateUrl: './chat-bot-user.component.html',
  styleUrls: ['./chat-bot-user.component.css'],
  imports:[CommonModule,ReactiveFormsModule],
  encapsulation: ViewEncapsulation.None
})
export class ChatBotUserComponent implements OnInit, OnDestroy {
  chatForm: FormGroup;
  messages: Message[] = [];
  conversations: Conversation[] = [];
  currentConversationId: string | null = null;
  isLoading = false;
  userId: string | null = null;
  private subscriptions: Subscription[] = [];

  constructor(
    private chatService: ChatBotService,
    private fb: FormBuilder,
    private authService: AuthService,

  ) {
    this.chatForm = this.fb.group({
      message: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.userId = this.authService.getCurrentUser().id;
  
    const savedConversationId = localStorage.getItem('currentConversationId');
    if (savedConversationId) {
      this.currentConversationId = savedConversationId;
      this.loadConversation(savedConversationId);
    }
  
    if (this.userId) {
      this.loadConversations();
    }
  }
  

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadConversations(): void {
    this.subscriptions.push(
      this.chatService.GetConversationsByUserId(this.userId!).subscribe({
        next: (conversations: any) => {
          this.conversations = conversations.map((conv: any) => ({
            id: conv.id,
            title: conv.title,
            createdAt: new Date(conv.createdAt)
          }));
        },
        error: (err) => console.error('Error loading conversations:', err)
      })
    );
  }

  loadConversation(conversationId: string): void {
    this.currentConversationId = conversationId;
    localStorage.setItem('currentConversationId', conversationId); // Sauvegarder dans le localStorage
    this.messages = [];
  
    this.subscriptions.push(
      this.chatService.GetConversation(conversationId).subscribe({
        next: (conversation: any) => {
          this.messages = conversation.messages
            .map((msg: any): Message => ({
              content: msg.content,
              isUser: msg.role === 'user',
              timestamp: new Date(msg.sentAt)
            }))
            .sort((a: Message, b: Message) => a.timestamp.getTime() - b.timestamp.getTime());
  
          this.scrollToBottom();
        },
        error: (err) => console.error('Error loading conversation:', err)
      })
    );
  }
  
  
  
  startNewConversation(): void {
    this.subscriptions.push(
      this.chatService.NewConversation(null).subscribe({
        next: (response: any) => {
          this.currentConversationId = response.conversationId;
          this.messages = [];
          this.loadConversations();
        },
        error: (err) => console.error('Error creating new conversation:', err)
      })
    );
  }

  deleteConversation(conversationId: string): void {
    this.subscriptions.push(
      this.chatService.DeleteConversation(conversationId).subscribe({
        next: () => {
          if (this.currentConversationId === conversationId) {
            this.currentConversationId = null;
            localStorage.removeItem('currentConversationId');
            this.messages = [];
          }
          this.loadConversations();
        },
        error: (err) => console.error('Error deleting conversation:', err)
      })
    );
  }
  

  onSubmit(): void {
    if (this.chatForm.invalid) return;

    const message = this.chatForm.get('message')?.value;
    this.addMessage(message, true);
    this.chatForm.reset();

    if (!this.currentConversationId) {
      // Create new conversation if none exists
      this.subscriptions.push(
        this.chatService.NewConversation(message.substring(0, 15)).subscribe({
          next: (response: any) => {
            this.currentConversationId = response.conversationId;
            this.sendMessageToBot(message);
            this.loadConversations();
          },
          error: (err) => console.error('Error creating conversation:', err)
        })
      );
    } else {
      this.sendMessageToBot(message);
    }
  }

  private sendMessageToBot(message: string): void {
    this.isLoading = true;
  
    // Utiliser la valeur actuelle de conversationId (stockée aussi dans localStorage)
    const conversationId = localStorage.getItem('currentConversationId');
  
    this.subscriptions.push(
      this.chatService.AskBot(message, conversationId).subscribe({
        next: (response: any) => {
          // Sauvegarder conversationId si renvoyé par le backend
          if (response.conversationId) {
            this.currentConversationId = response.conversationId;
            localStorage.setItem('currentConversationId', response.conversationId);
          }
  
          this.addMessage(response.response, false);
          this.isLoading = false;
          this.scrollToBottom();
        },
        error: (err: any) => {
          console.error('Error getting bot response:', err);
          this.isLoading = false;
        }
      })
    );
  }
  

  private addMessage(content: string, isUser: boolean): void {
    this.messages.push({
      content,
      isUser,
      timestamp: new Date()
    });
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      const chatContainer = document.querySelector('.chat-messages');
      if (chatContainer) {
        chatContainer.scrollTop = chatContainer.scrollHeight;
      }
    }, 100);
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}