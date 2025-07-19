import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
  imports: [CommonModule],
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
  
  quickLinks = [
    { name: 'Dashboard', icon: 'dashboard' },
    { name: 'Projects', icon: 'folder' },
    { name: 'Analytics', icon: 'analytics' },
    { name: 'Team', icon: 'people' }
  ];

  socialLinks = [
    { name: 'Twitter', icon: 'twitter', url: '#' },
    { name: 'LinkedIn', icon: 'linkedin', url: '#' },
    { name: 'GitHub', icon: 'code', url: '#' },
    { name: 'YouTube', icon: 'play_circle', url: '#' }
  ];

  companyInfo = [
    { name: 'About Us', url: '#' },
    { name: 'Careers', url: '#' },
    { name: 'Privacy Policy', url: '#' },
    { name: 'Terms of Service', url: '#' }
  ];
}