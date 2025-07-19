// video-player.component.ts
import { CommonModule } from '@angular/common';
import { Component, Input, ElementRef, ViewChild, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class VideoPlayerComponent implements AfterViewInit {
  @Input() videoUrl!: string;
  @Input() posterUrl?: string;
  @Input() showControls = true;
  
  @ViewChild('videoPlayer') videoPlayer!: ElementRef<HTMLVideoElement>;
  
  isPlaying = false;
  isMuted = false;
  progress = 0;

  ngAfterViewInit() {
    this.videoPlayer.nativeElement.addEventListener('timeupdate', () => {
      this.progress = (this.videoPlayer.nativeElement.currentTime / 
                      this.videoPlayer.nativeElement.duration) * 100;
    });
  }

  togglePlay() {
    if (this.videoPlayer.nativeElement.paused) {
      this.videoPlayer.nativeElement.play();
      this.isPlaying = true;
    } else {
      this.videoPlayer.nativeElement.pause();
      this.isPlaying = false;
    }
  }

  toggleMute() {
    this.isMuted = !this.isMuted;
    this.videoPlayer.nativeElement.muted = this.isMuted;
  }

  seek(event: MouseEvent) {
    const progressBar = event.target as HTMLElement;
    const pos = (event.offsetX / progressBar.offsetWidth);
    this.videoPlayer.nativeElement.currentTime = pos * this.videoPlayer.nativeElement.duration;
  }
}