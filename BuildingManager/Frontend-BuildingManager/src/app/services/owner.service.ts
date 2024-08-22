import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { OwnerEndpoints } from '../networking/endpoints';
import { OwnerReturnModel } from '../models/owner/ownerReturnModel';
import { OwnerCreateModel } from '../models/owner/ownerCreateModel';


interface IOwnerService {
  getOwners(): Observable<OwnerReturnModel[]>
  addOwner(apartment: OwnerCreateModel): Observable<OwnerReturnModel>
  updateOwner(id: number, owner: OwnerCreateModel): Observable<OwnerReturnModel>
  deleteOwner(id: number): Observable<void>
}

@Injectable({
  providedIn: 'root'
})
export class OwnerService implements IOwnerService{

  constructor(private http: HttpClient) {}

  getOwners(): Observable<OwnerReturnModel[]> {
    return this.http.get<OwnerReturnModel[]>(OwnerEndpoints.OWNERS);
  }

  addOwner(owner: OwnerCreateModel): Observable<OwnerReturnModel> {
    return this.http.post<OwnerReturnModel>(OwnerEndpoints.OWNERS, owner);
  }

  updateOwner(id: number, owner: OwnerCreateModel): Observable<OwnerReturnModel> {
    return this.http.put<OwnerReturnModel>(`${OwnerEndpoints.OWNER}${id}`, owner);
  }

  deleteOwner(id: number): Observable<void> {
    return this.http.delete<void>(`${OwnerEndpoints.OWNER}${id}`);
  }
}

