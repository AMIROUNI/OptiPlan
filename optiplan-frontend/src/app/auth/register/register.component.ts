import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import { PopupComponent } from "../../popup/popup.component";
import { RegisterDto } from '../../models/dto/register.dto';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PopupComponent,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  avatarFile!: File | null;

  formRegister: FormGroup = this.fb.group({
    username: ['', [Validators.required,Validators.minLength(3), Validators.maxLength(20)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(10)]],
    fullName: ['', [Validators.required]],
    jobTitle: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required]],
    avatarUrl: ['', [Validators.required]],
    companyName: [''],
    department: [''],
    country: ['']
  });

  showPopup = false;
  popupTitle = '';
  popupMessage = '';
  popupIsSuccess = false;
  popupRedirectPath: string | null = null;
  showCancelButton = false;

  isInvalidAndTouchedOrDirty(control: AbstractControl | null): boolean {
    return !!control && control.invalid && (control.touched || control.dirty);
  }

  showSuccessPopup() {
    this.popupTitle = 'Account Created!';
    this.popupMessage = 'Your account has been successfully created.';
    this.popupIsSuccess = true;
    this.popupRedirectPath = '/login';
    this.showCancelButton = false;
    this.showPopup = true;
  }

  showErrorPopup(errorMessage: string) {
    this.popupTitle = 'Registration Failed';
    this.popupMessage = errorMessage;
    this.popupIsSuccess = false;
    this.popupRedirectPath = null;
    this.showCancelButton = true;
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }
onSubmit(): void {
  if (this.formRegister.invalid) {
    this.formRegister.markAllAsTouched();
    return;
  }

  const formValue = this.formRegister.value;
  const formData = new FormData();

  // Append all form fields
  for (const key in formValue) {
    if (formValue[key] != null) {
      formData.append(key, formValue[key]);
    }
  }

  // Append avatar file if exists
  if (this.avatarFile) {
    formData.append("avatar", this.avatarFile);
  }

  this.authService.register(formData).subscribe({
    next: () => this.showSuccessPopup(),
    error: (error) => {
      const errorMsg = error.error?.message || 'Username or email already exists';
      this.showErrorPopup(errorMsg);
    }
  });
}



  onFileSelected(event: any): void {
  const file = event.target.files[0];
  if (file) {
    this.avatarFile = file;
    // Optionally update the form control if needed
    this.formRegister.patchValue({ avatarUrl: file.name });
  }
}
}