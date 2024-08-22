import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OwnerService } from '../../services/owner.service';
import { OwnerCreateModel } from '../../models/owner/ownerCreateModel';


@Component({
  selector: 'app-add-owner-modal',
  templateUrl: './add-owner-modal.component.html',
  styleUrls: ['./add-owner-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddOwnerModalComponent {
  @Output() ownerAdded = new EventEmitter<void>();
  ownerForm: FormGroup;
  isVisible = false;

  constructor(
    private fb: FormBuilder,
    private ownerService: OwnerService,

  ) {
    this.ownerForm = this.fb.group({
      name: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.ownerForm.reset();
  }

  submit() {
    if (this.ownerForm.valid) {
      const formValue = this.ownerForm.value;
      const newOwner: OwnerCreateModel = {
        ...formValue
      };
      this.ownerService.addOwner(newOwner).subscribe(
        () => {
          this.close();
          this.ownerAdded.emit();
          this.ownerForm.reset();
        },
        error => {
          console.error('Error adding Owner:', error);
        }
      );
    }
  }
}

