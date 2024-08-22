import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { SessionStorageService } from '../services/session-storage.service';
import { OwnerService } from '../services/owner.service';
import { OwnerReturnModel } from '../models/owner/ownerReturnModel';
import { AddOwnerModalComponent } from './add-owner-modal/add-owner-modal.component';
import { EditOwnerModalComponent } from './edit-owner-modal/edit-owner-modal.component';

@Component({
  selector: 'app-owner',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddOwnerModalComponent, EditOwnerModalComponent],
  templateUrl: './owner.component.html',
  styleUrl: './owner.component.css'
})
export class OwnerComponent implements OnInit {
  @ViewChild('addOwnerModal') addOwnerModal!: AddOwnerModalComponent;
  @ViewChild('editOwnerModal') editOwnerModal!: EditOwnerModalComponent;
  owners: OwnerReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private ownerService: OwnerService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadOwners();
  }

  loadOwners() {
    this.ownerService.getOwners().subscribe(
      (data: OwnerReturnModel[]) => {
        this.owners = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }
  
  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openModal() {
    this.addOwnerModal.open();
  }

  onOwnerAdded() {
    this.loadOwners();
  }

  openEditModal(owner: OwnerReturnModel) {
    this.editOwnerModal.open(owner);
  }

  deleteOwner(id: number) {
    this.ownerService.deleteOwner(id).subscribe(
      () => {
        this.loadOwners();
      },
      error => {
        console.error('There was an error deleting the owner!', error);
      }
    );
  }

  toggleMenu(index: number) {
    this.menuOpenIndex = this.menuOpenIndex === index ? null : index;
  }

  closeMenu() {
    this.menuOpenIndex = null;
  }
}
