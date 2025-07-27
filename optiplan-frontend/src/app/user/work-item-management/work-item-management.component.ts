import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NavbarComponent } from './navbar/navbar.component';
import { BoardViewComponent } from './board-view/board-view.component';
import { ReportsViewComponent } from './reports-view/reports-view.component';
import { BacklogViewComponent } from './backlog-view/backlog-view.component';
import { WorkItem } from '../../models/work-item';
import { Project } from '../../models/project';
import { WorkItemType } from '../../models/enums/work-item-type';
import { WorkItemStatus } from '../../models/enums/work-item-status';
import { WorkItemPriority } from '../../models/enums/work-item-priority';
import { WorkItemService } from '../../services/work-item.service';
import { ProjectService } from '../../services/project.service';
import { UserService } from '../../services/user.service';
import { WorkItemFormComponent } from './work-item-form/work-item-form.component';
import { AddSprintComponent } from "./add-sprint/add-sprint.component";
import { SidebarComponent } from './sidebar/sidebar.component';


@Component({
  selector: 'app-work-item-management',
  templateUrl: './work-item-management.component.html',
  styleUrls: ['./work-item-management.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    SidebarComponent,
    NavbarComponent,
    BoardViewComponent,
    BacklogViewComponent,
    ReportsViewComponent,
    WorkItemFormComponent,
    AddSprintComponent
  ]
})
export class WorkItemManagementComponent implements OnInit {
  activeView: 'board' | 'backlog' | 'reports' = 'board';
  workItems: WorkItem[] = [];
  filteredWorkItems: WorkItem[] = [];
  project: Project | null = null;
  teamMembers: any[] = [];
  loading = false;
  error: string | null = null;
  selectedWorkItem: WorkItem | null = null;
  showCreateModal = false;
  showCreateSprintModal = false;

  constructor(
    private route: ActivatedRoute,
    private workItemService: WorkItemService,
    private projectService: ProjectService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    const projectId = this.route.snapshot.params['id'];

    this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.loadWorkItems(projectId);
        this.loadTeamMembers(projectId);
      },
      error: () => {
        this.handleError('Failed to load project');
      }
    });
  }

  loadWorkItems(projectId: string): void {
    this.workItemService.getWorkItemsByProject(projectId).subscribe({
      next: (items) => {
        this.workItems = items;
        this.filteredWorkItems = [...items];
        this.loading = false;
      },
      error: () => {
        this.handleError('Failed to load work items');
      }
    });
  }

  loadTeamMembers(projectId: string): void {
    this.userService.getProjectTeam(projectId).subscribe({
      next: (members) => {
        this.teamMembers = members;
      },
      error: (err) => {
        console.error('Failed to load team members', err);
      }
    });
  }

  onViewChange(view: 'board' | 'backlog' | 'reports'): void {
    this.activeView = view;
  }

  onWorkItemSelected(item: WorkItem): void {
    this.selectedWorkItem = item;
  }

  openCreateModal(): void {
    this.showCreateModal = true;
  }

  openCreateSprintModal(): void {
    this.showCreateSprintModal = true;
  }

  closeCreateSprintModal(): void {
    this.showCreateSprintModal = false;
  }

  onSprintCreated(success: boolean): void {
    if (success) {
      this.closeCreateSprintModal();
      // Optionally reload sprints or work items if needed
    }
  }

  onWorkItemCreated(item: WorkItem): void {
    this.workItems.push(item);
    this.filteredWorkItems = [...this.workItems];
    this.showCreateModal = false;
  }

  private handleError(message: string): void {
    this.error = message;
    this.loading = false;
    console.error(message);
  }

  getPriorityIcon(priority: WorkItemPriority): string {
    switch (priority) {
      case WorkItemPriority.High: return 'bi bi-arrow-up';
      case WorkItemPriority.Medium: return 'bi bi-arrow-right';
      case WorkItemPriority.Low: return 'bi bi-arrow-down';
      default: return '';
    }
  }
}