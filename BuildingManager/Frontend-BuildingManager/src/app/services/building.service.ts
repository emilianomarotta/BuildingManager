import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BuildingEndpoints } from '../networking/endpoints';
import { BuildingReturnModel } from '../models/building/buildingReturnModel';
import { BuildingCreateModel } from '../models/building/buildingCreateModel';
import { BuildingPutModel } from '../models/building/buildingPutModel';


interface IBuildingService {
  getBuildings(): Observable<BuildingReturnModel[]>
  addBuilding(building: BuildingCreateModel): Observable<BuildingCreateModel>
}

@Injectable({
  providedIn: 'root'
})
export class BuildingService implements IBuildingService{

  constructor(private http: HttpClient) {}

  getBuildings(): Observable<BuildingReturnModel[]> {
    return this.http.get<BuildingReturnModel[]>(BuildingEndpoints.BUILDINGS);
  }

  addBuilding(building: BuildingCreateModel): Observable<BuildingCreateModel> {
    return this.http.post<BuildingCreateModel>(BuildingEndpoints.BUILDINGS, building);
  }

  updateBuilding(id: number, building: BuildingPutModel): Observable<BuildingReturnModel> {
    return this.http.put<BuildingReturnModel>(`${BuildingEndpoints.BUILDING}${id}`, building);
  }

  deleteBuilding(id: number): Observable<void> {
    return this.http.delete<void>(`${BuildingEndpoints.BUILDING}${id}`);
  }
}
