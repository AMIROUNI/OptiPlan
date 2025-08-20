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
    if (this.connectionPromise) {
      return this.connectionPromise;
    }

    const url = 'https://localhost:7078/chathub';
    console.log('Connecting to SignalR at:', url);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () => token,
        skipNegotiation: false, // Ensure negotiation happens
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Debug) // Enable detailed logging
      .build();

    // Setup connection event handlers
    this.setupConnectionEvents();

    this.connectionPromise = this.hubConnection.start()
      .then(() => {
        this.isConnected = true;
        console.log('SignalR connected successfully');
        this.connectionStatus.next(true);
      })
      .catch(err => {
        console.error('Connection error details:', err);
        this.isConnected = false;
        this.connectionStatus.next(false);
        this.connectionPromise = null;
        throw err;
      });

    return this.connectionPromise;
  }

  private setupConnectionEvents() {
    this.hubConnection.onclose((error) => {
      console.log('SignalR connection closed', error);
      this.isConnected = false;
      this.connectionStatus.next(false);
    });

    this.hubConnection.onreconnecting((error) => {
      console.log('SignalR reconnecting...', error);
      this.isConnected = false;
      this.connectionStatus.next(false);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('SignalR reconnected', connectionId);
      this.isConnected = true;
      this.connectionStatus.next(true);
    });
  }

  async joinChat(receiverUsername: string): Promise<void> {
    if (!this.isConnected) {
      await this.waitForConnection();
    }
    
    console.log('Joining chat for user:', receiverUsername);
    try {
      await this.hubConnection.invoke('JoinChat', receiverUsername);
      console.log('Successfully joined chat');
    } catch (err) {
      console.error('Error joining chat:', err);
      throw err;
    }
  }

  async sendMessage(receiverUsername: string, content: string): Promise<void> {
    if (!this.isConnected) {
      await this.waitForConnection();
    }
    
    console.log('Sending message to:', receiverUsername);
    try {
      await this.hubConnection.invoke('SendMessage', receiverUsername, content);
      console.log('Message sent successfully');
    } catch (err) {
      console.error('Error sending message:', err);
      throw err;
    }
  }

  onMessageReceived(callback: (msg: Message) => void) {
    if (!this.hubConnection) {
      console.error('Hub connection not initialized');
      return;
    }
    
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

  private async waitForConnection(timeoutMs: number = 10000): Promise<void> {
    if (this.isConnected) return;
    
    const startTime = Date.now();
    
    return new Promise((resolve, reject) => {
      const checkConnection = () => {
        if (this.isConnected) {
          resolve();
        } else if (Date.now() - startTime > timeoutMs) {
          reject(new Error('Connection timeout: SignalR connection not established'));
        } else {
          setTimeout(checkConnection, 100);
        }
      };
      
      checkConnection();
    });
  }
}