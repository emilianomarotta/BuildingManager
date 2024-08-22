import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { OwnerService } from '../../services/owner.service';
import { ApartmentService } from '../../services/apartment.service';
import { OwnerReturnModel } from '../../models/owner/ownerReturnModel';
import { ApartmentCreateModel } from '../../models/apartment/apartmentCreateModel';
import { BuildingReturnModel } from '../../models/building/buildingReturnModel';

@Component({
  selector: 'app-add-apartment-modal',
  templateUrl: './add-apartment-modal.component.html',
  styleUrls: ['./add-apartment-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddApartmentModalComponent {
  @Output() apartmentAdded = new EventEmitter<void>();
  apartmentForm: FormGroup;
  isVisible = false;
  buildings: BuildingReturnModel[] = [];
  owners: OwnerReturnModel[] = [];

  constructor(
    private fb: FormBuilder,
    private buildingService: BuildingService,
    private ownerService: OwnerService,
    private apartmentService: ApartmentService
  ) {
    this.apartmentForm = this.fb.group({
      floor: ['', Validators.required],
      number: ['', Validators.required],
      buildingId: ['', Validators.required],
      ownerId: ['', Validators.required],
      bedrooms: ['', Validators.required],
      bathrooms: ['', Validators.required],
      balcony: ['', Validators.required]
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.apartmentForm.reset();
  }

  loadBuildings() {
    this.buildingService.getBuildings().subscribe(
      (data: BuildingReturnModel[]) => {
        this.buildings = data;
      },
      error => {
        console.error('Error loading buildings:', error);
      }
    );
  }

  loadOwners() {
    this.ownerService.getOwners().subscribe(
      (data: OwnerReturnModel[]) => {
        this.owners = data;
      },
      error => {
        console.error('Error loading owners:', error);
      }
    );
  }

  submit() {
    if (this.apartmentForm.valid) {
      const formValue = this.apartmentForm.value;
      const newApartment: ApartmentCreateModel = {
        ...formValue,
        balcony: formValue.balcony === 'true' || formValue.balcony === true
      };
      console.log('newApartment:', newApartment);
      this.apartmentService.addApartment(newApartment).subscribe(
        () => {
          this.close();
          this.apartmentAdded.emit();
          this.apartmentForm.reset();
        },
        error => {
          console.error('Error adding apartment:', error);
        }
      );
    }
  }
}

