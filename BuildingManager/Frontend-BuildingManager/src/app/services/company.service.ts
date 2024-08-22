import { Injectable } from '@angular/core';
import { CompanyReturnModel } from '../models/company/companyReturnModel';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CompanyEndpoints } from '../networking/endpoints';
import { CompanyCreateModel } from '../models/category/companyCreateModel';

interface ICompanyService {
  getCompanies(): Observable<CompanyReturnModel[]>;
  addCompany(company: CompanyCreateModel): Observable<CompanyReturnModel>;
  updateCompany(id: number, company: CompanyCreateModel): Observable<CompanyReturnModel>;
}

@Injectable({
  providedIn: 'root'
})
export class CompanyService implements ICompanyService {

  constructor(private http: HttpClient) {}

  getCompanies(): Observable<CompanyReturnModel[]> {
    return this.http.get<CompanyReturnModel[]>(CompanyEndpoints.COMPANIES);
  }

  addCompany(company: CompanyCreateModel): Observable<CompanyReturnModel> {
    return this.http.post<CompanyReturnModel>(CompanyEndpoints.COMPANIES, company);
  }

  updateCompany(id: number, company: CompanyCreateModel): Observable<CompanyReturnModel> {
    return this.http.put<CompanyReturnModel>(`${CompanyEndpoints.COMPANY}${id}`, company);
  }
  
  deleteCompany(id: number): Observable<void> {
    return this.http.delete<void>(`${CompanyEndpoints.COMPANY}${id}`);
  }
}
