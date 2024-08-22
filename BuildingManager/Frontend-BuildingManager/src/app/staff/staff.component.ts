import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { SessionStorageService } from '../services/session-storage.service';
import { UserReturnModel } from '../models/user/userReturnModel';
import { AddStaffModalComponent } from './add-staff-modal/add-staff-modal.component';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-staff',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddStaffModalComponent],
  templateUrl: './staff.component.html',
  styleUrl: './staff.component.css'
})
export class StaffComponent implements OnInit {
  @ViewChild('addStaffModal') addStaffModal!: AddStaffModalComponent;
  staffMembers: UserReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private userService: UserService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadStaff();
  }

  loadStaff() {
    this.userService.getStaffs().subscribe(
      (data: UserReturnModel[]) => {
        this.staffMembers = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openAddModal() {
    this.addStaffModal.open();
  }

  onStaffAdded() {
    this.loadStaff();
  }

  deleteStaff(id: number) {
    this.userService.deleteStaff(id).subscribe(
      () => {
        this.loadStaff();
      },
      error => {
        console.error('There was an error deleting the staff!', error);
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
