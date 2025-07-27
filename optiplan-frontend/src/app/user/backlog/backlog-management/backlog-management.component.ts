import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkItem } from '../../../models/work-item';
import { Sprint } from '../../../models/sprint.model';
import { SprintService } from '../../../services/sprint.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { AddTaskComponent } from "../add-task/add-task.component";
import { AddSprintComponent } from "../../work-item-management/add-sprint/add-sprint.component";
import { WorkItemService } from '../../../services/work-item.service';
import { WorkItemStatus } from '../../../models/enums/work-item-status';


@Component({
  selector: 'app-backlog-management',
  templateUrl: './backlog-management.component.html',
  styleUrls: ['./backlog-management.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    AddTaskComponent,
    AddSprintComponent
  ]
})
export class BacklogManagementComponent implements OnInit {
  @ViewChild('addTaskModal') addTaskModal: any;
  @ViewChild('addSprintModal') addSprintModal: any;

  backlogTasks: WorkItem[] = [];
  sprints: Sprint[] = [];
  activeSprint: Sprint | null = null;
  loading = false;
  error = '';
  activeTab: 'backlog' | 'sprints' = 'backlog';
  viewMode: 'list' | 'board' = 'list';
  projectId: string = '';
  dragTask: WorkItem | null = null;

  constructor(
    private workItemService: WorkItemService,
    private sprintService: SprintService,
    private route: ActivatedRoute,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    if (!this.projectId) {
      this.error = 'Invalid project ID';
      return;
    }
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    this.workItemService.getWorkItemsByProject(this.projectId).subscribe({
      next: (tasks) => {
        this.backlogTasks = tasks;
        this.loadSprints();
      },
      error: () => {
        this.handleError('Failed to load tasks. Please try again.');
      }
    });
  }

  loadSprints(): void {
    this.sprintService.GetSprintsForProject(this.projectId).subscribe({
      next: (sprints) => {
        this.sprints = sprints;
        this.activeSprint = sprints.find(s => !s.isCompleted) || null;
        this.loading = false;
      },
      error: () => {
        this.handleError('Failed to load sprints. Please try again.');
      }
    });
  }

  openTaskModal(): void {
    this.modalService.open(this.addTaskModal, { size: 'lg', backdrop: 'static' });
  }

  openSprintModal(): void {
    this.modalService.open(this.addSprintModal, { size: 'lg', backdrop: 'static' });
  }

  handleTaskCreated(shouldClose: boolean): void {
    if (shouldClose) {
      this.modalService.dismissAll();
      this.loadData();
    }
  }

  handleSprintCreated(shouldClose: boolean): void {
    if (shouldClose) {
      this.modalService.dismissAll();
      this.loadData();
    }
  }

  getTypeIcon(type: string): string {
    switch (type.toLowerCase()) {
      case 'task': return 'bi bi-check-circle';
      case 'bug': return 'bi bi-bug';
      case 'feature': return 'bi bi-star';
      case 'improvement': return 'bi bi-lightbulb';
      case 'epic': return 'bi bi-collection';
      default: return 'bi bi-card-checklist';
    }
  }

  getPriorityIcon(priority: string): string {
    switch (priority.toLowerCase()) {
      case 'low': return 'bi bi-arrow-down';
      case 'medium': return 'bi bi-dash';
      case 'high': return 'bi bi-arrow-up';
      case 'critical': return 'bi bi-exclamation-triangle';
      default: return 'bi bi-dash';
    }
  }

  getFilteredTasks(status: string): WorkItem[] {
    return this.backlogTasks.filter(t => t.status === status);
  }

  getSprintCompletionPercentage(sprint?: Sprint): number {
    const targetSprint = sprint || this.activeSprint;
    if (!targetSprint) return 0;

    const totalTasks = this.backlogTasks.filter(t => t.sprintId === targetSprint.id).length;
    const completedTasks = this.backlogTasks.filter(t =>
      t.sprintId === targetSprint.id && t.status === WorkItemStatus.Done
    ).length;

    return totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;
  }

  hasCompletedSprints(): boolean {
    return this.sprints.some(s => s.isCompleted);
  }

  getCompletedSprints(): Sprint[] {
    return this.sprints.filter(s => s.isCompleted);
  }

  getFutureSprints(): Sprint[] {
    return this.sprints.filter(s => !s.isCompleted && s !== this.activeSprint);
  }

  onDragStart(task: WorkItem): void {
    this.dragTask = task;
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  onDrop(status: string, event: DragEvent): void {
    event.preventDefault();

    const newStatus = status as unknown as WorkItemStatus;

    if (this.dragTask && this.dragTask.status !== newStatus) {
      const updatedTask: WorkItem = {
        ...this.dragTask,
        status: newStatus
      };

      this.workItemService.updateTask(updatedTask, this.projectId).subscribe({
        next: () => {
          this.loadData();
          this.dragTask = null;
        },
        error: () => {
          this.handleError('Failed to update task status.');
        }
      });
    }
  }

  private handleError(message: string): void {
    this.error = message;
    this.loading = false;
    console.error(message);
  }
}
