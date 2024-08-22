import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { SessionStorageService } from '../services/session-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  showPasswordError: boolean = false;
  isShaking: boolean = false;

  constructor(
    private _authService: AuthService,
    private _sessionStorage: SessionStorageService,
    private _router: Router
  ) {}

login() {
  this._authService.login(this.email, this.password).subscribe(
    (response) => {
      this._sessionStorage.setToken(response.token);
      this._sessionStorage.setRole(response.role);
      this._sessionStorage.setId(response.userId);
      this._sessionStorage.setSessionId(response.id);
      this._sessionStorage.setEmail(this.email);
      this._router.navigate(['/home']);
      this.showPasswordError = false;
    },
    (error) => {
      console.log('error', error);
      this.showPasswordError = true;
      this.isShaking = true;

      this.email = '';
      this.password = '';

      setTimeout(() => {
        this.isShaking = false;
      }, 400);
    }
  );
}
}
