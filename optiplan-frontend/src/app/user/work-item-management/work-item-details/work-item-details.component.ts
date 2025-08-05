// work-item-details.component.ts
import {
  Component,
  Input,
  OnInit,
  ViewChild,
  ElementRef,
  OnChanges,
  SimpleChanges,
  Output,
  EventEmitter
} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { WorkItem } from '../../../models/work-item';
import { WorkItemService } from '../../../services/work-item.service';
import { AttachmentService } from '../../../services/attachment.service';
import { CommentService } from '../../../services/comment.service';
import { Comment } from '../../../models/comment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectService } from '../../../services/project.service';
import { User } from '../../../models/user';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { WorkItemHistory } from '../../../models/work-item-history';
import { WorkItemHistoryService } from '../../../services/work-item-history.service';
import { saveAs } from 'file-saver';
import { FileSizePipe } from "../../../pipes/file-size.pipe";

@Component({
  selector: 'app-work-item-details',
  templateUrl: './work-item-details.component.html',
  styleUrls: ['./work-item-details.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, FileSizePipe]
})
export class WorkItemDetailsComponent implements OnInit, OnChanges {
  @Input() workItemId!: string;
  @Output() closeModal = new EventEmitter<void>();
  @ViewChild('modal') modal!: ElementRef;

  workItem: WorkItem | null = null;
  comments: Comment[] = [];
  attachments: any[] = [];
  historyItems: WorkItemHistory[] = [];
  isLoading = false;
  newCommentText = '';
  selectedFile: File | null = null;
  teamMembers: User[] = [];
  selectedUserId: string = '';
  showModal = false;
  defaultAvatar = 'assets/images/default-profile.png';
  apiUrl = environment.apiUrl;
  @Input() projectID!: string;
  activeTab: 'details' | 'comments' | 'attachments' | 'history' = 'details';

  constructor(
    private workItemService: WorkItemService,
    private attachmentService: AttachmentService,
    private commentService: CommentService,
    private projectService: ProjectService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private workItemHistoryService: WorkItemHistoryService
  ) {}

  ngOnInit(): void {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['workItemId'] && this.workItemId) {
      this.showModal = true;
      this.loadWorkItemDetails();
    }
  }

  close(): void {
    this.showModal = false;
    this.resetForm();
    this.closeModal.emit();
  }

  loadWorkItemDetails(): void {
    this.isLoading = true;
    this.workItemService.getWorkItemsByProject(this.projectID).pipe(
      catchError(error => {
        this.toastr.error('Failed to load work item details', 'Error');
        console.error('Error loading work item:', error);
        return of(null);
      })
    ).subscribe(data => {
      if (data) {
        this.workItem = data.find((item: WorkItem) => item.id === this.workItemId) || null;
        if (this.workItem?.projectId) {
          this.loadTeamMembers();
        }
        this.loadComments();
        this.loadAttachments();
        this.loadWorkItemHistory();
      }
      this.isLoading = false;
    });
  }

  loadWorkItemHistory(): void {
    if (!this.workItemId) return;
    this.workItemHistoryService.GetWorkItemHistoryForWorkItem(this.workItemId).subscribe({
      next: (history) => {
        this.historyItems = history;
        console.log("Work item history loaded", this.historyItems);
      },
      error: (err) => {
        console.error('Error loading work item history:', err);
        this.toastr.error('Failed to load work item history', 'Error');
      }
    });
  }

  loadTeamMembers(): void {
    if (this.workItem?.projectId) {
      this.projectService.getTeamMemberShips(this.workItem.projectId).subscribe({
        next: (members) => this.teamMembers = members,
        error: (err) => console.error('Error loading team members:', err)
      });
    }
  }

  loadComments(): void {
    this.commentService.GetCommentsByWorkItemIdAsync(this.workItemId).subscribe({
      next: (comments) =>{ this.comments = comments; console.log(comments)  },
      error: (err) => console.error('Error loading comments:', err)
    });
  }

  loadAttachments(): void {
    this.attachmentService.GetAttachmentsByWorkItemId(this.workItemId).subscribe({
      next: (attachments) => this.attachments = attachments,
      error: (err) => console.error('Error loading attachments:', err)
    });
  }

  createComment(): void {
    if (!this.newCommentText.trim()) return;
    const comment: Comment = {
      workItemId: this.workItemId,
      content: this.newCommentText,
    };
    this.commentService.CreateComment(comment).subscribe({
      next: () => {
        this.toastr.success('Comment added successfully');
        this.newCommentText = '';
        this.loadComments();
      },
      error: (err) => {
        this.toastr.error('Failed to add comment', 'Error');
        console.error('Error creating comment:', err);
      }
    });
  }

  uploadAttachment(): void {
    if (!this.selectedFile) return;
    this.attachmentService.UploadAttachment(this.selectedFile, this.workItemId).subscribe({
      next: () => {
        this.toastr.success('Attachment uploaded successfully');
        this.selectedFile = null;
        this.loadAttachments();
      },
      error: (err) => {
        this.toastr.error('Failed to upload attachment', 'Error');
        console.error('Error uploading attachment:', err);
      }
    });
  }

  downloadAttachment(attachment: any): void {
    this.attachmentService.DownloadAttachment(attachment.id).subscribe({
      next: (data) => {
        const blob = new Blob([data], { type: attachment.contentType });
        saveAs(blob, attachment.name);
      },
      error: (err) => {
        this.toastr.error('Failed to download attachment', 'Error');
        console.error('Error downloading attachment:', err);
      }
    });
  }

  assignUser(): void {
    if (!this.selectedUserId) return;
    this.workItemService.AssignUserToWorkItem(this.selectedUserId, this.workItemId).subscribe({
      next: () => {
        this.toastr.success('User assigned successfully');
        this.selectedUserId = '';
        this.loadWorkItemDetails();
      },
      error: (err) => {
        this.toastr.error('Failed to assign user', 'Error');
        console.error('Error assigning user:', err);
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  resetForm(): void {
    this.newCommentText = '';
    this.selectedFile = null;
    this.selectedUserId = '';
  }

  getAvatarUrl(userImg: string | undefined): string {
    if (!userImg) return this.defaultAvatar;
    return userImg.startsWith('http') ? userImg : `${this.apiUrl}/${userImg}`;
  }

  formatFieldName(field: string): string {
    return field.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase());
  }

  setActiveTab(tab: 'details' | 'comments' | 'attachments' | 'history'): void {
    this.activeTab = tab;
  }
}