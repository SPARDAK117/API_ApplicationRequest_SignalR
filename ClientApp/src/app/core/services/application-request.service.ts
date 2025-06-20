import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApplicationRequest } from "../models/application-request.model";
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApplicationRequestService {
  private baseUrl = `${environment.ApplicationApiBaseUrl}/Application`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApplicationRequest[]> {
    return this.http.get<ApplicationRequest[]>(`${this.baseUrl}/getApplicationDashboard`);
  }

  create(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/create`, payload);
  }

  delete(ids: number[]): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/deleteApplicationBatch`, {
      body: {ids}
    });
  }
}
