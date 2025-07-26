import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { WorkItem } from '../../models/work-item';
import { WorkItemService } from '../../services/work-item.service';
import { WorkItemPriority } from '../../models/enums/work-item-priority';
import { WorkItemStatus } from '../../models/enums/work-item-status';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.css'],
  standalone: true,
  imports: [CommonModule, SidebarComponent]
})
export class ProjectDetailsComponent implements OnInit {
  projectId!: string;
  project!: Project;
  tasks: WorkItem[] = [];
  loading = true;
  error: string | null = null;
  activeTab: 'details' | 'tasks' = 'details';
  filteredTasks: WorkItem[] = [];
  taskstatusFilter: 'all' | 'todo' | 'inprogress' | 'done' | 'review' = 'all';

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private taskService: WorkItemService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('id')!;
    this.loadProjectDetails();
    this.loadProjectTasks();
  }

  loadProjectDetails(): void {
    this.projectService.getProject(this.projectId).subscribe({
      next: (response) => {
        this.project = response;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load project details';
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadProjectTasks(): void {
    this.taskService.getTasksForProject(this.projectId).subscribe({
      next: (response) => {
        this.tasks = response;
        this.filterTasks();
      },
      error: (err) => {
        this.error = 'Failed to load project tasks';
        console.error(err);
      }
    });
  }

  getTodoTasksCount(): number {
    return this.tasks.filter(t => t.status === WorkItemStatus.ToDo).length;
  }

  getInProgressTasksCount(): number {
    return this.tasks.filter(t => t.status === WorkItemStatus.InProgress).length;
  }

  getDoneTasksCount(): number {
    return this.tasks.filter(t => t.status === WorkItemStatus.Done).length;
  }

  getInReviewTasksCount(): number {
    return this.tasks.filter(t => t.status === WorkItemStatus.InReview).length;
  }

  getBlockedTasksCount(): number {
    return this.tasks.filter(t => t.isBlocked).length;
  }

  getProgressPercentage(): number {
    if (!this.tasks.length) return 0;
    const completedTasks = this.tasks.filter(task => task.status === WorkItemStatus.Done).length;
    return Math.round((completedTasks / this.tasks.length) * 100);
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }

  calculateDuration(startDate: string, endDate: string): number {
    if (!startDate || !endDate) return 0;
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diffTime = Math.abs(end.getTime() - start.getTime());
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  }

  switchTab(tab: 'details' | 'tasks'): void {
    this.activeTab = tab;
  }

  getStatusClass(status: WorkItemStatus): string {
    switch (status) {
      case WorkItemStatus.ToDo: return 'status-todo';
      case WorkItemStatus.InProgress: return 'status-in-progress';
      case WorkItemStatus.InReview: return 'status-in-review';
      case WorkItemStatus.Done: return 'status-done';
      case WorkItemStatus.Rejected: return 'status-rejected';
      case WorkItemStatus.Archived: return 'status-archived';
      default: return '';
    }
  }

  getPriorityClass(priority: WorkItemPriority): string {
    switch (priority) {
      case WorkItemPriority.Low: return 'priority-low';
      case WorkItemPriority.Medium: return 'priority-medium';
      case WorkItemPriority.High: return 'priority-high';
      case WorkItemPriority.Critical: return 'priority-critical';
      default: return '';
    }
  }

  filterTasks(status: 'all' | 'todo' | 'inprogress' | 'done' | 'review' = 'all'): void {
    this.taskstatusFilter = status;
    switch (status) {
      case 'todo':
        this.filteredTasks = this.tasks.filter(task => task.status === WorkItemStatus.ToDo);
        break;
      case 'inprogress':
        this.filteredTasks = this.tasks.filter(task => task.status === WorkItemStatus.InProgress);
        break;
      case 'done':
        this.filteredTasks = this.tasks.filter(task => task.status === WorkItemStatus.Done);
        break;
      case 'review':
        this.filteredTasks = this.tasks.filter(task => task.status === WorkItemStatus.InReview);
        break;
      default:
        this.filteredTasks = [...this.tasks];
    }
  }
}
