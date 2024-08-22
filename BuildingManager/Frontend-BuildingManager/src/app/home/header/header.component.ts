import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SessionStorageService } from '../../services/session-storage.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  menuOpen = false;
  constructor(
    private sessionStorageService: SessionStorageService,
    private _authService: AuthService,
    private _router: Router
  ) {}

  ngOnInit() {
    this.getRole();
    this.getEmail();
  }
  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  getEmail(): string | null {
    return this.sessionStorageService.getEmail();
  }

  logout() {
    this._authService.logout().subscribe(
      () => {
        this.sessionStorageService.clear();
        this._router.navigate(['/login']);
      },
      (error) => {
        console.log('error', error);
      }
    );
  }
}
