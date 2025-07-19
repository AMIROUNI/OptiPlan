import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { VideoPlayerComponent } from "../video-player/video-player.component";




@Component({
  selector: 'app-ather-landing',
  standalone: true,
  templateUrl: './ather-landing.component.html',
  styleUrl: './ather-landing.component.css',
  imports: [CommonModule, VideoPlayerComponent],
})
export class AtherLandingComponent {
    currentYear = new Date().getFullYear();
  
  services = [
    { 
      title: 'Gestion des Candidats', 
      icon: 'people',
      description: 'Organisez et suivez facilement les candidatures avec des outils puissants et personnalisables.'
    },
    { 
      title: 'Planification d\'Entretiens', 
      icon: 'calendar_today',
      description: 'Automatisez la prise de rendez-vous et coordonnez les entretiens sans effort.'
    },
    { 
      title: 'Analytique RH', 
      icon: 'analytics',
      description: 'Obtenez des insights précieux grâce à des rapports détaillés sur vos processus de recrutement.'
    }
  ];

}
