import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkItem} from '../../../models/work-item';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { WorkItemType } from '../../../models/enums/work-item-type';
import { WorkItemStatus } from '../../../models/enums/work-item-status';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';

@Component({
  selector: 'app-work-item-form',
  templateUrl: './work-item-form.component.html',
  styleUrls: ['./work-item-form.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class WorkItemFormComponent {
  @Input() projectId: string = '';
  @Output() workItemCreated = new EventEmitter<WorkItem>();
  @Output() cancel = new EventEmitter<void>();

  workItemForm: FormGroup;
  workItemTypes = Object.values(WorkItemType);
  statuses = Object.values(WorkItemStatus);
  priorities = Object.values(WorkItemPriority);
  teamMembers: any[] = [];

  constructor(private fb: FormBuilder, private userService: UserService) {
    this.workItemForm = this.fb.group({
      type: [WorkItemType.Task, Validators.required],
      title: ['', Validators.required],
      description: [''],
      priority: [WorkItemPriority.Medium, Validators.required],
      status: [WorkItemStatus.ToDo, Validators.required],
      estimatedHours: [null, [Validators.min(0)]],
      dueDate: [null],
      labels: [''],
      assignedUserId: [null],
      storyPoints: [null, [Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.userService.getProjectTeam(this.projectId).subscribe({
      next: (members) => {
        this.teamMembers = members;
      },
      error: (err) => {
        console.error('Failed to load team members', err);
      }
    });
  }

  onSubmit(): void {
    if (this.workItemForm.invalid) return;

    const formValue = this.workItemForm.value;
    const newWorkItem: WorkItem = {
      ...formValue,
      id: this.generateId(),
      projectId: this.projectId,
      createdAt: new Date(),
      isBlocked: false,
      comments: [],
      attachments: [],
      history: [],
      subItems: [],
      completionPercentage: 0
    };

    this.workItemCreated.emit(newWorkItem);
  }

  onTypeChange(): void {
    const type = this.workItemForm.value.type;
    if (type === WorkItemType.Epic || type === WorkItemType.Story) {
      this.workItemForm.get('storyPoints')?.setValidators([Validators.min(1)]);
    } else {
      this.workItemForm.get('storyPoints')?.clearValidators();
    }
    this.workItemForm.get('storyPoints')?.updateValueAndValidity();
  }

  private generateId(): string {
    return Math.random().toString(36).substring(2, 9);
  }
}