import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { Role } from '../models/enums/role';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  userImg: string | null = null;
  isLoggedIn = false;
  userRole: Role = Role.User;
  username: string = "";
  currentUser: User | null = null;
  defaultAvatar = 'assets/images/default-profile.png';
  apiUrl = environment.apiUrl;

  constructor(
    private router: Router, 
    private authService: AuthService,
    private userService: UserService
  ) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkAuthStatus();
      }
    });
  }

  ngOnInit(): void {
    this.checkAuthStatus();
    this.loadUserData();
  }

  private checkAuthStatus(): void {
    this.isLoggedIn = this.authService.isAuthenticated();
    if (this.isLoggedIn) {
      this.userRole = this.authService.getRole() ?? Role.User;
      this.extractAvatarFromToken();
    }
  }

  private extractAvatarFromToken(): void {
    const token = this.authService.getToken();
    if (token) {
      try {
        const decoded: any = JSON.parse(atob(token.split('.')[1]));
        this.userImg = decoded.AvatarUrl || null;
      } catch (error) {
        console.error('Error decoding token:', error);
        this.userImg = null;
      }
    }
  }

  private loadUserData(): void {
    if (this.isLoggedIn) {
      this.userService.getCurrentUser().subscribe({
        next: (user) => {
          this.currentUser = user;
          this.username = user?.username || '';
          // If avatar wasn't in token but is in user data
          if (!this.userImg && user?.avatarUrl) {
            this.userImg = user.avatarUrl;
          }
        },
        error: (err) => {
          console.error('Error loading user data:', err);
        }
      });
    }
  }

  logout(): void {
    try {
      this.authService.logout();
      this.resetUserData();
      this.router.navigate(['/']);
    } catch (err) {
      console.error("Logout error", err);
    }
  }

  private resetUserData(): void {
    this.isLoggedIn = false;
    this.userRole = Role.User;
    this.username = "";
    this.currentUser = null;
    this.userImg = null;
  }

  isAdmin(): boolean {
    return this.userRole === Role.Admin;
  }

  isUser(): boolean {
    return this.userRole === Role.User;
  }

  isModerator(): boolean {
    return this.userRole === Role.Moderator;
  }

  getAvatarUrl(): string {
    if (!this.userImg) return this.defaultAvatar;
    return this.userImg.startsWith('http') ? this.userImg : `${this.apiUrl}/${this.userImg}`;
  }
}