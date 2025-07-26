import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Project } from '../../models/project';

import { CommonModule } from '@angular/common';
import { WorkItem } from '../../models/work-item';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule]

})
export class SidebarComponent implements OnInit {
  @Input() project!: Project;
  @Input() tasks: WorkItem[] = [];
  @Output() viewChange = new EventEmitter<'board' | 'backlog' | 'reports'>();

  isCollapsed = false;
  activeSection: string = 'overview';
  quickFilters = [
    { id: 'my-tasks', label: 'My Tasks', icon: 'person', count: 0 },
    { id: 'recent', label: 'Recently Updated', icon: 'schedule', count: 0 },
    { id: 'due-soon', label: 'Due Soon', icon: 'alarm', count: 0 },
    { id: 'overdue', label: 'Overdue', icon: 'warning', count: 0 }
  ];

  blockedTasks: number = 0;
  doneTasks: number = 0;
  totalTasks: number = 0;
  progressPercentage: number = 0;

  ngOnInit(): void {
    this.calculateQuickFilterCounts();
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  setActiveSection(section: string): void {
    this.activeSection = section;
  }

  calculateQuickFilterCounts(): void {
    this.quickFilters[0].count = this.tasks.filter(t => t.assignedUser?.username === 'currentUser').length;
    this.quickFilters[1].count = this.tasks.filter(t => {
      const sevenDaysAgo = new Date();
      sevenDaysAgo.setDate(sevenDaysAgo.getDate() - 7);
      return new Date(t.updatedAt) > sevenDaysAgo;
    }).length;
    this.quickFilters[2].count = this.tasks.filter(t => {
      if (!t.dueDate) return false;
      const dueDate = new Date(t.dueDate);
      const inThreeDays = new Date();
      inThreeDays.setDate(inThreeDays.getDate() + 3);
      return dueDate <= inThreeDays && dueDate >= new Date();
    }).length;
    this.quickFilters[3].count = this.tasks.filter(t => t.dueDate && new Date(t.dueDate) < new Date()).length;
  }

  getProgressPercentage(): number {
    if (!this.tasks.length) return 0;
    const completedTasks = this.tasks.filter(task => task.status === 'Done').length;
    return Math.round((completedTasks / this.tasks.length) * 100);
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
  }

  get doneTasksCount(): number {
  return this.tasks ? this.tasks.filter(t => t.status === 'Done').length : 0;
}

get blockedTasksCount(): number {
  return this.tasks ? this.tasks.filter(t => t.isBlocked).length : 0;
}


addTask(): void {
  console.log("Add Task clicked");
  // Rediriger vers une page d'ajout, ou ouvrir un modal
}

}