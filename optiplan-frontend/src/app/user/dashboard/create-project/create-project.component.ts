import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectService } from '../../../services/project.service';
import { Router } from '@angular/router';
import { PopupComponent } from "../../../popup/popup.component";
import { CommonModule } from '@angular/common';
import { ProjectDto } from '../../../models/dto/project.dto';

@Component({
  selector: 'app-create-project',
  imports: [PopupComponent,CommonModule,ReactiveFormsModule],
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.css'
})
export class CreateProjectComponent {
  errorMessage: string = '';
  showPopup = false;
  popupTitle = '';
  popupMessage = '';
  popupIsSuccess = false;
  popupRedirectPath: string | null = null;
  showCancelButton = false;
  @Output() cancel = new EventEmitter<boolean>();

  onCancel() {
    this.cancel.emit(false); 
  }

  formProject = new FormGroup({
    title: new FormControl('', [Validators.required, Validators.minLength(3)]),
    description: new FormControl('', [Validators.required, Validators.minLength(10)]),
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl('', [Validators.required])
  });

  constructor(
    private projectService: ProjectService,
    private router: Router
  ) {}

  isInvalidAndTouchedOrDirty(controlName: string): boolean {
    const control = this.formProject.get(controlName);
    return control ? control.invalid && (control.touched || control.dirty) : false;
  }

  onSubmitProject(): void {
    if (this.formProject.invalid) {
      this.markAllAsTouched();
      this.errorMessage = 'Please fill all required fields correctly.';
      return;
    }

    const projectData : ProjectDto = {
      title: this.formProject.get('title')?.value ?? '',
      description: this.formProject.get('description')?.value ?? '',
      startDate: this.formProject.get('startDate')?.value ? new Date(this.formProject.get('startDate')?.value as string).toISOString() : '',
      endDate: this.formProject.get('endDate')?.value ? new Date(this.formProject.get('endDate')?.value as string).toISOString() : ''
    };

    this.projectService.Create(projectData).subscribe({
      next: () => {
        this.showSuccessPopup();
        this.router.navigate(['/projects']);
      },
      error: (err) => {
        console.error(err);
        if (err.status === 401) {
          this.errorMessage = 'Session expired. Please login again.';
        } else {
          this.errorMessage = 'Error creating project. Please try again.';
        }
        this.showErrorPopup("Error creating project");
      }
    });
  }

  private markAllAsTouched(): void {
    Object.values(this.formProject.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  showSuccessPopup() {
    this.popupTitle = 'Project Created!';
    this.popupMessage = 'Your project has been successfully created.';
    this.popupIsSuccess = true;
    this.popupRedirectPath = '/projects';
    this.showCancelButton = false;
    this.showPopup = true;
  }

  showErrorPopup(errorMessage: string) {
    this.popupTitle = 'Project Creation Failed';
    this.popupMessage = errorMessage;
    this.popupIsSuccess = false;
    this.popupRedirectPath = null;
    this.showCancelButton = true;
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }
}