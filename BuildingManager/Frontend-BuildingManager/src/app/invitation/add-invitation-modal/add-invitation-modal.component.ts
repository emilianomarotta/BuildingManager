import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RoleDescriptions } from '../../helpers/roles';
import { InvitationService } from '../../services/invitation.service';
import { InvitationCreateModel } from '../../models/invitation/invitationCreateModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-invitation-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-invitation-modal.component.html',
  styleUrl: './add-invitation-modal.component.css'
})
export class AddInvitationModalComponent implements OnInit {
  @Output() invitationAdded = new EventEmitter<void>();
  invitationForm: FormGroup;
  isVisible = false;
  roles = RoleDescriptions;

  constructor(
    private fb: FormBuilder,
    private invitationService: InvitationService
  ) {
    this.invitationForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      expiration: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  ngOnInit() {}

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.invitationForm.reset();
  }

  submit() {
    if (this.invitationForm.valid) {
      const formValue = this.invitationForm.value;
      const newInvitation: InvitationCreateModel = {
        ...formValue,
        role: parseInt(formValue.role, 10)
      };
      this.invitationService.addInvitation(newInvitation).subscribe(
        () => {
          this.close();
          this.invitationAdded.emit();
          this.invitationForm.reset();
        },
        error => {
          console.error('Error adding invitation:', error);
        }
      );
    }
  }
}
