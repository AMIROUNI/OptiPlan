import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { Skill } from '../../models/skill';
import { UserProfileService } from '../../services/user-profile.service';
import { AuthService } from '../../services/auth.service';
import { UserProfile } from '../../models/dto/profile.dto';
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
  totalSteps: number = 3;
  profilePicturePreview: string | null = null;
  backgroundImagePreview: string | null = null;
  skills: Skill[] = [];
  newSkill: string = '';
  isLoading: boolean = false;
  errorMessage: string | null = null;
  showModal: boolean = true;

  constructor(
    private fb: FormBuilder,
    private userProfileService: UserProfileService,
    private authService: AuthService
  ) {
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      jobTitle: ['', Validators.required],
      bio: ['', [Validators.maxLength(500)]],
      country: [''],
      companyName: [''],
      department: [''],
      phoneNumber: ['']
    });
  }

  ngOnInit(): void {
   
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

  addSkill(): void {
    if (this.newSkill.trim()) {
      this.skills.push({
        id: crypto.randomUUID(),
        name: this.newSkill.trim(),
        userProfileId: '',
        proficiencyLevel: 1,
        yearsExperience: 0
      });
      this.newSkill = '';
    }
  }

  removeSkill(skill: Skill): void {
    this.skills = this.skills.filter(s => s.id !== skill.id);
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
    if (this.profileForm.valid) {
      this.isLoading = true;
      const profile: UserProfile = {
        fullName: this.profileForm.get('fullName')?.value,
        jobTitle: this.profileForm.get('jobTitle')?.value,
        bio: this.profileForm.get('bio')?.value,
        country: this.profileForm.get('country')?.value,
        companyName: this.profileForm.get('companyName')?.value,
        department: this.profileForm.get('department')?.value,
        phoneNumber: this.profileForm.get('phoneNumber')?.value,
        skills: this.skills,
        avatarUrl: ''
      };

      const profilePictureInput = document.getElementById('profilePicture') as HTMLInputElement;
      const backgroundImageInput = document.getElementById('backgroundImage') as HTMLInputElement;
      const avatarFile = profilePictureInput?.files?.[0];
      const backgroundFile = backgroundImageInput?.files?.[0];

      this.userProfileService.InitializeProfile(profile, avatarFile, backgroundFile)
        .subscribe({
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
}