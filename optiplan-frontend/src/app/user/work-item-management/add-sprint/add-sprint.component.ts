  import { Component, EventEmitter, Input, Output } from '@angular/core';
  import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
  import { SprintService } from '../../../services/sprint.service';
  import { SprintDto } from '../../../models/dto/sprint.dto';
  import { CommonModule } from '@angular/common';

  @Component({
    selector: 'app-add-sprint',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './add-sprint.component.html',
    styleUrls: ['./add-sprint.component.css']
  })
  export class AddSprintComponent {
    @Input() projectId: string = '';
    @Output() sprintCreated = new EventEmitter<boolean>();
    @Output() cancel = new EventEmitter<boolean>();

    sprintForm: FormGroup;
    isLoading = false;
    errorMessage = '';

    constructor(
      private fb: FormBuilder,
      private sprintService: SprintService
    ) {
      this.sprintForm = this.fb.group({
        name: ['', [Validators.required, Validators.maxLength(50)]],
        description: ['', [Validators.maxLength(200)]],
        startDate: ['', [Validators.required]],
        endDate: ['', [Validators.required]]
      });
    }

    onSubmit(): void {
      if (this.sprintForm.invalid) {
        this.sprintForm.markAllAsTouched();
        return;
      }

      this.isLoading = true;
      this.errorMessage = '';

      const sprintData: SprintDto = {
        name: this.sprintForm.value.name,
        description: this.sprintForm.value.description,
        startDate: new Date(this.sprintForm.value.startDate),
        endDate: new Date(this.sprintForm.value.endDate),

      };

      this.sprintService.CreateSprint(this.projectId, sprintData).subscribe({
        next: () => {
          this.isLoading = false;
          this.sprintCreated.emit(true); // Emit true to close modal
          this.resetForm();
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Failed to create sprint. Please try again.';
          console.error('Error creating sprint', err);
          this.sprintCreated.emit(false); // Emit false to keep modal open
        }
      });
    }

    onCancel(): void {
      this.cancel.emit(true);
    }

    private resetForm(): void {
      this.sprintForm.reset({
        name: '',
        description: '',
        startDate: '',
        endDate: ''
      });
    }
  }