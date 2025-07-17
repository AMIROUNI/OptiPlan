import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';

import { HttpHeaders } from '@angular/common/http';
import { User } from '../models/user';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { Role } from '../models/enums/role';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  userImg: string = '';
  isLoggedIn = false;
  userRole: Role = Role.User; // Default role
  username: string ="";
  currentUser: User | null = null;

  constructor(private router: Router, private authService: AuthService,private userService: UserService) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoggedIn = this.authService.isAuthenticated();
      }
    });
  }

  ngOnInit(): void {
    this.userRole = this.authService.getRole() ?? Role.User;
    this.isLoggedIn = this.authService.isAuthenticated();
    const token = this.authService.getToken();
    if (token) {
      const decoded: any = JSON.parse(atob(token.split('.')[1]));
      this.userImg = decoded.picture ;
    }
    this.userService.getCurrentUser().subscribe((user) => {
      this.currentUser = user;
      this.username = this.currentUser?.username || '';
    });

    
  }

  logout() {
    try {
      this.authService.logout();
      localStorage.removeItem('token');
      this.router.navigate(['/']);
    } catch (err) {
      console.error("Logout error", err);
    }
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
}
