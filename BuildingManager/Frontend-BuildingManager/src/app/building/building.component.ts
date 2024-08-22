import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { BuildingService } from '../services/building.service';
import { AddBuildingModalComponent } from './add-building-modal/add-building-modal.component';
import { EditBuildingModalComponent } from './edit-building-modal/edit-building-modal.component';
import { UploadBuildingsModalComponent } from './upload-buildings-modal/upload-buildings-modal.component';
import { SessionStorageService } from '../services/session-storage.service';
import { BuildingReturnModel } from '../models/building/buildingReturnModel';

@Component({
  selector: 'app-building',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddBuildingModalComponent, EditBuildingModalComponent, UploadBuildingsModalComponent],
  templateUrl: './building.component.html',
  styleUrl: './building.component.css'
})
export class BuildingComponent implements OnInit {
  @ViewChild('addBuildingModal') addBuildingModal!: AddBuildingModalComponent;
  @ViewChild('editBuildingModal') editBuildingModal!: EditBuildingModalComponent;
  @ViewChild('uploadBuildingsModal') uploadBuildingsModal!: UploadBuildingsModalComponent;
  buildings: BuildingReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private buildingService: BuildingService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadBuildings();
  }

  loadBuildings() {
    this.buildingService.getBuildings().subscribe(
      (data: BuildingReturnModel[]) => {
        this.buildings = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openGoogleMaps(location: string): void {
    const [lat, lng] = location.split(',');
    const url = `https://www.google.com/maps/search/?api=1&query=${lat},${lng}`;
    window.open(url, '_blank');
  }

  openAddModal() {
    this.addBuildingModal.open();
    this.addBuildingModal.loadCompanies();
    this.addBuildingModal.loadManagers();
  }

  openUploadModal() {
    this.uploadBuildingsModal.open();
    this.uploadBuildingsModal.loadImporters();
  }

  onBuildingAdded() {
    this.loadBuildings();
  }

  openEditModal(building: BuildingReturnModel) {
    this.editBuildingModal.open(building);
  }

  deleteBuilding(id: number) {
    this.buildingService.deleteBuilding(id).subscribe(
      () => {
        this.loadBuildings();
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
