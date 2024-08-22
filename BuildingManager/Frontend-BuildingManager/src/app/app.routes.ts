import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { BuildingComponent } from './building/building.component';
import { ProfileComponent } from './profile/profile.component';
import { InvitationComponent } from './invitation/invitation.component';
import { InvitationResponseComponent } from './invitation-response/invitation-response.component';
import { ApartmentComponent } from './apartment/apartment.component';
import { CategoryComponent } from './category/category.component';
import { CompanyComponent } from './company/company.component';
import { RoleGuard } from './guards/auth-administrator.guard';
import { AuthGuard } from './guards/auth.guard';
import { NoAuthGuard } from './guards/noAuth.guard';
import { OwnerComponent } from './owner/owner.component';
import { StaffComponent } from './staff/staff.component';
import { TaskComponent } from './task/task.component';
import { ReportComponent } from './report/report.component';

export const routes: Routes = [
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
    { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard]},
    { path: 'building', component: BuildingComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['CompanyAdmin', 'Manager']}},
    { path: 'category', component: CategoryComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Administrator']}},
    { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Administrator', 'Manager']}},
    { path: 'invitation', component: InvitationComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Administrator']}},
    { path: 'invitation-response/:id', component: InvitationResponseComponent, canActivate: [NoAuthGuard]},
    { path: 'apartment', component: ApartmentComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Manager']}},
    { path: 'company', component: CompanyComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['CompanyAdmin']}},
    { path: 'owner', component: OwnerComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Manager']}},
    { path: 'staff', component: StaffComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Manager']}},
    { path: 'task', component: TaskComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Manager', 'Staff']}},
    { path: 'report', component: ReportComponent, canActivate: [AuthGuard, RoleGuard], data: { allowedRoles: ['Manager']}},
    { path: '**', redirectTo: '/login'},
];
