import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OwnerReturnModel } from '../../models/owner/ownerReturnModel';
import { OwnerCreateModel } from '../../models/owner/ownerCreateModel';
import { OwnerService } from '../../services/owner.service';

@Component({
  selector: 'app-edit-owner-modal',
  templateUrl: './edit-owner-modal.component.html',
  styleUrls: ['./edit-owner-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class EditOwnerModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  ownerForm: FormGroup;
  owner!: OwnerReturnModel;

  constructor(
    private fb: FormBuilder,
    private ownerService: OwnerService
  ) {
    this.ownerForm = this.fb.group({
      name: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required]
    });
  }

  open(owner: OwnerReturnModel) {
    this.isVisible = true;
    this.owner = owner;
    this.ownerForm.patchValue({
      name: this.owner.name,
      lastName: this.owner.lastName,
      email: this.owner.email
    });
  }

  close() {
    this.isVisible = false;
    this.ownerForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.ownerForm.valid) {
      const formValue = this.ownerForm.value;
      const updatedOwner: OwnerCreateModel = {
        name: formValue.name,
        lastName: formValue.lastName,
        email: formValue.email
      };
      this.ownerService.updateOwner(this.owner.id, updatedOwner).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error updating owner:', error);
        }
      );
    }
  }
}
