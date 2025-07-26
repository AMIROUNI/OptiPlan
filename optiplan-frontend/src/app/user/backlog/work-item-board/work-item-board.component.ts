import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { WorkItem } from '../../../models/work-item';
import { WorkItemType } from '../../../models/enums/work-item-type';
import { WorkItemStatus } from '../../../models/enums/work-item-status';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';
import { WorkItemService } from '../../../services/work-item.service';
import { ProjectService } from '../../../services/project.service';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-work-item-board',
  templateUrl: './work-item-board.component.html',
  styleUrls: ['./work-item-board.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
    
  ],
  standalone: true
})
export class WorkItemBoardComponent implements OnInit {
  @ViewChild('createModal') createModal: any;
  @ViewChild('detailModal') detailModal: any;

  activeView: 'board' | 'backlog' | 'reports' = 'board';
  viewMode: 'list' | 'board' = 'board';
  searchQuery: string = '';
  
  workItems: WorkItem[] = [];
  filteredWorkItems: WorkItem[] = [];
  selectedWorkItem: WorkItem | null = null;
  project: any = null;
  teamMembers: any[] = [];
  
  workItemForm: FormGroup;
  
  workItemTypes = Object.values(WorkItemType);
  statuses = Object.values(WorkItemStatus);
  priorities = Object.values(WorkItemPriority);
  
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    private workItemService: WorkItemService,
    private projectService: ProjectService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {
    this.workItemForm = this.fb.group({
      type: [WorkItemType.Task, Validators.required],
      title: ['', Validators.required],
      description: [''],
      priority: [WorkItemPriority.Medium, Validators.required],
      status: [WorkItemStatus.ToDo, Validators.required],
      estimatedHours: [null],
      dueDate: [null],
      labels: [''],
      assignedUserId: [null],
      storyPoints: [null]
    });
  }

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
      error: (err) => {
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
      error: (err) => {
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

  getWorkItemsByStatus(status: WorkItemStatus): WorkItem[] {
    return this.workItems.filter(item => item.status === status);
  }

  getPriorityIcon(priority: WorkItemPriority): string {
    switch(priority) {
      case WorkItemPriority.Low: return 'bi bi-arrow-down';
      case WorkItemPriority.Medium: return 'bi bi-dash';
      case WorkItemPriority.High: return 'bi bi-arrow-up';
      case WorkItemPriority.Critical: return 'bi bi-exclamation-triangle';
      default: return 'bi bi-dash';
    }
  }

  openCreateModal(): void {
    this.workItemForm.reset({
      type: WorkItemType.Task,
      priority: WorkItemPriority.Medium,
      status: WorkItemStatus.ToDo
    });
    this.modalService.open(this.createModal, { size: 'lg' });
  }

  openDetailModal(item: WorkItem): void {
    this.selectedWorkItem = item;
    this.modalService.open(this.detailModal, { size: 'xl' });
  }

  createWorkItem(): void {
    if (this.workItemForm.invalid) return;

    const formData = this.workItemForm.value;
    const newItem: Partial<WorkItem> = {
      ...formData,
      projectId: this.project.id,
      reporterId: 'current-user-id' // Replace with actual user ID
    };

    this.workItemService.createWorkItem2(newItem).subscribe({
      next: (createdItem) => {
        this.workItems.push(createdItem);
        this.filteredWorkItems = [...this.workItems];
        this.modalService.dismissAll();
      },
      error: (err) => {
        this.handleError('Failed to create work item');
      }
    });
  }

  onTypeChange(): void {
    // You can add logic here to adjust form based on type selection
    const type = this.workItemForm.value.type;
    if (type === WorkItemType.Epic || type === WorkItemType.Story) {
      this.workItemForm.get('storyPoints')?.setValidators([Validators.min(1)]);
    } else {
      this.workItemForm.get('storyPoints')?.clearValidators();
    }
    this.workItemForm.get('storyPoints')?.updateValueAndValidity();
  }

  onDragStart(item: WorkItem): void {
    // You can store the dragged item or its ID
    // This is handled by HTML5 drag and drop API
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    // Add visual feedback if needed
  }

  onDrop(newStatus: WorkItemStatus, event: DragEvent): void {
    event.preventDefault();
    const itemId = event.dataTransfer?.getData('text/plain');
    if (!itemId) return;

    const item = this.workItems.find(i => i.id === itemId);
    if (item && item.status !== newStatus) {
      const updatedItem = { ...item, status: newStatus };
      this.workItemService.updateWorkItem(updatedItem).subscribe({
        next: () => {
          item.status = newStatus;
          // Update the UI accordingly
        },
        error: (err) => {
          this.handleError('Failed to update work item status');
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