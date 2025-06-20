import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SignalRService } from '../../core/services/signalr.service';
import { AuthService } from '../../core/services/auth.service';
import { RequestType } from '../../core/models/request-type.model';
import { CatalogService } from '../../core/services/catalog.service';
import * as bootstrap from 'bootstrap';

    interface ApplicationRequest {
    id: number;
    date: string;
    type: number;
    status: string;
    }

    @Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule, RouterModule, ReactiveFormsModule],
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
    })
    export class DashboardComponent implements OnInit {
    applicationRequests: ApplicationRequest[] = [];
    selectedIds = new Set<number>();
    requestForm: FormGroup;
    requestTypes: RequestType[] = [];

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        private router: Router,
        private signalRService: SignalRService,
        private authService: AuthService,
        private catalogService: CatalogService
    ) {
        this.requestForm = this.fb.group({
        typeId: [1, Validators.required],
        message: ['', Validators.required]
        });
    }

    ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.onApplicationRequestsUpdated((updatedRequests: ApplicationRequest[]) => {
        console.log('📡 Evento recibido vía SignalR:', updatedRequests);

        updatedRequests.forEach(updated => {
        const index = this.applicationRequests.findIndex(req => req.id === updated.id);
        if (index !== -1) {
            this.applicationRequests[index].status = updated.status;
            this.applicationRequests[index].date = updated.date;
        }
        });
    });
    this.loadCatalog();
    this.fetchRequests();
    }

  
  fetchRequests(): void {
    this.http.get<ApplicationRequest[]>('https://localhost:7269/api/Application/getApplicationDashboard')
      .subscribe({
        next: data => {
          console.log('Data received:', data);
          this.applicationRequests = data;
        },
        error: err => console.error('Fetch error', err)
      });
  }
  getTypeName(typeId: number): string {
  switch (typeId) {
    case 1: return 'Request';
    case 2: return 'Offer';
    case 3: return 'Complaint';
    default: return 'Unknown';
  }
}

  toggleSelection(id: number): void {
    this.selectedIds.has(id)
      ? this.selectedIds.delete(id)
      : this.selectedIds.add(id);
  }

  isAllSelected(): boolean {
    return this.selectedIds.size === this.applicationRequests.length;
  }

  toggleAllSelection(): void {
    if (this.isAllSelected()) {
      this.selectedIds.clear();
    } else {
      this.applicationRequests.forEach(req => this.selectedIds.add(req.id));
    }
  }
  openModal(): void {
    const modalElement = document.getElementById('newRequestModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  createRequest(): void {
    if (this.requestForm.valid) {
      const payload = this.requestForm.value;
      this.http.post('https://localhost:7269/api/Application/create', payload).subscribe({
        next: () => {
          this.fetchRequests();
          const modalEl = document.getElementById('newRequestModal');
          if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
        },
        error: err => console.error('Create error', err)
      });
    }
  }
    loadCatalog(): void {
    this.catalogService.getRequestTypes().subscribe({
      next: data => {
        this.requestTypes = data;
      },
      error: err => console.error('Error loading catalog:', err)
    });
  }

  deleteSelected(): void {
  const selectedIdsArray = Array.from(this.selectedIds);

  this.http.delete('https://localhost:7269/api/Application/deleteApplicationBatch', {
    body: selectedIdsArray
    }).subscribe({
    next: () => {
      // Quita del array los requests eliminados
      this.applicationRequests = this.applicationRequests.filter(
        req => !this.selectedIds.has(req.id)
      );
      this.selectedIds.clear(); // Limpia la selección
    },
    error: err => console.error('Delete error', err)
    });
  }
    logout(): void {
    this.authService.logout();
    }
}
