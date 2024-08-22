import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { SessionStorageService } from '../services/session-storage.service';
import { ApartmentReturnModel } from '../models/apartment/apartmentReturnModel';
import { ApartmentService } from '../services/apartment.service';
import { AddApartmentModalComponent } from './add-apartment-modal/add-apartment-modal.component';
import { EditApartmentModalComponent } from './edit-apartment-modal/edit-apartment-modal.component';

@Component({
  selector: 'app-apartment',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddApartmentModalComponent, EditApartmentModalComponent],
  templateUrl: './apartment.component.html',
  styleUrl: './apartment.component.css'
})
export class ApartmentComponent implements OnInit {
  @ViewChild('addApartmentModal') addApartmentModal!: AddApartmentModalComponent;
  @ViewChild('editApartmentModal') editApartmentModal!: EditApartmentModalComponent;
  apartments: ApartmentReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private apartmentService: ApartmentService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadApartments();
  }

  loadApartments() {
    this.apartmentService.getApartments().subscribe(
      (data: ApartmentReturnModel[]) => {
        this.apartments = data;
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
    this.addApartmentModal.open();
    this.addApartmentModal.loadBuildings();
    this.addApartmentModal.loadOwners();
  }

  onApartmentAdded() {
    this.loadApartments();
  }

  openEditModal(apartment: ApartmentReturnModel) {
    this.editApartmentModal.open(apartment);
  }

  deleteApartment(id: number) {
    this.apartmentService.deleteApartment(id).subscribe(
      () => {
        this.loadApartments();
      },
      error => {
        console.error('There was an error deleting the building!', error);
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
