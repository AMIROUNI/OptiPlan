import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Project } from '../../../models/project';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class NavbarComponent {
  @Input() project!: Project;
  @Output() createClicked = new EventEmitter<void>();
  @Output() createSprintClicked = new EventEmitter<void>();

  openCreateModal(): void {
    this.createClicked.emit();
  }

  openCreateSprintModal(): void {
    this.createSprintClicked.emit();
  }
}