import { Component, OnInit } from '@angular/core';
import { SprintService } from '../../services/sprint.service';
import { Sprint } from '../../models/sprint.model';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgClass, NgFor, NgIf } from '@angular/common';
import { ProjectTask } from '../../models/projectTask';

@Component({
  selector: 'app-backlog',
  templateUrl: './backlog.component.html',
  styleUrls: ['./backlog.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgClass,
    NgFor,
    NgIf,
    
  ]
  ,
})
export class BacklogComponent implements OnInit {
 
  sprints: Sprint[] = [];
  activeSprint: Sprint | null = null;
  backlogTasks: ProjectTask[] = [];
  loading = true;
  error = '';
  
  // Forms
  sprintForm: FormGroup;
  taskForm: FormGroup;
  
  // UI State
  viewMode: 'list' | 'board' = 'board';
  activeTab: 'backlog' | 'sprints' = 'backlog';
  selectedSprintId: string | null = null;

  constructor(
    private sprintService: SprintService,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private datePipe: DatePipe
  ) {
    this.sprintForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', Validators.maxLength(200)],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });

    this.taskForm = this.fb.group({
      Title: ['', [Validators.required, Validators.maxLength(100)]],
      Description: ['', Validators.maxLength(500)],
      Type: ['Task', Validators.required],
      Priority: ['Medium', Validators.required],
      Status: ['ToDo', Validators.required],
      EstimatedHours: [1, [Validators.required, Validators.min(1)]],
      DueDate: ['']
    });
  }

  ngOnInit(): void {
    this.loadSprints();
    // Simulate backlog tasks - in real app you'd get these from a service
    this.backlogTasks = this.generateMockBacklogTasks();
  }

  loadSprints(): void {
    this.loading = true;
    // In a real app, you'd get the project ID from route or state
    const projectId = 'current-project-id';
    
    this.sprintService.GetSprintsForProject(projectId).subscribe({
      next: (sprints) => {
        this.sprints = sprints;
        this.activeSprint = sprints.find(s => !s.isCompleted) || null;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load sprints';
        this.loading = false;
        this.toastr.error('Failed to load sprints', 'Error');
      }
    });
  }

  getSprintCompletionPercentage(sprint?: Sprint): number {
    const targetSprint = sprint || this.activeSprint;
    if (!targetSprint) return 0;
    
    // In a real app, you would calculate this based on actual tasks completion
    const totalTasks = 10; // Example value
    const completedTasks = 4; // Example value
    return Math.round((completedTasks / totalTasks) * 100);
  }

  createSprint(): void {
    if (this.sprintForm.invalid) return;
    
    const sprintData = this.sprintForm.value;
    const projectId = 'current-project-id'; // Get from route/state in real app
    
    this.sprintService.CreateSprint(projectId, sprintData).subscribe({
      next: (newSprint) => {
        this.sprints.push(newSprint);
        this.sprintForm.reset();
        this.modalService.dismissAll();
        this.toastr.success('Sprint created successfully!', 'Success');
      },
      error: (err) => {
        this.toastr.error('Failed to create sprint', 'Error');
      }
    });
  }

  openSprintModal(content: any): void {
    this.modalService.open(content, { size: 'lg', centered: true });
  }

  openTaskModal(content: any): void {
    this.modalService.open(content, { size: 'lg', centered: true });
  }

  selectSprint(sprintId: string): void {
    this.selectedSprintId = sprintId;
  }

  getFilteredTasks(status: string): ProjectTask[] {
    return this.backlogTasks.filter(t => t.status === status);
  }

  // Mock data generation for demo purposes
  private generateMockBacklogTasks(): ProjectTask[] {
    return [
      {
        id: 'task-1',
        title: 'Implement user authentication',
        description: 'Set up JWT authentication for the API',
        status: 'ToDo',
        type: 'Feature',
        priority: 'High',
        projectId: 'project-1',
        createdAt: new Date(),
        updatedAt: new Date(),
        estimatedHours: 8,
        completionPercentage: 0,
        isBlocked: false,
        isCompleted: false
      },
      {
        id: 'task-2',
        title: 'Design dashboard UI',
        description: 'Create mockups for the main dashboard',
        status: 'ToDo',
        type: 'Task',
        priority: 'Medium',
        projectId: 'project-1',
        createdAt: new Date(),
        updatedAt: new Date(),
        estimatedHours: 5,
        completionPercentage: 0,
        isBlocked: false,
        isCompleted: false
      },
     
    ];
  }
  hasCompletedSprints(): boolean {
  return this.sprints?.filter(s => s.isCompleted)?.length > 0;
}

getCompletedSprints(): Sprint[] {
  return this.sprints?.filter(s => s.isCompleted) || [];
}
}