import { Component, EventEmitter, Input, Output } from '@angular/core';
import { WorkItemService } from '../../../services/work-item.service';
import { CommonModule } from '@angular/common';
import { WorkItem } from '../../../models/work-item';
import { WorkItemStatus } from '../../../models/enums/work-item-status';
import { WorkItemPriority } from '../../../models/enums/work-item-priority';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { UpdateWorkItemStatusDto } from '../../../models/dto/updateWorkItemStatus.dto';
import { ConfirmationService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { PopupComponent } from "../../../popup/popup.component";

@Component({
  selector: 'app-board-view',
  templateUrl: './board-view.component.html',
  styleUrls: ['./board-view.component.css'],
  standalone: true,
  imports: [CommonModule, DragDropModule, DialogModule, ButtonModule, PopupComponent],
  providers: [ConfirmationService]
})
export class BoardViewComponent {


   // popup variables ///////////////////////////////////////////////////////////////
   showPopup = false;
   popupTitle = '';
   popupMessage = '';
   popupIsSuccess = false;
   popupRedirectPath: string | null = null;
   showCancelButton = false;
 
   ///////////////////////////////////////////////////////////////
  @Input() workItems: WorkItem[] = [];
  @Output() workItemSelected = new EventEmitter<WorkItem>();
  @Output() workItemDeleted = new EventEmitter<string>();

  statuses = Object.values(WorkItemStatus);
  displayDeleteDialog = false;
  itemToDelete: WorkItem | null = null;

  constructor(
    private workItemService: WorkItemService,
    private confirmationService: ConfirmationService
  ) {}

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
          item.status = status;
        },
        error: (err) => {
          console.error('Failed to update work item status', err);
        }
      });
    }
  }

  deleteWorkItem(itemId: string): void {
    console.log("delete now ok");
    
  
    this.workItemService.deleteWorkItem(itemId).subscribe({
      next: (rep) => {
        console.log("delete it", rep);
        this.showSuccessPopup('Work Item Deleted', 'Work item deleted successfully.');
   
      },
      error: (error) => {
        console.error("delete error", error);
        this.showErrorPopup('Error Deleting Work Item', 'Failed to delete work item. Please try again.');
      }
    });
  }




   /// popup methods //////////////////////////////////////////

   showSuccessPopup(title :string ,message: string) {
    this.popupTitle = title;
    this.popupMessage =  message;
    this.popupIsSuccess = true;
    this.popupRedirectPath = null;
    this.showCancelButton = false;
    this.showPopup = true;
  }

  showErrorPopup(title : string ,errorMessage: string) {
    this.popupTitle =  title;
    this.popupMessage = errorMessage;
    this.popupIsSuccess = false;
    this.popupRedirectPath = null;
    this.showCancelButton = true;
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }
////////////////////////////////////
  
 }
    
    
    
    

  
