import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { User } from '../../../models/user';
import { UserService } from '../../../services/user.service';
import { ProjectService } from '../../../services/project.service';
import { AuthService } from '../../../services/auth.service';
import { InvitationService } from '../../../services/invitation.service';
import { TeamRole } from '../../../models/enums/team-role';
import { Team } from '../../../models/team';

@Component({
  selector: 'app-invite-user',
  templateUrl: './invite-user.component.html',
  styleUrls: ['./invite-user.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class InviteUserComponent implements OnInit {
  public TeamRole = TeamRole;
  public selectedRole: TeamRole = TeamRole.TeamMember;
  email:string="" ;
  team?:Team;
  allUsers: User[] = [];
  filteredUsers: User[] = [];
  searchQuery = '';
  isLoading = false;
  isSending = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  selectedUsers: Set<string> = new Set();
  projectId: string;
  currentUser: any;
  private searchTerms = new Subject<string>();

  constructor(
    private userService: UserService,
    private projectService: ProjectService,
    private authService: AuthService,
    private invitationService: InvitationService,
    private route: ActivatedRoute
  ) {
    this.projectId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    console.log(this.currentUser)
    this.loadUsers();

    // Setup debounced search (300ms delay)
    this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(term => this.filterUsers(term));

    this.projectService.getTeamByProjectId(this.projectId).subscribe({
      next: (req) => {
        this.team = req;
      },
      error: (err) => {
        console.log(err);
      }
    });
    
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getAllUserNotAdmis().subscribe({
      next: (users: any) => {
        this.allUsers = users;
        this.filteredUsers = [...users];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load users. Please try again.';
        this.isLoading = false;
        console.error('Error loading users:', err);
      }
    });
  }

  search(term: string): void {
    this.searchTerms.next(term);
  }

  filterUsers(term: string): void {
    if (!term) {
      this.filteredUsers = [...this.allUsers];
      return;
    }

    const lowerTerm = term.toLowerCase();
    this.filteredUsers = this.allUsers.filter(user => 
      (user.fullName?.toLowerCase().includes(lowerTerm) ?? false) ||
      user.email.toLowerCase().includes(lowerTerm) ||
      (user.jobTitle?.toLowerCase().includes(lowerTerm) ?? false) ||
      (user.companyName?.toLowerCase().includes(lowerTerm) ?? false)
    );
  }

  toggleUserSelection(userId: string): void {
    if (this.selectedUsers.has(userId)) {
      this.selectedUsers.delete(userId);
    } else {
      this.selectedUsers.add(userId);
    }
  }

  getTheEmail(email:string){
    this.email=email;
  }

  sendInvitations(role: TeamRole): void {
    if (this.selectedUsers.size === 0) {
      this.errorMessage = 'Please select at least one user';
      return;
    }

    this.isSending = true;
    this.errorMessage = null;
    this.successMessage = null;

    const invitations = Array.from(this.selectedUsers).map(userId => ({
      inviteeId: userId,
      inviterId: this.currentUser.id,
      teamId: this.team?.id,
      email:this.email,
      role: role
    }));
    console.log(invitations)

    // Send all invitations
    Promise.all(invitations.map(inv => 
      this.invitationService.sendInvitation(inv).toPromise()
    )).then(results => {
      this.successMessage = `Successfully sent ${this.selectedUsers.size} invitation(s)`;
      this.selectedUsers.clear();
      this.isSending = false;
    }).catch(err => {
      this.errorMessage = err.error?.message || 'Failed to send some invitations. Please try again.';
      this.isSending = false;
      console.error('Error sending invitations:', err);
    });
  }

  getAvatarUrl(user: User): string {
    if (user.avatarUrl && user.avatarUrl.startsWith('http')) {
      return user.avatarUrl;
    }
    return 'assets/images/default-avatar.png';
  }

  trackByUserId(index: number, user: User): string {
    return user.id;
  }
}