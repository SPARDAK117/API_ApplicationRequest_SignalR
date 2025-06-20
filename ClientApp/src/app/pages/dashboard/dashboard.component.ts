import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApplicationRequest } from '../../core/models/application-request.model';
import { RequestType } from '../../core/models/request-type.model';
import { SignalRService } from '../../core/services/signalr.service';
import { AuthService } from '../../core/services/auth.service';
import { CatalogService } from '../../core/services/catalog.service';
import { ApplicationRequestService } from '../../core/services/application-request.service';
import * as bootstrap from 'bootstrap';

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
  requestTypes: RequestType[] = [];
  requestForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private signalRService: SignalRService,
    private catalogService: CatalogService,
    private applicationService: ApplicationRequestService
  ) {
    this.requestForm = this.fb.group({
      typeId: [1, Validators.required],
      message: ['', Validators.required]
    });
  }

ngOnInit(): void {
  this.signalRService.startConnection();
  this.setupSignalRUpdates();

  this.catalogService.getRequestTypes().subscribe({
    next: types => {
      this.requestTypes = types;

      this.loadRequests();
    },
    error: err => console.error('Error loading catalog', err)
  });   
}

private setupSignalRUpdates(): void {
  this.signalRService.onApplicationRequestsUpdated((updatedRequests: ApplicationRequest[]) => {
    updatedRequests.forEach(updated => {
      const index = this.applicationRequests.findIndex(req => req.id === updated.id);
      if (index !== -1) {
        this.applicationRequests[index] = { ...this.applicationRequests[index], ...updated };
      }
    });
  });
}



  private loadRequests(): void {
    this.applicationService.getAll().subscribe({
      next: data => this.applicationRequests = data,
      error: err => console.error('Error loading requests', err)
    });
  }

  private loadCatalog(): void {
    this.catalogService.getRequestTypes().subscribe({
      next: data => this.requestTypes = data,
      error: err => console.error('Error loading catalog', err)
    });
  }

  createRequest(): void {
    if (this.requestForm.valid) {
      const payload = this.requestForm.value;
      this.applicationService.create(payload).subscribe({
        next: () => {
          this.loadRequests();
          const modalEl = document.getElementById('newRequestModal');
          if (modalEl) bootstrap.Modal.getInstance(modalEl)?.hide();
        },
        error: err => console.error('Create error', err)
      });
    }
  }

  deleteSelected(): void {
    const ids = Array.from(this.selectedIds);
    this.applicationService.delete(ids).subscribe({
      next: () => {
        this.applicationRequests = this.applicationRequests.filter(req => !this.selectedIds.has(req.id));
        this.selectedIds.clear();
      },
      error: err => console.error('Delete error', err)
    });
  }

  getTypeName(type: number): string {
    const match = this.requestTypes.find(t => t.id === type);
    return match?.name || 'Unknown';
  }

  toggleSelection(id: number): void {
    this.selectedIds.has(id) ? this.selectedIds.delete(id) : this.selectedIds.add(id);
  }

  isAllSelected(): boolean {
    return this.selectedIds.size === this.applicationRequests.length;
  }

  toggleAllSelection(): void {
    this.isAllSelected()
      ? this.selectedIds.clear()
      : this.applicationRequests.forEach(req => this.selectedIds.add(req.id));
  }

  openModal(): void {
    const modalElement = document.getElementById('newRequestModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
