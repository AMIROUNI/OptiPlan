import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-welcome-card',
  templateUrl: './welcome-card.component.html',
  styleUrls: ['./welcome-card.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class WelcomeCardComponent implements OnInit {
  today: Date = new Date();
  username: string = '';
  greeting: string = '';
  todaysJobs: number = 0;
  newUsersToday: number = 0;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.setGreeting();
    this.userService.getCurrentUser().subscribe({
      next: (data: User) => {
        this.username = data.username || 'Admin';
        console.log("Admin username:", this.username);
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
        this.username = 'Admin'; // Fallback
      }
    });

    // Mock data - replace with actual API calls
    this.todaysJobs = 24;
    this.newUsersToday = 8;
  }

  private setGreeting(): void {
    const hour = this.today.getHours();
    if (hour < 12) {
      this.greeting = 'Good morning';
    } else if (hour < 18) {
      this.greeting = 'Good afternoon';
    } else {
      this.greeting = 'Good evening';
    }
  }
}