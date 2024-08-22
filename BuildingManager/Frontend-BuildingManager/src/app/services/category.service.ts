import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEndpoints } from '../networking/endpoints';
import { HttpClient } from '@angular/common/http';
import { CategoryReturnModel } from '../models/category/categoryReturnModel';
import { CategoryCreateModel } from '../models/category/categoryCreateModel';


interface ICategoryService {
  getCategories(): Observable<CategoryReturnModel[]>;
  addCategory(category: CategoryCreateModel): Observable<CategoryCreateModel>;
  updateCategory(id: number, category: CategoryCreateModel): Observable<CategoryReturnModel>;
  deleteCategory(id: number): Observable<void>;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService implements ICategoryService{

  constructor(private http: HttpClient) {}

  getCategories(): Observable<CategoryReturnModel[]> {
    return this.http.get<CategoryReturnModel[]>(CategoryEndpoints.CATEGORIES);
  }

  addCategory(category: CategoryCreateModel): Observable<CategoryCreateModel> {
    return this.http.post<CategoryCreateModel>(CategoryEndpoints.CATEGORY, category);
  }

  updateCategory(id: number, category: CategoryCreateModel): Observable<CategoryReturnModel> {
    return this.http.put<CategoryReturnModel>(`${CategoryEndpoints.CATEGORY}${id}`, category);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${CategoryEndpoints.CATEGORY}${id}`);
  }
}
