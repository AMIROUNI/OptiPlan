  import { Component, EventEmitter, Input, Output } from '@angular/core';
  import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
  import { SprintService } from '../../../services/sprint.service';
  import { SprintDto } from '../../../models/dto/sprint.dto';
  import { CommonModule } from '@angular/common';
import { PopupComponent } from "../../../popup/popup.component";

  @Component({
    selector: 'app-add-sprint',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule, PopupComponent],
    templateUrl: './add-sprint.component.html',
    styleUrls: ['./add-sprint.component.css']
  })
  export class AddSprintComponent {


    
  // popup variables ///////////////////////////////////////////////////////////////
  showPopup = false;
  popupTitle = '';
  popupMessage = '';
  popupIsSuccess = false;
  popupRedirectPath: string | null = null;
  showCancelButton = false;

  ///////////////////////////////////////////////////////////////
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
      console.log(this.sprintForm.value);
     /* if (this.sprintForm.invalid) {
        this.sprintForm.markAllAsTouched();
        return;
      }*/

      this.isLoading = true;
      this.errorMessage = '';

      const sprintData: SprintDto = {
        name: this.sprintForm.value.name,
        description: this.sprintForm.value.description,
        startDate: new Date(this.sprintForm.value.startDate),
        endDate: new Date(this.sprintForm.value.endDate),

      };

      console.log(sprintData);

      this.sprintService.CreateSprint(this.projectId, sprintData).subscribe({
        next: () => {
          console.log('Sprint created successfully');
          this.isLoading = false;
          this.showSuccessPopup('Sprint Created', 'Sprint created successfully.');
         // this.sprintCreated.emit(true); // Emit true to close modal
          this.resetForm();
        },
        error: (err) => {
          console.log('Failed to create sprint');
          this.isLoading = false;
          this.errorMessage = 'Failed to create sprint. Please try again.';
          console.error('Error creating sprint', err);
          this.showErrorPopup('Error Creating Sprint', 'Failed to create sprint. Please try again.');
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