import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { AuthService } from './services/auth.service';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { SessionStorageService } from './services/session-storage.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { APIInterceptor } from './interceptors/api.interceptor';
import { BuildingService } from './services/building.service';
import { CompanyService } from './services/company.service';
import { UserService } from './services/user.service';
import { InvitationService } from './services/invitation.service';
import { ApartmentService } from './services/apartment.service';
import { OwnerService } from './services/owner.service';
import { CategoryService } from './services/category.service';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { NotificationService } from './services/notification.service';
import { RoleGuard } from './guards/auth-administrator.guard';
import { AuthGuard } from './guards/auth.guard';
import { NoAuthGuard } from './guards/noAuth.guard';
import { TaskService } from './services/task.service';
import { ReportService } from './services/report.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideHttpClient(withInterceptorsFromDi()),
    AuthService,
    SessionStorageService,    
    BuildingService,
    CategoryService,
    CompanyService,
    UserService,
    InvitationService,
    ApartmentService,
    OwnerService,
    NotificationService,
    RoleGuard,
    AuthGuard,
    NoAuthGuard,
    TaskService,
    ReportService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: APIInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    }
  ]
};
