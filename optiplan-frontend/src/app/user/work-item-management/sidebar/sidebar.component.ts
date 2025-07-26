import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Project } from '../../../models/project';
import { WorkItem } from '../../../models/work-item';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class SidebarComponent {
  @Input() project!: Project;
  @Input() tasks: WorkItem[] = [];
  @Output() viewChange = new EventEmitter<'board' | 'backlog' | 'reports'>();

  isCollapsed = false;
  activeSection: string = 'board';

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  setActiveSection(section: string, viewType?: 'board' | 'backlog' | 'reports'): void {
    this.activeSection = section;
    if (viewType) {
      this.viewChange.emit(viewType);
    }
  }

  getProgressPercentage(): number {
    if (!this.tasks.length) return 0;
    const completedTasks = this.tasks.filter(task => task.status === 'Done').length;
    return Math.round((completedTasks / this.tasks.length) * 100);
  }

  get doneTasksCount(): number {
    return this.tasks.filter(t => t.status === 'Done').length;
  }

  get blockedTasksCount(): number {
    return this.tasks.filter(t => t.isBlocked).length;
  }
}