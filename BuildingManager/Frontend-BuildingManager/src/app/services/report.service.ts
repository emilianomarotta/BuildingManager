import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ReportEndpoints } from '../networking/endpoints';
import { ReportStaffReturnModel } from '../models/report/ReportStaffReturnModel';
import { ReportBuildingsReturnModel } from '../models/report/ReportBuildingsReturnModel';

interface IReportService {
  getStaffReport(staffId: number): Observable<ReportStaffReturnModel[]>;
  getBuildingReport(buildingId: number): Observable<ReportBuildingsReturnModel[]>;
  getBuildingsReports(): Observable<ReportBuildingsReturnModel[]>;
  getStaffReports(): Observable<ReportStaffReturnModel[]>;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService implements IReportService {

  constructor(private http: HttpClient) {}

  getStaffReport(staffId: number): Observable<ReportStaffReturnModel[]> {
    const params = new HttpParams()
      .set('staffId', staffId)
    return this.http.get<ReportStaffReturnModel[]>(ReportEndpoints.REPORTS_STAFF, { params });
  }

  getBuildingReport(buildingId: number): Observable<ReportBuildingsReturnModel[]> {
    const params = new HttpParams()
      .set('buildingId', buildingId)
    return this.http.get<ReportBuildingsReturnModel[]>(ReportEndpoints.REPORTS_BUILDINGS, { params });
  }

  getBuildingsReports(): Observable<ReportBuildingsReturnModel[]> {
    return this.http.get<ReportBuildingsReturnModel[]>(ReportEndpoints.REPORTS_BUILDINGS);
  }

  getStaffReports(): Observable<ReportStaffReturnModel[]> {
    return this.http.get<ReportStaffReturnModel[]>(ReportEndpoints.REPORTS_STAFF);
  }
}
