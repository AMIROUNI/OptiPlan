import { Component, Input } from '@angular/core';
import { WorkItem} from '../../../models/work-item';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reports-view',
  templateUrl: './reports-view.component.html',
  styleUrls: ['./reports-view.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class ReportsViewComponent {
  @Input() workItems: WorkItem[] = [];

  getStatusDistribution(): { [key: string]: number } {
    return this.workItems.reduce((acc, item) => {
      acc[item.status] = (acc[item.status] || 0) + 1;
      return acc;
    }, {} as { [key: string]: number });
  }

  getTypeDistribution(): { [key: string]: number } {
    return this.workItems.reduce((acc, item) => {
      acc[item.type] = (acc[item.type] || 0) + 1;
      return acc;
    }, {} as { [key: string]: number });
  }
}