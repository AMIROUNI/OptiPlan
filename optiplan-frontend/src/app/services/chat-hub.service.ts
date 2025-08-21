import { Injectable } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { Message } from "../models/message";
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatHubService {
  private hubConnection!: signalR.HubConnection;
  isConnected = false;
  private connectionPromise: Promise<void> | null = null;
  private connectionStatus = new Subject<boolean>();

  connectionStatus$ = this.connectionStatus.asObservable();

  connect(token: string): Promise<void> {
    if (this.connectionPromise) return this.connectionPromise;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7078/chathub', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.setupConnectionEvents();

    this.connectionPromise = this.hubConnection.start()
      .then(() => {
        this.isConnected = true;
        this.connectionStatus.next(true);
      })
      .catch(err => {
        this.isConnected = false;
        this.connectionStatus.next(false);
        this.connectionPromise = null;
        throw err;
      });

    return this.connectionPromise;
  }

  private setupConnectionEvents() {
    this.hubConnection.onclose(() => {
      this.isConnected = false;
      this.connectionStatus.next(false);
    });

    this.hubConnection.onreconnecting(() => {
      this.isConnected = false;
      this.connectionStatus.next(false);
    });

    this.hubConnection.onreconnected(() => {
      this.isConnected = true;
      this.connectionStatus.next(true);
    });
  }

  async joinChat(receiverUsername: string): Promise<void> {
    await this.waitForConnection();
    await this.hubConnection.invoke('JoinChat', receiverUsername);
  }

  async sendMessage(receiverUsername: string, content: string): Promise<void> {
    await this.waitForConnection();
    await this.hubConnection.invoke('SendMessage', receiverUsername, content);
  }

  onMessageReceived(callback: (msg: Message) => void) {
    if (!this.hubConnection) return;
    this.hubConnection.on('ReceiveMessage', callback);
  }

  disconnect() {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.isConnected = false;
      this.connectionPromise = null;
      this.connectionStatus.next(false);
    }
  }

  private async waitForConnection(timeoutMs = 10000): Promise<void> {
    if (this.isConnected) return;
    const start = Date.now();

    return new Promise((resolve, reject) => {
      const check = () => {
        if (this.isConnected) resolve();
        else if (Date.now() - start > timeoutMs) reject(new Error('SignalR connection timeout'));
        else setTimeout(check, 100);
      };
      check();
    });
  }
}
