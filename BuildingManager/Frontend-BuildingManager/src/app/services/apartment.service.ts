import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApartmentEndpoints, BuildingEndpoints } from '../networking/endpoints';
import { ApartmentReturnModel } from '../models/apartment/apartmentReturnModel';
import { ApartmentCreateModel } from '../models/apartment/apartmentCreateModel';
import { ApartmentPutModel } from '../models/apartment/apartmentPutModel';


interface IApartmentService {
  getApartments(): Observable<ApartmentReturnModel[]>
  addApartment(apartment: ApartmentCreateModel): Observable<ApartmentReturnModel>
  updateApartment(id: number, apartment: ApartmentPutModel): Observable<ApartmentReturnModel>
  deleteApartment(id: number): Observable<void>
}

@Injectable({
  providedIn: 'root'
})
export class ApartmentService implements IApartmentService{

  constructor(private http: HttpClient) {}

  getApartments(): Observable<ApartmentReturnModel[]> {
    return this.http.get<ApartmentReturnModel[]>(ApartmentEndpoints.APARTMENTS);
  }

  addApartment(apartment: ApartmentCreateModel): Observable<ApartmentReturnModel> {
    return this.http.post<ApartmentReturnModel>(ApartmentEndpoints.APARTMENTS, apartment);
  }

  updateApartment(id: number, apartment: ApartmentPutModel): Observable<ApartmentReturnModel> {
    return this.http.put<ApartmentReturnModel>(`${ApartmentEndpoints.APARTMENT}${id}`, apartment);
  }

  deleteApartment(id: number): Observable<void> {
    return this.http.delete<void>(`${ApartmentEndpoints.APARTMENT}${id}`);
  }
}
