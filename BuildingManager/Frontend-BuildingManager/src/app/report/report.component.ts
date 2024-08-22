import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { SessionStorageService } from '../services/session-storage.service';
import { ReportBuildingsReturnModel } from '../models/report/ReportBuildingsReturnModel';
import { ReportStaffReturnModel } from '../models/report/ReportStaffReturnModel';
import { ReportService } from '../services/report.service';
import { BuildingService } from '../services/building.service';
import { UserService } from '../services/user.service';
import { BuildingReturnModel } from '../models/building/buildingReturnModel';
import { UserReturnModel } from '../models/user/userReturnModel';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, FormsModule],
  templateUrl: './report.component.html',
  styleUrl: './report.component.css'
})
export class ReportComponent implements OnInit {
  buildings: BuildingReturnModel[] = [];
  staffs: UserReturnModel[] = [];
  buildingReports: ReportBuildingsReturnModel[] = [];
  staffReports: ReportStaffReturnModel[] = [];
  selectedBuildingId: number | null = null;
  selectedStaffId: number | null = null;

  constructor(
    private buildingService: BuildingService,
    private userService: UserService,
    private reportService: ReportService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadBuildings();
    this.loadStaff();
    this.loadBuildingsReports();
    this.loadStaffReports();
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

  loadStaff() {
    this.userService.getStaffs().subscribe(
      (data: UserReturnModel[]) => {
        this.staffs = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  loadBuildingsReports() {
    this.reportService.getBuildingsReports().subscribe(
      (data: ReportBuildingsReturnModel[]) => {
        this.buildingReports = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  loadBuildingReport(buildingId: number) {
    this.reportService.getBuildingReport(buildingId).subscribe(
      (data: ReportBuildingsReturnModel[]) => {
        this.buildingReports = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  loadStaffReports() {
    this.reportService.getStaffReports().subscribe(
      (data: ReportStaffReturnModel[]) => {
        this.staffReports = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  loadStaffReport(staffId: number) {
    this.reportService.getStaffReport(staffId).subscribe(
      (data: ReportStaffReturnModel[]) => {
        this.staffReports = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  filterBuildingReport() {
    if (this.selectedBuildingId !== null) {
      this.loadBuildingReport(this.selectedBuildingId);
    }
  }

  filterStaffReport() {
    if (this.selectedStaffId !== null) {
      this.loadStaffReport(this.selectedStaffId);
    }
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }
}
