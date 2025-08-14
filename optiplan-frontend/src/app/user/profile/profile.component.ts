import { Component, OnInit } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEdit, faSave, faTrash, faPlus, faUser, faBriefcase, faPhone, faBuilding, faGlobe, faCode } from '@fortawesome/free-solid-svg-icons';
import { UserProfile } from '../../models/dto/profile.dto';
import { Skill } from '../../models/skill';
import { UserProfileService } from '../../services/user-profile.service';
import { SkillService } from '../../services/skill.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, FontAwesomeModule]
})
export class ProfileComponent implements OnInit {
  profile: UserProfile | null = null;
  skills: Skill[] = [];
  isEditing = false;
  isLoading = true;
  error: string | null = null;
  
  // Icons
  faEdit = faEdit;
  faSave = faSave;
  faTrash = faTrash;
  faPlus = faPlus;
  faUser = faUser;
  faBriefcase = faBriefcase;
  faPhone = faPhone;
  faBuilding = faBuilding;
  faGlobe = faGlobe;
  faCode = faCode;

  profileForm: FormGroup;
  newSkillForm: FormGroup;

  constructor(
    private profileService: UserProfileService,
    private skillService: SkillService,
    private fb: FormBuilder
  ) {
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      jobTitle: [''],
      phoneNumber: [''],
      companyName: [''],
      department: [''],
      country: [''],
      bio: ['']
    });

    this.newSkillForm = this.fb.group({
      name: ['', Validators.required],
      proficiencyLevel: [1, [Validators.required, Validators.min(1), Validators.max(10)]],
      yearsExperience: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.loadProfile();
    this.loadSkills();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.profileService.GetProfile().subscribe({
      next: (data) => {
        this.profile = data;
        this.profileForm.patchValue({
          fullName: data.fullName,
          jobTitle: data.jobTitle,
          phoneNumber: data.phoneNumber,
          companyName: data.companyName,
          department: data.department,
          country: data.country,
          bio: data.bio
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load profile';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  loadSkills(): void {
    this.skillService.Get().subscribe({
      next: (data: any) => {
        this.skills = Array.isArray(data) ? data : [];
      },
      error: (err) => {
        console.error('Failed to load skills', err);
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing && this.profile) {
      this.profileForm.patchValue({
        fullName: this.profile.fullName,
        jobTitle: this.profile.jobTitle,
        phoneNumber: this.profile.phoneNumber,
        companyName: this.profile.companyName,
        department: this.profile.department,
        country: this.profile.country,
        bio: this.profile.bio
      });
    }
  }

  saveProfile(): void {
    if (this.profileForm.valid && this.profile) {
      const updatedProfile = {
        ...this.profile,
        ...this.profileForm.value
      };

      this.profileService.UpdateProfile(updatedProfile).subscribe({
        next: () => {
          this.profile = updatedProfile;
          this.isEditing = false;
        },
        error: (err) => {
          console.error('Failed to update profile', err);
        }
      });
    }
  }

  addSkill(): void {
    if (this.newSkillForm.valid) {
      this.skillService.AddSkill(this.newSkillForm.value).subscribe({
        next: () => {
          this.loadSkills();
          this.newSkillForm.reset({
            proficiencyLevel: 1,
            yearsExperience: 1
          });
        },
        error: (err) => {
          console.error('Failed to add skill', err);
        }
      });
    }
  }

  deleteSkill(skillId: string): void {
    if (confirm('Are you sure you want to delete this skill?')) {
      this.skillService.Delete(skillId).subscribe({
        next: () => {
          this.loadSkills();
        },
        error: (err) => {
          console.error('Failed to delete skill', err);
        }
      });
    }
  }

  getProficiencyStars(level: number): number[] {
    return Array(level).fill(0);
  }

  getEmptyStars(level: number): number[] {
    return Array(10 - level).fill(0);
  }
}