import { Component, EventEmitter, Input, Output } from '@angular/core';
import { WorkItemService } from '../../../services/work-item.service';
import { CommonModule } from '@angular/common';
import { WorkItem } from '../../../models/work-item';
import { WorkItemStatus } from '../../../models/enums/work-item-status';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { UpdateWorkItemStatusDto } from '../../../models/dto/updateWorkItemStatus.dto';

@Component({
  selector: 'app-board-view',
  templateUrl: './board-view.component.html',
  styleUrls: ['./board-view.component.css'],
  standalone: true,
  imports: [CommonModule,DragDropModule]
})
export class BoardViewComponent {
  @Input() workItems: WorkItem[] = [];
  @Output() workItemSelected = new EventEmitter<WorkItem>();

  statuses = Object.values(WorkItemStatus);

  constructor(private workItemService: WorkItemService) {}

  getWorkItemsByStatus(status: WorkItemStatus): WorkItem[] {
    return this.workItems.filter(item => item.status === status);
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

  onDragStart(event: DragEvent, item: WorkItem): void {
    event.dataTransfer?.setData('text/plain', item.id);
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  




  onDrop(status: WorkItemStatus, event: DragEvent): void {
  event.preventDefault();
  const itemId = event.dataTransfer?.getData('text/plain');
  if (!itemId) return;

  const item = this.workItems.find(i => i.id === itemId);
  if (item && item.status !== status) {
    const dto: UpdateWorkItemStatusDto = {
      workItemId: item.id,
      newStatus: status
    };

    this.workItemService.updateStatus(dto).subscribe({
      next: () => {
        // Mise à jour locale seulement si succès
        item.status = status;
      },
      error: (err) => {
        console.error('Failed to update work item status', err);
        // tu peux ajouter un feedback UI ici (alerte, snackbar, etc.)
      }
    });
  }

}
}