import { Component, EventEmitter, Input, Output } from '@angular/core';
import { WorkItem } from '../../../models/work-item';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';

@Component({
  selector: 'app-backlog-view',
  templateUrl: './backlog-view.component.html',
  styleUrls: ['./backlog-view.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class BacklogViewComponent {
  @Input() workItems: WorkItem[] = [];
  @Output() workItemSelected = new EventEmitter<WorkItem>();
  searchQuery = '';

  get filteredWorkItems(): WorkItem[] {
    if (!this.searchQuery) return this.workItems;
    return this.workItems.filter(item => 
      item.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      item.description?.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  getPriorityIcon(priority: WorkItemPriority): string {
    switch (priority) {
      case WorkItemPriority.Low: return 'bi bi-arrow-down';
      case WorkItemPriority.Medium: return 'bi bi-dash';
      case WorkItemPriority.High: return 'bi bi-arrow-up';
      case WorkItemPriority.Critical: return 'bi bi-exclamation-triangle';
      default: return 'bi bi-dash';
    }
  }
}