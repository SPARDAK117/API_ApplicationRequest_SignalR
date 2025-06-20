import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

interface ApplicationRequest {
  id: number;
  date: string;
  type: number;
  status: string;
}

@Injectable({
  providedIn: 'root'
})

export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7269/applicationRequestsHub',{
    accessTokenFactory: () => sessionStorage.getItem('token') || ''
  })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR conectado'))
      .catch(err => console.error('Error al conectar SignalR:', err));
  }

onApplicationRequestsUpdated(callback: (data: ApplicationRequest[]) => void): void {
  this.hubConnection.on('ApplicationRequestsUpdated', (data: ApplicationRequest[]) => {
    console.log('📡 Evento recibido vía SignalR:', data);
    callback(data);
  });
}
}
