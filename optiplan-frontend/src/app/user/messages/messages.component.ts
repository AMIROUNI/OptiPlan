import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatApiService } from '../../services/chat-api.service';
import { User } from '../../models/user';
import { ChatBoxComponent } from '../../chat-box/chat-box.component';
import { animate, style, transition, trigger, keyframes, query, stagger } from '@angular/animations';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, FormsModule, ChatBoxComponent],
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({ opacity: 0, transform: 'translateY(20px)' })),
      ]),
    ]),
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('400ms ease-in', style({ opacity: 1 })),
      ]),
    ]),
    trigger('searchAnimation', [
      transition(':enter', [
        style({ opacity: 0, width: '0%' }),
        animate('500ms ease-out', style({ opacity: 1, width: '100%' })),
      ]),
    ]),
    trigger('userItemAnimation', [
      transition('void => in', [
        animate('400ms {{delay}} ease-in', keyframes([
          style({ opacity: 0, transform: 'translateX(-50px)', offset: 0 }),
          style({ opacity: 0.5, transform: 'translateX(-20px)', offset: 0.5 }),
          style({ opacity: 1, transform: 'translateX(0)', offset: 1.0 }),
        ])),
      ]),
    ]),
  ],
})
export class MessagesComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  showChat: boolean = false;
  selectedUser: User | null = null;
  selectedUserUsername: string = '';
  selectedUserId: string = '';
  loading: boolean = false;
  error: string | null = null;

  constructor(private chatApiService: ChatApiService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.error = null;
    this.chatApiService.getUsersIHaveChatWithIt().subscribe({
      next: (res: User[]) => {
        this.users = res.map(user => ({ ...user, online: Math.random() > 0.5, lastMessagePreview: 'Hey, how are you?' })); // Mock data
        this.filteredUsers = [...this.users];
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Error loading users:', err);
        this.error = 'Failed to load users. Please try again.';
        this.loading = false;
      }
    });
  }

  filterUsers(event: Event): void {
    const searchTerm = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredUsers = this.users.filter(user => user.username.toLowerCase().includes(searchTerm));
  }

  showChatBox(user: User): void {
    this.selectedUser = user;
    this.selectedUserUsername = user.username;
    this.selectedUserId = user.id;
    this.showChat = true;
  }

  closeChat(): void {
    this.showChat = false;
    this.selectedUser = null;
    this.selectedUserUsername = '';
    this.selectedUserId = '';
  }
}