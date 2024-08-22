import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserReturnModel } from '../models/user/userReturnModel';
import { AdministratorEndpoints, ManagerEndpoints, StaffEndpoints } from '../networking/endpoints';
import { UserCreateModel } from '../models/user/userCreateModel';
import { SessionStorageService } from './session-storage.service';

interface IUserService {
  getManager(id:number): Observable<UserReturnModel>
  getAdministrator(id:number): Observable<UserReturnModel>
  getStaff(id:number): Observable<UserReturnModel>
}

@Injectable({
  providedIn: 'root'
})

export class UserService implements IUserService {
  constructor(private http: HttpClient, private _sessionStorage: SessionStorageService) {}

  getUserId(): number | null {
    return this._sessionStorage.getId();
  }

  getAdministrator(): Observable<UserReturnModel> {
    return this.http.get<UserReturnModel>(`${AdministratorEndpoints.ADMINISTRATOR}${this. getUserId()}`);
  }

  getManager(): Observable<UserReturnModel> {
    return this.http.get<UserReturnModel>(`${ManagerEndpoints.MANAGER}${this. getUserId()}`);
  }

  getManagers(): Observable<UserReturnModel[]> {
    return this.http.get<UserReturnModel[]>(ManagerEndpoints.MANAGERS);
  }

  getStaffs(): Observable<UserReturnModel[]> {
    return this.http.get<UserReturnModel[]>(StaffEndpoints.STAFFS);
  }

  getStaff(): Observable<UserReturnModel> {
    return this.http.get<UserReturnModel>(`${StaffEndpoints.STAFF}${this. getUserId()}`);
  }

  updateAdministrator(updatedUser: UserCreateModel): Observable<UserReturnModel> {
    return this.http.put<UserReturnModel>(`${AdministratorEndpoints.ADMINISTRATOR}${this. getUserId()}`, updatedUser);
  }

  updateManager(updatedUser: UserCreateModel): Observable<UserReturnModel> {
    return this.http.put<UserReturnModel>(`${ManagerEndpoints.MANAGER}${this. getUserId()}`, updatedUser);
  }

  updateStaff(updatedUser: UserCreateModel): Observable<UserReturnModel> {
    return this.http.put<UserReturnModel>(`${StaffEndpoints.STAFF}${this. getUserId()}`, updatedUser);
  }

  addStaff(newStaff: UserCreateModel): Observable<UserReturnModel> {
    return this.http.post<UserReturnModel>(StaffEndpoints.STAFFS, newStaff);
  }

  deleteStaff(id: number): Observable<void> {
    return this.http.delete<void>(`${StaffEndpoints.STAFF}${id}`);
  }
}