import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  styleUrl: './toast.component.css',
  templateUrl: './toast.component.html',
  imports: [CommonModule],
  selector: 'app-toast',
  styles: []
})
export class ToastComponent implements OnInit {
  message: string | null = null;

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.notification$.subscribe(message => {
      this.message = message;
      console.log('message', message);
      setTimeout(() => this.message = null, 3000);
    });
  }
}