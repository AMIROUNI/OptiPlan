import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectTask } from '../../../models/projectTask';
import { Sprint } from '../../../models/sprint.model';
import { TaskService } from '../../../services/task.service';
import { SprintService } from '../../../services/sprint.service';
import { CommonModule } from '@angular/common';
import { AddTaskComponent } from '../add-task/add-task.component';
import { AddSprintComponent } from '../add-sprint/add-sprint.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-backlog-management',
  standalone: true,
  imports: [CommonModule, AddTaskComponent, AddSprintComponent],
  templateUrl: './backlog-management.component.html',
  styleUrls: ['./backlog-management.component.css']
})
export class BacklogManagementComponent implements OnInit {
  @ViewChild('addTaskModal') addTaskModal: any;
  @ViewChild('addSprintModal') addSprintModal: any;

  backlogTasks: ProjectTask[] = [];
  sprints: Sprint[] = [];
  activeSprint: Sprint | null = null;
  loading = false;
  error = '';
  activeTab: 'backlog' | 'sprints' = 'backlog';
  viewMode: 'list' | 'board' = 'board';
  projectId: string = '';
  dragTask: ProjectTask | null = null;

  constructor(
    private taskService: TaskService,
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

    this.taskService.getTasksForProject(this.projectId).subscribe({
      next: (tasks) => {

        console.log('Tasks loaded:', tasks);
        this.backlogTasks = tasks;
        this.loadSprints();
      },
      error: (err) => {
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
      error: (err) => {
        this.handleError('Failed to load sprints. Please try again.');
      }
    });
  }

  openTaskModal(): void {
    this.modalService.open(this.addTaskModal, { size: 'lg', backdrop: 'static', ariaLabelledBy: 'addTaskModalLabel' });
  }

  openSprintModal(): void {
    this.modalService.open(this.addSprintModal, { size: 'lg', backdrop: 'static', ariaLabelledBy: 'addSprintModalLabel' });
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

  private handleError(message: string): void {
    this.error = message;
    this.loading = false;
    console.error(message);
  }

  getFilteredTasks(status: string): ProjectTask[] {
    return this.backlogTasks.filter(t => t.status === status);
  }

  getSprintCompletionPercentage(sprint?: Sprint): number {
    const targetSprint = sprint || this.activeSprint;
    if (!targetSprint) return 0;
    
    const totalTasks = this.backlogTasks.filter(t => t.sprintId === targetSprint.id).length;
    const completedTasks = this.backlogTasks.filter(t => 
      t.sprintId === targetSprint.id && t.status === 'Done'
    ).length;
    
    return totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;
  }

  hasCompletedSprints(): boolean {
    return this.sprints.filter(s => s.isCompleted).length > 0;
  }

  getCompletedSprints(): Sprint[] {
    return this.sprints.filter(s => s.isCompleted);
  }

  onDragStart(task: ProjectTask): void {
    this.dragTask = task;
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  onDrop(status: string, event: DragEvent): void {
    event.preventDefault();
    if (this.dragTask && this.dragTask.status !== status) {
      const updatedTask = { ...this.dragTask, Status: status };
      this.taskService.updateTask(updatedTask,this.projectId).subscribe({
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
}