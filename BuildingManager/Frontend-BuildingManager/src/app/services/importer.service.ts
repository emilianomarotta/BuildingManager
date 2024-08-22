import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImporterEndpoints } from '../networking/endpoints';

@Injectable({
  providedIn: 'root'
})
export class ImporterService {

  constructor(private http: HttpClient) {}

  getImporters(): Observable<string[]> {
    return this.http.get<string[]>(ImporterEndpoints.IMPORTERS);
  }

  uploadFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${ImporterEndpoints.IMPORTERS}/upload`, formData);
  }

  importFile(importerName: string, datasourcePath: string): Observable<any> {
    const params = new HttpParams()
      .set('importerName', importerName)
      .set('datasourcePath', datasourcePath);
    return this.http.get(ImporterEndpoints.IMPORTER, { params });
  }
}
