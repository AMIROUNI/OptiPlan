import { Component } from '@angular/core';
import { UserDashboardService } from '../../services/user-dashboard.service';
import { Kpi } from '../../models/dashboard/kpi';
import { KpiCardsComponent } from "../../dashboard-components/kpi-cards/kpi-cards.component";
import { WelcomeCardComponent } from "../../dashboard-components/welcome-card/welcome-card.component";
import { GenericChartComponent } from "../../dashboard-components/growth-chart/generic-chart.component";
import { CommonModule } from '@angular/common';
import { DisplayProjectComponent } from "./display-project/display-project.component";

@Component({
  selector: 'app-dashboard',
  imports: [KpiCardsComponent, WelcomeCardComponent, GenericChartComponent, CommonModule, DisplayProjectComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  kpis: Kpi[] = [];
  err: string = '';
  
  // Chart data variables
  taskChartData: number[] = [];
  taskChartCategories: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  taskStats: any[] = [];
  loadingChart = true;
  chartError: string | null = null;

  constructor(private userDashboardService: UserDashboardService) {}

  ngOnInit(): void {
    this.loadKpis();
    this.loadTaskChartData();
  }

  loadKpis(): void {
    this.userDashboardService.getKpis().subscribe(
      (data: Kpi[]) => {
        this.kpis = data;
        console.log('KPI Data:', this.kpis);
      },
      error => {
        this.err = 'Error loading KPIs';
        console.error('Error loading KPIs:', error);
      }
    );
  }

  loadTaskChartData(): void {
    this.loadingChart = true;
    this.chartError = null;
    
    // Create FormData if your API requires it
    const formData = new FormData();
    // Add any required parameters to formData if needed
    
    this.userDashboardService.GetUserTasksGroupedByProjectForMonth(formData).subscribe(
      (response: any) => {
        console.log (response)
        this.processChartData(response);
        this.loadingChart = false;
      },
      error => {
        this.loadingChart = false;
        this.chartError = 'Failed to load chart data';
        console.error('Error loading task chart data:', error);
      }
    );
  }

  private processChartData(apiData: any): void {
    try {
      // Initialize monthly data array with zeros
      const monthlyData = new Array(12).fill(0);
      
      // Process the API response to count tasks per month
      if (apiData && Array.isArray(apiData)) {
        apiData.forEach((projectGroup: any) => {
          if (projectGroup.tasks && Array.isArray(projectGroup.tasks)) {
            projectGroup.tasks.forEach((task: any) => {
              if (task.createdAt) {
                const taskDate = new Date(task.createdAt);
                const month = taskDate.getMonth(); // 0-11
                monthlyData[month]++;
              }
            });
          }
        });
      }
      
      this.taskChartData = monthlyData;
      
      // Update stats for footer
      const totalTasks = monthlyData.reduce((sum, count) => sum + count, 0);
      const avgTasks = totalTasks > 0 ? Math.round(totalTasks / 12) : 0;
      
      this.taskStats = [
        { value: totalTasks.toString(), label: 'Total Tasks' },
        { value: avgTasks.toString(), label: 'Avg/Month' }
      ];
    } catch (error) {
      console.error('Error processing chart data:', error);
      this.chartError = 'Error processing data';
      this.taskChartData = [];
      this.taskStats = [];
    }
  }
}