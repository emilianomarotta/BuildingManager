import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionStorageService {
  private _sessionId = 'id'
  private _userTokenKey = 'userToken';
  private _role = 'role';
  private _email = 'email'
  private _id = 'userId';

  constructor() { }

  public getSessionId(): number | null {
    const sessionId = sessionStorage.getItem(this._sessionId);
    return sessionId ? parseInt(sessionId, 10) : null;
  }

  public setSessionId(_sessionId: number): void {
    sessionStorage.setItem(this._sessionId, _sessionId.toString());
  }

  public removeSessionId(): void {
    sessionStorage.removeItem(this._sessionId);
  }

  public getToken(): string | null {
    return sessionStorage.getItem(this._userTokenKey);
  }

  public setToken(token: string): void {
    sessionStorage.setItem(this._userTokenKey, token);
  }

  public removeToken(): void {
    sessionStorage.removeItem(this._userTokenKey);
  }

  public getRole(): string | null {
    return sessionStorage.getItem(this._role);
  }

  public setRole(role: string): void {
    sessionStorage.setItem(this._role, role);
  }
  
  public removeRole(): void {
    sessionStorage.removeItem(this._role);
  }

  public getId(): number | null {
    const id = sessionStorage.getItem(this._id);
    return id ? parseInt(id, 10) : null;
  }

  public setId(id: number): void {
    sessionStorage.setItem(this._id, id.toString());
  }

  public removeId(): void {
    sessionStorage.removeItem(this._id);
  }

  public getEmail(): string | null {
    return sessionStorage.getItem(this._email);
  }

  public setEmail(email: string): void {
    sessionStorage.setItem(this._email, email);
  }

  public removeEmail(): void {
    sessionStorage.removeItem(this._email);
  }

  public clear(): void {
    this.removeId();
    this.removeRole();
    this.removeToken();
    this.removeSessionId();
    this.removeEmail()
  }

}
