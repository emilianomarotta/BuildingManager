import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OwnerReturnModel } from '../../models/owner/ownerReturnModel';
import { ApartmentReturnModel } from '../../models/apartment/apartmentReturnModel';
import { OwnerService } from '../../services/owner.service';
import { ApartmentService } from '../../services/apartment.service';
import { ApartmentPutModel } from '../../models/apartment/apartmentPutModel';

@Component({
  selector: 'app-edit-apartment-modal',
  templateUrl: './edit-apartment-modal.component.html',
  styleUrls: ['./edit-apartment-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class EditApartmentModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  apartmentForm: FormGroup;
  owners: OwnerReturnModel[] = [];
  apartment!: ApartmentReturnModel;

  constructor(
    private fb: FormBuilder,
    private apartmentService: ApartmentService,
    private ownerService: OwnerService
  ) {
    this.apartmentForm = this.fb.group({
      ownerId: ['', Validators.required],
    });
  }

  open(apartment: ApartmentReturnModel) {
    this.isVisible = true;
    this.apartment = apartment;
    this.apartmentForm.patchValue({
      ownerId: apartment.ownerId ? apartment.ownerId : null
    });
    this.loadOwners();
  }

  close() {
    this.isVisible = false;
    this.apartmentForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.apartmentForm.valid) {
      const formValue = this.apartmentForm.value;
      const updatedApartment: ApartmentPutModel = {
        ownerId: formValue.ownerId || null
      };
      this.apartmentService.updateApartment(this.apartment.id, updatedApartment).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error updating apartment:', error);
        }
      );
    }
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
}

