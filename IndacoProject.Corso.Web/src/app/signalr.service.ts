import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection | undefined;
  hubHelloMessage: BehaviorSubject<string>;

  constructor(private config: ConfigService) {
    this.hubHelloMessage = new BehaviorSubject<string>("");
  }

  public initiateSignalrConnection(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${this.config["apiEndpoint"]}/notify`)
        .build();

      this.setSignalrClientMethods();

      this.connection
        .start()
        .then(() => {
          console.log(`SignalR connection success! connectionId: ${this.connection?.connectionId} `);
          resolve();
        })
        .catch((error) => {
          console.log(`SignalR connection error: ${error}`);
          reject();
        });
    });
  }
  private setSignalrClientMethods(): void {
    this.connection?.on('DisplayMessage', (message: string) => {
      this.hubHelloMessage.next(message);
    });
  }
}