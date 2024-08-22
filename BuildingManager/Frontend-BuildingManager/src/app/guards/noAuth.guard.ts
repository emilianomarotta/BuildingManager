import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { SessionStorageService } from "../services/session-storage.service"

@Injectable()
export class NoAuthGuard implements CanActivate{

  constructor(private router: Router, private sessionStorage: SessionStorageService) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = this.sessionStorage.getToken();

    if (token) {
      this.router.navigate(['/home']);
      return false;
    }

    return true;
  }
}