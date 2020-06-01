import { Category } from './../models/category';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = 'https://localhost:5001/api/category/';
  constructor(private http: HttpClient) { }

    getCategories(): Observable<Category[]> {
      return this.http.get<Category[]>(this.baseUrl + 'all');
    }

    createCategory(category: Category) {
      return this.http.post(this.baseUrl + 'create', category);
    }
}
