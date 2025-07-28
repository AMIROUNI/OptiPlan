import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkItem } from '../../../models/work-item';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { WorkItemType } from '../../../models/enums/work-item-type';
import { WorkItemStatus } from '../../../models/enums/work-item-status';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';
import { WorkItemService } from '../../../services/work-item.service';

@Component({
  selector: 'app-work-item-form',
  templateUrl: './work-item-form.component.html',
  styleUrls: ['./work-item-form.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class WorkItemFormComponent {



    // popup variables ///////////////////////////////////////////////////////////////
    showPopup = false;
    popupTitle = '';
    popupMessage = '';
    popupIsSuccess = false;
    popupRedirectPath: string | null = null;
    showCancelButton = false;
  
    ///////////////////////////////////////////////////////////////
onCancel() {
  this.cancel.emit(true);
  
}
  @Input() projectId: string = '';
  @Output() workItemCreated = new EventEmitter<WorkItem>();
  @Output() cancel = new EventEmitter<boolean>();

  workItemForm: FormGroup;
  workItemTypes = Object.values(WorkItemType);
  statuses = Object.values(WorkItemStatus);
  priorities = Object.values(WorkItemPriority);
  teamMembers: any[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private fb: FormBuilder, private userService: UserService, private workItemService: WorkItemService) {
    this.workItemForm = this.fb.group({
      type: [WorkItemType.Task, Validators.required],
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(500)],
      priority: [WorkItemPriority.Medium, Validators.required],
      status: [WorkItemStatus.ToDo, Validators.required],
      estimatedHours: [null, [Validators.min(0), Validators.max(1000)]],
      dueDate: [null],
      labels: ['', Validators.maxLength(100)],
      assignedUserId: [null],
      storyPoints: [null, [Validators.min(1), Validators.max(20)]]
    });
  }

  ngOnInit(): void {
    this.loadTeamMembers();
  }

  loadTeamMembers(): void {
    this.isLoading = true;
    this.userService.getProjectTeam(this.projectId).subscribe({
      next: (members) => {
        this.teamMembers = members;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load team members', err);
        this.errorMessage = 'Failed to load team members';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.workItemForm.invalid) {
      this.workItemForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const formValue = this.workItemForm.value;
    
    const newWorkItem: WorkItem = {
      ...formValue,
      id: this.generateId(),
      projectId: this.projectId,
      createdAt: new Date(),
      updatedAt: new Date(),
      isBlocked: false,
      comments: [],
      attachments: [],
      history: [],
      subItems: [],
      completionPercentage: 0
    };

   this.workItemService.createWorkItem(this.projectId,newWorkItem).subscribe({
    next: (response) => {
      this.showSuccessPopup('Work Item Created', 'Work item created successfully.');
      this.workItemCreated.emit(response);
      this.isLoading = false;
 
    },
    error: (err) => {
      console.error('Failed to create work item', err);
      this.errorMessage = 'Failed to create work item';
      this.showErrorPopup('Error Creating Work Item', 'Failed to create work item. Please try again.');
      
    }
   })
   
  }

  onTypeChange(): void {
    const type = this.workItemForm.value.type;
    if (type === WorkItemType.Epic || type === WorkItemType.Story) {
      this.workItemForm.get('storyPoints')?.setValidators([Validators.min(1), Validators.max(20)]);
    } else {
      this.workItemForm.get('storyPoints')?.clearValidators();
      this.workItemForm.get('storyPoints')?.setValue(null);
    }
    this.workItemForm.get('storyPoints')?.updateValueAndValidity();
  }

  private generateId(): string {
    return 'WI-' + Math.random().toString(36).substring(2, 9).toUpperCase();
  }

  getTypeColor(type: WorkItemType): string {
    switch (type) {
      case WorkItemType.Epic: return '#7e3b8a';
      case WorkItemType.Story: return '#2e7d32';
      case WorkItemType.Task: return '#1976d2';
      case WorkItemType.Bug: return '#d32f2f';
      default: return '#666666';
    }
  }


   /// popup methods //////////////////////////////////////////

   showSuccessPopup(title :string ,message: string) {
    this.popupTitle = title;
    this.popupMessage =  message;
    this.popupIsSuccess = true;
    this.popupRedirectPath = null;
    this.showCancelButton = false;
    this.showPopup = true;
  }

  showErrorPopup(title : string ,errorMessage: string) {
    this.popupTitle =  title;
    this.popupMessage = errorMessage;
    this.popupIsSuccess = false;
    this.popupRedirectPath = null;
    this.showCancelButton = true;
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }
////////////////////////////////////
}