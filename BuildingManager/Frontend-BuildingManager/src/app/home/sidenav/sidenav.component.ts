import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SessionStorageService } from '../../services/session-storage.service';

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.css'
})
export class SidenavComponent {
  public sidebarOpen = false;

  constructor(private sessionStorageService: SessionStorageService) {}

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }
}
