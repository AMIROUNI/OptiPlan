import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserProfile } from '../../models/dto/profile.dto';
import { UserProfileService } from '../../services/user-profile.service';
import { SkillService } from '../../services/skill.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ChatBoxComponent } from "../../chat-box/chat-box.component";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  encapsulation: ViewEncapsulation.None,
  imports: [CommonModule, ReactiveFormsModule, ChatBoxComponent],
  standalone: true
})
export class ProfileComponent implements OnInit {
  profile: UserProfile | null = null;
  isEditing = false;
  isLoading = true;
  isMyProfile = false;
  showSkillForm = false;
  showChat = false;
  username: string = "";
  profileForm: FormGroup;
  newSkillForm: FormGroup;
  selectedUserId:string="";

  constructor(
    private profileService: UserProfileService,
    private skillService: SkillService,
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.profileForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(100)]],
      jobTitle: ['', Validators.maxLength(100)],
      phoneNumber: ['', Validators.maxLength(20)],
      companyName: ['', Validators.maxLength(100)],
      department: ['', Validators.maxLength(100)],
      country: ['', Validators.maxLength(50)],
      bio: ['', Validators.maxLength(1000)]
    });

    this.newSkillForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      proficiencyLevel: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
      yearsExperience: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    console.log('ProfileComponent initialized');
    this.username = this.route.snapshot.paramMap.get("username") ?? "";
    const currentUser = this.authService.getCurrentUsername();
    this.isMyProfile = !this.username || this.username === currentUser;
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.profileService.GetProfile(this.username).subscribe({
      next: (data) => {
        this.profile = data ?? {
          fullName: 'Default Name',
          jobTitle: 'N/A',
          country: '',
          bio: '',
          companyName: '',
          department: '',
          phoneNumber: '',
          avatarUrl: '',
          backGround: '',
          skills: [],
          userId:''
        };
        console.log("this id from the load profile ", data.skills[1]?.id);
        this.profileForm.patchValue(this.profile);
        this.isLoading = false;
        this.selectedUserId=data.userId ?? ""
      },
      error: (err) => {
        console.error('Error loading profile:', err);
        this.isLoading = false;
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.saveProfile();
    }
  }

  saveProfile(): void {
    if (this.profileForm.valid && this.profile) {
      const updatedProfile = { ...this.profile, ...this.profileForm.value };
      this.profileService.UpdateProfile(updatedProfile).subscribe({
        next: () => {
          this.profile = updatedProfile;
          this.isEditing = false;
        },
        error: (err) => console.error('Error updating profile:', err)
      });
    }
  }

  addSkill(): void { 
    this.showSkillForm = true; 
  }

  saveNewSkill(): void {
    if (this.newSkillForm.valid) {
      this.skillService.AddSkill(this.newSkillForm.value).subscribe({
        next: () => {
          this.loadProfile();
          this.newSkillForm.reset({ proficiencyLevel: 5, yearsExperience: 1 });
          this.showSkillForm = false;
        },
        error: (err) => console.error('Error adding skill:', err)
      });
    }
  }

  cancelAddSkill(): void {
    this.showSkillForm = false;
    this.newSkillForm.reset({ proficiencyLevel: 5, yearsExperience: 1 });
  }

  deleteSkill(skillId: string): void {
    if (confirm('Are you sure you want to delete this skill?')) {
      console.log("skill id from the delete method ???", skillId);
      this.skillService.Delete(skillId).subscribe({
        next: () => this.loadProfile(),
        error: (err) => console.error('Error deleting skill:', err)
      });
    }
  }

  openChat(): void {
    this.showChat = true;
  }

  closeChat(): void {
    this.showChat = false;
  }

  getProficiencyStars(level: number): number[] {
    return Array(level).fill(0);
  }

  getEmptyStars(level: number): number[] {
    return Array(5 - level).fill(0);
  }
}