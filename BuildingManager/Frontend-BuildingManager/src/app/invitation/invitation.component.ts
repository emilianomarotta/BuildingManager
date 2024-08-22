import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { InvitationReturnModel } from '../models/invitation/invitationReturnModel';
import { InvitationService } from '../services/invitation.service';
import { InvitationStatus, InvitationStatusDescriptions } from '../helpers/status';
import { AddInvitationModalComponent } from './add-invitation-modal/add-invitation-modal.component';
import { RoleDescriptions } from '../helpers/roles';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-invitation',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddInvitationModalComponent, RouterModule],
  templateUrl: './invitation.component.html',
  styleUrl: './invitation.component.css'
})
export class InvitationComponent implements OnInit{
  @ViewChild('addInvitationModal') addInvitationModal!: AddInvitationModalComponent;
  invitations: InvitationReturnModel[] = [];

  constructor(private invitationService: InvitationService) {}

  ngOnInit() {
    this.loadInvitations();
  }

  loadInvitations() {
    this.invitationService.getInvitations().subscribe(
      (data: InvitationReturnModel[]) => {
        this.invitations = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  getStatusDescription(status: number): string {
    return InvitationStatusDescriptions[status] || 'Unknown';
  }

  getRoleDescription(role: number): string {
    return RoleDescriptions[role] || 'Unknown';
  }

  isPending(status: number): boolean {
    return status === InvitationStatus.Pending;
  }

  openModal() {
    this.addInvitationModal.open();
  }

  onInvitationAdded() {
    this.loadInvitations();
  }
}
