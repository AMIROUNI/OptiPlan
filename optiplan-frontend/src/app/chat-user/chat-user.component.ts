import { Component, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../models/user';


@Component({
  selector: 'app-chat-user',
  templateUrl: './chat-user.component.html',
  styleUrls: ['./chat-user.component.css']
})
export class ChatUserComponent {
  @Input() user!: User;
  @Output() selectUser = new EventEmitter<User>();

  onSelect() {
    this.selectUser.emit(this.user);
  }
}