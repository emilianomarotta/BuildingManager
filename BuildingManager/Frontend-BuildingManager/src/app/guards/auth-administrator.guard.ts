import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { SessionStorageService } from '../services/session-storage.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private router: Router, private sessionStorage: SessionStorageService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const allowedRoles = route.data['allowedRoles'] as Array<string>;
    const userRole = this.sessionStorage.getRole();

    if (userRole && !allowedRoles.includes(userRole)) {
      this.router.navigate(['/home']);
      return false;
    }

    return true;
  }
}