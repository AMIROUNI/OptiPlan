import { Component } from '@angular/core';
import { ProjectService } from '../../../services/project.service';
import { Project } from '../../../models/project';
import { CommonModule } from '@angular/common';
import { CreateProjectComponent } from "../create-project/create-project.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-display-project',
  templateUrl: './display-project.component.html',
  styleUrls: ['./display-project.component.css'],
  imports: [CommonModule, CreateProjectComponent,RouterLink]
})
export class DisplayProjectComponent {
  projects: Project[] = [];
  isLoading = true;
  showForm = false;

  constructor(private projectService: ProjectService) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading = true;
    this.projectService.GetProjectsForUserAsync().subscribe(
      (projects: Project[]) => {
        this.projects = projects;
        this.isLoading = false;
      },
      error => {
        console.error('Error loading projects:', error);
        this.isLoading = false;
      }
    );
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  onCreateProject(): void {
    console.log("Create project button clicked");
    this.showForm = true;
  }

onClosePopup(value: boolean) {
  this.showForm = value; // will be false when cancel is clicked
}
}