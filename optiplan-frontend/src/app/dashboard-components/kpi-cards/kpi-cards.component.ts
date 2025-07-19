import { Component, Input } from '@angular/core';
import { Kpi } from '../../models/dashboard/kpi';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-kpi-cards',
  standalone: true,
  templateUrl: './kpi-cards.component.html',
  styleUrl: './kpi-cards.component.css',
  imports: [CommonModule],
})
export class KpiCardsComponent {
  @Input() kpis: Kpi[] = [];  
  @Input() errorMessage: string | null = null; 
}