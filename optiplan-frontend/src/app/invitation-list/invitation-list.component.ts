import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Invitation } from '../models/invitation';
import { InvitationStatus } from '../models/enums/invitation-status';
import { InvitationService } from '../services/invitation.service';
import { TeamRole } from '../models/enums/team-role';

@Component({
  selector: 'app-invitation-list',
  templateUrl: './invitation-list.component.html',
  styleUrls: ['./invitation-list.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class InvitationListComponent implements OnInit {
  InvitationStatus = InvitationStatus;
  TeamRole = TeamRole;

  invitations: Invitation[] = [];
  isLoading = false;
  activeTab: InvitationStatus = InvitationStatus.Pending;
  processingIds = new Set<string>(); // Track invitations being processed

  constructor(
    private invitationService: InvitationService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadInvitations();
  }

  loadInvitations(): void {
    this.isLoading = true;
    this.invitationService.GetAllInvitationForUser().subscribe({
      next: (invitations) => {
        this.invitations = invitations;
        this.isLoading = false;
      },
      error: (err) => {
        this.toastr.error('Failed to load invitations', 'Error');
        this.isLoading = false;
        console.error('Error loading invitations:', err);
      }
    });
  }

  acceptInvitation(invitationId: string): void {
    this.processingIds.add(invitationId);
    this.invitationService.AccepteInitation(invitationId).subscribe({
      next: () => {
        this.toastr.success('Invitation accepted successfully');
        this.processingIds.delete(invitationId);
        this.updateInvitationStatus(invitationId, InvitationStatus.Accepted);
      },
      error: (err) => {
        this.toastr.error('Failed to accept invitation', 'Error');
        this.processingIds.delete(invitationId);
        console.error('Error accepting invitation:', err);
      }
    });
  }

  rejectInvitation(invitationId: string): void {
    this.processingIds.add(invitationId);
    this.invitationService.RejectInvitation(invitationId).subscribe({
      next: () => {
        this.toastr.success('Invitation rejected successfully');
        this.processingIds.delete(invitationId);
        this.updateInvitationStatus(invitationId, InvitationStatus.Rejected);
      },
      error: (err) => {
        this.toastr.error('Failed to reject invitation', 'Error');
        this.processingIds.delete(invitationId);
        console.error('Error rejecting invitation:', err);
      }
    });
  }

  private updateInvitationStatus(invitationId: string, status: InvitationStatus): void {
    const invitation = this.invitations.find(i => i.id === invitationId);
    if (invitation) {
      invitation.status = status;
      invitation.respondedAt = new Date().toISOString();
    }
  }

  get filteredInvitations(): Invitation[] {
    return this.invitations.filter(inv => inv.status === this.activeTab);
  }

  changeTab(tab: InvitationStatus): void {
    this.activeTab = tab;
  }

  getStatusBadgeClass(status: InvitationStatus): string {
    switch (status) {
      case InvitationStatus.Pending:
        return 'status-pending';
      case InvitationStatus.Accepted:
        return 'status-accepted';
      case InvitationStatus.Rejected:
        return 'status-rejected';
      default:
        return 'status-unknown';
    }
  }

  getRoleBadgeClass(role: TeamRole): string {
    switch (role) {
      case TeamRole.ProjectCreator:
        return 'role-project-creator';
      case TeamRole.ProjectManager:
        return 'role-project-manager';
      case TeamRole.TeamLeader:
        return 'role-team-leader';
      case TeamRole.TeamMember:
        return 'role-team-member';
      case TeamRole.Guest:
        return 'role-guest';
      default:
        return 'role-unknown';
    }
  }

  countInvitations(status: InvitationStatus): number {
    return this.invitations.filter(inv => inv.status === status).length;
  }

  isProcessing(invitationId: string): boolean {
    return this.processingIds.has(invitationId);
  }
}