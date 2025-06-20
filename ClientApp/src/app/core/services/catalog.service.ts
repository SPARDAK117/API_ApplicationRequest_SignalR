import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestType } from '../models/request-type.model';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private readonly baseUrl = 'https://localhost:7269/api/Catalog';

  constructor(private http: HttpClient) {}

  getRequestTypes(): Observable<RequestType[]> {
    return this.http.get<RequestType[]>(`${this.baseUrl}/RequestTypeCatalog`);
  }
}
