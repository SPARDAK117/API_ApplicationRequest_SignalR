import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestType } from '../models/request-type.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private baseUrl = `${environment.ApplicationApiBaseUrl}/Catalog`;

  constructor(private http: HttpClient) {}

  getRequestTypes(): Observable<RequestType[]> {
    return this.http.get<RequestType[]>(`${this.baseUrl}/RequestTypeCatalog`);
  }
}
