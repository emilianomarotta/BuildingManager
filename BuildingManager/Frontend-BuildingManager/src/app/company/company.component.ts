import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { SessionStorageService } from '../services/session-storage.service';
import { CompanyReturnModel } from '../models/company/companyReturnModel';
import { CompanyService } from '../services/company.service';
import { AddCompanyModalComponent } from './add-company-modal/add-company-modal.component';
import { EditCompanyModalComponent } from './edit-company-modal/edit-company-modal.component';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddCompanyModalComponent, EditCompanyModalComponent],
  templateUrl: './company.component.html',
  styleUrl: './company.component.css'
})
export class CompanyComponent implements OnInit {
  @ViewChild('addCompanyModal') addCompanyModal!: AddCompanyModalComponent;
  @ViewChild('editCompanyModal') editCompanyModal!: EditCompanyModalComponent;
  companies: CompanyReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private companyService: CompanyService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyService.getCompanies().subscribe(
      (data: CompanyReturnModel[]) => {
        this.companies = data;
      },
      error => {
        console.error('Error loading companies:', error);
      }
    );
  }
  
  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openAddModal() {
    this.addCompanyModal.open();
  }

  onCompanyAdded() {
    this.loadCompanies();
  }

  openEditModal(company: CompanyReturnModel) {
    this.editCompanyModal.open(company);
  }

  deleteCompany(id: number) {
    this.companyService.deleteCompany(id).subscribe(
      () => {
        this.loadCompanies();
      },
      error => {
        console.error('There was an error deleting the company!', error);
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
