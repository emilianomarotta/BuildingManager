import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserCreateModel } from '../../models/user/userCreateModel';

@Component({
  selector: 'app-add-staff-modal',
  templateUrl: './add-staff-modal.component.html',
  styleUrls: ['./add-staff-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddStaffModalComponent {
  @Output() staffAdded = new EventEmitter<void>();
  staffForm: FormGroup;
  isVisible = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService
  ) {
    this.staffForm = this.fb.group({
      name: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.staffForm.reset();
  }

  submit() {
    if (this.staffForm.valid) {
      const formValue = this.staffForm.value;
      const newStaff: UserCreateModel = {
        ...formValue
      };
      this.userService.addStaff(newStaff).subscribe(
        () => {
          this.close();
          this.staffAdded.emit();
          this.staffForm.reset();
        },
        error => {
          console.error('Error adding staff:', error);
        }
      );
    }
  }
}

