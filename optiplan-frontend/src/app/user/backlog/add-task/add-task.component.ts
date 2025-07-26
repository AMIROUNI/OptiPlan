import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { ProjectTaskDto } from '../../../models/dto/projectTask.dto';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { WorkItem } from '../../../models/work-item';
import { WorkItemService } from '../../../services/work-item.service';

@Component({
  selector: 'app-add-task',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.css']
})
export class AddTaskComponent {
  @Input() projectId: string = '';
  @Output() taskCreated = new EventEmitter<boolean>();
  @Output() cancel = new EventEmitter<boolean>();

  taskForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  userId: string = '';

  constructor(
    private fb: FormBuilder,
    private workItemService: WorkItemService,
    private authService: AuthService
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      type: ['Task', [Validators.required]],
      priority: ['Medium', [Validators.required]],
      status: ['ToDo', [Validators.required]],
      estimatedHours: [1, [Validators.required, Validators.min(1)]],
      dueDate: [''],
      storyPoints: [null, [Validators.min(1)]],
      labels: ['']
    });

    this.userId = this.authService.getCurrentUser()?.id || '';
    console.log('Current User ID:', this.userId);
  }

  onSubmit(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const taskData: ProjectTaskDto = {
      title: this.taskForm.value.title,
      description: this.taskForm.value.description,
      type: this.taskForm.value.type,
      priority: this.taskForm.value.priority,
      status: this.taskForm.value.status,
      estimatedHours: this.taskForm.value.estimatedHours,
      dueDate: this.taskForm.value.dueDate ? new Date(this.taskForm.value.dueDate) : undefined,
      storyPoints: this.taskForm.value.storyPoints,
      labels: this.taskForm.value.labels,
      completionPercentage: this.taskForm.value.status === 'Done' ? 100 : 0,
      projectId: this.projectId,
      reporterId: this.userId
    };

    this.workItemService.createWorkItem(this.projectId, taskData).subscribe({
     
      next: () => {
         console.log(taskData);
        console.log('Task created successfully');
        this.isLoading = false;
        this.taskCreated.emit(true); // Emit true to close modal
        this.resetForm();
      },
      error: (err) => {
         console.log(taskData);
        this.isLoading = false;
        this.errorMessage = 'Failed to create task. Please try again.';
        console.error('Error creating task', err);
        this.taskCreated.emit(false); // Emit false to keep modal open
      }
    });
  }

  onCancel(): void {
    this.cancel.emit(true);
  }

  private resetForm(): void {
    this.taskForm.reset({
      type: 'Task',
      priority: 'Medium',
      status: 'ToDo',
      estimatedHours: 1
    });
  }
}