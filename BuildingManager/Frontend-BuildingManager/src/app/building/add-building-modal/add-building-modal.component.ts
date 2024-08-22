import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { ManagerReturnModel } from '../../models/managerReturnModel';
import { CompanyService } from '../../services/company.service';
import { CompanyReturnModel } from '../../models/company/companyReturnModel';
import { UserService } from '../../services/user.service';
import { BuildingCreateModel } from '../../models/building/buildingCreateModel';

@Component({
  selector: 'app-add-building-modal',
  templateUrl: './add-building-modal.component.html',
  styleUrls: ['./add-building-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddBuildingModalComponent {
  @Output() buildingAdded = new EventEmitter<void>();
  buildingForm: FormGroup;
  isVisible = false;
  managers: ManagerReturnModel[] = [];
  companies: CompanyReturnModel[] = [];

  constructor(
    private fb: FormBuilder,
    private buildingService: BuildingService,
    private userService: UserService,
    private companyService: CompanyService
  ) {
    this.buildingForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      location: ['', Validators.required],
      fees: ['', Validators.required],
      companyId: ['', Validators.required],
      managerId: ['']
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.buildingForm.reset();
  }

  loadManagers() {
    this.userService.getManagers().subscribe(
      (data: ManagerReturnModel[]) => {
        this.managers = data;
      },
      error => {
        console.error('Error loading managers:', error);
      }
    );
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

  submit() {
    if (this.buildingForm.valid) {
      const formValue = this.buildingForm.value;
      const newBuilding: BuildingCreateModel = {
        ...formValue,
        managerId: formValue.managerId || null
      };
      this.buildingService.addBuilding(newBuilding).subscribe(
        () => {
          this.close();
          this.buildingAdded.emit();
          this.buildingForm.reset();
        },
        error => {
          console.error('Error adding building:', error);
        }
      );
    }
  }
}

