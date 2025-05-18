import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService 
{

  hubUrl = environment.hubUrl;
  hubConnection?: HubConnection;
  bookStatusUpdateSignal = signal<number | null>(null);     // this number will be book Id

  // establish signalR connection with API
  createHubConnection() 
  {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, 
        { withCredentials: true})
      .withAutomaticReconnect()
      .build();
    
    this.hubConnection.start().catch(error => console.log("Error SignalR - " + error));

    this.hubConnection.on('BookStatusUpdated', (bookId: number) => {
      console.log('Signal R Notification: Book status updated for book Id ' + bookId);
      this.bookStatusUpdateSignal.set(bookId);
    });
  }

  // disconnect signalR connection with API
  stopHubConnection()
  {
    if (this.hubConnection?.state === HubConnectionState.Connected)
    {
      this.hubConnection?.stop()
        .catch(error => 
              { 
                console.log('Signal R Hub connection stopping error ' + error);
              }
            ); 
    }
  }
}
