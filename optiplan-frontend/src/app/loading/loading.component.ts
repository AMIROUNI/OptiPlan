import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-service-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css'],
  standalone: true
})
export class LoadingComponent implements OnInit {
  constructor(private router: Router) {}
  services = [
    { 
      title: "Workflow Automation", 
      description: "Optimizing your business processes with AI-driven automation",
      icon: "⚙️"
    },
    { 
      title: "Resource Planning", 
      description: "Smart allocation of resources for maximum efficiency",
      icon: "📊"
    },
    { 
      title: "Project Tracking", 
      description: "Real-time monitoring of all your project milestones",
      icon: "📌"
    }
  ];
  
  currentService = this.services[0];
  progress = 0;

  ngOnInit() {
    this.rotateServices();
    this.startProgress();
  }

  rotateServices() {
    let index = 0;
    setInterval(() => {
      index = (index + 1) % this.services.length;
      this.currentService = this.services[index];
    }, 3000);
  }

  startProgress(): void {
    const interval = setInterval(() => {
      this.progress += Math.random() * 10;
      if (this.progress >= 100) {
        this.progress = 100;
        clearInterval(interval);
        this.router.navigate(['/landing']); 
      }
    }, 100);
  }
}