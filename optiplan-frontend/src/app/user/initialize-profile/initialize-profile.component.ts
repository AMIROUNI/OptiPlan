import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { UserProfileService } from '../../services/user-profile.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-initialize-profile',
  templateUrl: './initialize-profile.component.html',
  styleUrls: ['./initialize-profile.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class InitializeProfileComponent implements OnInit {
  profileForm: FormGroup;
  currentStep: number = 1;
  totalSteps: number = 2;
  profilePicturePreview: string | null = null;
  backgroundImagePreview: string | null = null;
  isLoading: boolean = false;
  errorMessage: string | null = null;
  @Input() showModal: boolean = true;

  constructor(
    private fb: FormBuilder,
    private userProfileService: UserProfileService,
    private authService: AuthService
  ) {
    this.profileForm = this.fb.group({
      bio: ['', [Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    // Check if it's the user's first login
   
  }

  onProfilePictureChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.profilePicturePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onBackgroundImageChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.backgroundImagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  nextStep(): void {
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  skipProfile(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    this.isLoading = true;
    const profile = {
      bio: this.profileForm.get('bio')?.value || ''
    };

    const avatarFile = (document.getElementById('profilePicture') as HTMLInputElement)?.files?.[0];
    const backgroundFile = (document.getElementById('backgroundImage') as HTMLInputElement)?.files?.[0];

    this.userProfileService.InitializeProfile(profile, avatarFile, backgroundFile).subscribe({
      next: () => {
        this.isLoading = false;
        this.showModal = false;
        this.errorMessage = null;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to initialize profile. Please try again.';
        console.error(err);
      }
    });
  }
}