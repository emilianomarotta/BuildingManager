import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { UserReturnModel } from '../models/user/userReturnModel';
import { SessionStorageService } from '../services/session-storage.service';
import { UserCreateModel } from '../models/user/userCreateModel';
import { Observable } from 'rxjs';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidenavComponent } from '../home/sidenav/sidenav.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [HeaderComponent, SidenavComponent, CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  user!: UserReturnModel;
  isEditing = false;

  constructor(
    private fb: FormBuilder,
    private _sessionStorage: SessionStorageService,
    private _userService: UserService
  ) {
    this.profileForm = this.fb.group({
      name: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    const role = this._sessionStorage.getRole();
    let getUserObservable;

    if (role === 'Administrator') {
      getUserObservable = this._userService.getAdministrator();
    } else if (role === 'Manager') {
      getUserObservable = this._userService.getManager();
    } else if (role === 'Staff') {
      getUserObservable = this._userService.getStaff();
    }

    if (getUserObservable) {
      getUserObservable.subscribe(
        (data: UserReturnModel) => {
          this.user = data;
          this.profileForm.patchValue(data);
        },
        error => {
          console.error('There was an error!', error);
        }
      );
    }
  }

  enableEditing() {
    this.isEditing = true;
  }

  saveProfile(): void {
    if (this.profileForm.valid) {
      const updatedUser: UserCreateModel = this.profileForm.getRawValue();
      const role = this._sessionStorage.getRole();

      let updateObservable: Observable<UserReturnModel> | undefined;
      if (role === 'Administrator') {
        updateObservable = this._userService.updateAdministrator(updatedUser);
      } else if (role === 'Manager') {
        updateObservable = this._userService.updateManager(updatedUser);
      } else if (role === 'Staff') {
        updateObservable = this._userService.updateStaff(updatedUser);
      }

      if (updateObservable) {
        updateObservable.subscribe(
          (data: UserReturnModel) => {
            this.user = { ...this.user, ...data };
            this.isEditing = false;
            this.profileForm.patchValue({ ...data, password: '' });
          },
          error => {
            console.error('There was an error updating the user!', error);
          }
        );
      }
    }
  }

  cancelEditing() {
    this.isEditing = false;
    if (this.user) {
      this.profileForm.patchValue(this.user);
    }
  }
}
