import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { ManagerReturnModel } from '../../models/managerReturnModel';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { BuildingReturnModel } from '../../models/building/buildingReturnModel';
import { BuildingPutModel } from '../../models/building/buildingPutModel';

@Component({
  selector: 'app-edit-building-modal',
  templateUrl: './edit-building-modal.component.html',
  styleUrls: ['./edit-building-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})

export class EditBuildingModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  buildingForm: FormGroup;
  managers: ManagerReturnModel[] = [];
  building!: BuildingReturnModel;

  constructor(
    private fb: FormBuilder,
    private buildingService: BuildingService,
    private userService: UserService
  ) {
    this.buildingForm = this.fb.group({
      fees: [0, Validators.required],
      managerId: [null]
    });
  }

  open(building: BuildingReturnModel) {
    this.isVisible = true;
    this.building = building;
    this.buildingForm.patchValue({
      fees: building.fees,
      managerId: building.manager ? building.manager.id : null
    });
    this.loadManagers();
  }

  close() {
    this.isVisible = false;
    this.buildingForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.buildingForm.valid) {
      const formValue = this.buildingForm.value;
      const updatedBuilding: BuildingPutModel = {
        fees: formValue.fees,
        managerId: formValue.managerId || null
      };
      this.buildingService.updateBuilding(this.building.id, updatedBuilding).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error updating building:', error);
        }
      );
    }
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
}
