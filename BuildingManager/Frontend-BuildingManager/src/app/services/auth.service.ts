import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginReturnModel } from '../models/loginReturnModel';
import { SessionEndpoints } from '../networking/endpoints';
import { SessionStorageService } from './session-storage.service';

interface IAuthService {
  login(email: string, password: string): Observable<LoginReturnModel>;
  logout() : void;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService implements IAuthService {

  constructor(private _httpClient: HttpClient, private _sessionStorage: SessionStorageService) { }

  login(email: string, password: string): Observable<LoginReturnModel> {
    return this._httpClient.post<LoginReturnModel>(
      SessionEndpoints.LOGIN, 
      { email, password }
    );
  }

  logout() {
    return this._httpClient.delete(`${SessionEndpoints.LOGOUT}${this._sessionStorage.getSessionId()}`);
  }
}
