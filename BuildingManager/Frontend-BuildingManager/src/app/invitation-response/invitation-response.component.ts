import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InvitationService } from '../services/invitation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { InvitationPutModel } from '../models/invitation/invitationPutModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invitation-response',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './invitation-response.component.html',
  styleUrl: './invitation-response.component.css'
})
export class InvitationResponseComponent implements OnInit {
  invitationForm: FormGroup;
  invitationId: string;
  
  constructor(
    private fb: FormBuilder,
    private invitationService: InvitationService,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) { 
    this.invitationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
    this.invitationId = '';
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.invitationId = params['id'];
      // Optionally, you could load invitation details using the ID
    });
  }

  respond(status: number) {
    if (this.invitationForm.valid) {
      const formValue = this.invitationForm.value;
      const response: InvitationPutModel = {
        email: formValue.email,
        password: formValue.password,
        status: status
      };

      this.invitationService.respondToInvitation(this.invitationId, response).subscribe(
        () => {
          alert('Response submitted successfully');
          this.router.navigate(['/']);
        },
        error => {
          console.error('Error responding to invitation:', error);
          alert('There was an error submitting your response.');
        }
      );
    }
  }

}
