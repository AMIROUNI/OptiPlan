import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  encapsulation: ViewEncapsulation.None
})
export class UserSearchComponent implements OnInit {
  allUsers: User[] = [];         // كل الـ Users ينزلو مرة وحدة
  filteredUsers: User[] = [];    // النتائج بعد البحث
  query: string = '';
  isLoading: boolean = false;
  noResults: boolean = false;

  private searchSubject = new Subject<string>();

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    // نعمل تحميل للـ Users مرة وحدة
    this.loadUsers();

    // البحث بالـ debounce
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(query => {
      this.applyFilter(query);
    });
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getAllUserNotAdmis().subscribe(
      (res: User[]) => {
        this.allUsers = res;
        this.filteredUsers = res; // في البداية نظهرهم كاملين
        this.isLoading = false;
      },
      err => {
        console.error('Error while loading users:', err);
        this.isLoading = false;
      }
    );
  }

  searchUsers(): void {
    this.searchSubject.next(this.query);
  }

  private applyFilter(query: string): void {
    if (!query.trim()) {
      this.filteredUsers = this.allUsers;
      this.noResults = false;
      return;
    }

    const lowerQuery = query.toLowerCase();

    this.filteredUsers = this.allUsers.filter(user =>
      (user.username && user.username.toLowerCase().includes(lowerQuery)) ||
      (user.fullName && user.fullName.toLowerCase().includes(lowerQuery)) ||
      (user.jobTitle && user.jobTitle.toLowerCase().includes(lowerQuery)) ||
      (user.email && user.email.toLowerCase().includes(lowerQuery)) ||
      (user.companyName && user.companyName.toLowerCase().includes(lowerQuery)) ||
      (user.department && user.department.toLowerCase().includes(lowerQuery)) ||
      (user.country && user.country.toLowerCase().includes(lowerQuery))
    );

    this.noResults = this.filteredUsers.length === 0;
  }
}
