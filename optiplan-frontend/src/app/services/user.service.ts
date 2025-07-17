import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  private apiUrl = 'https://localhost:7078/api/user'; // Adjust the API URL as needed
  constructor(private http:HttpClient) { }

  getCurrentUser(): Observable<User> {
    // This method should return the current user details
    // For example, it could make an HTTP GET request to your API endpoint
    return this.http.get<User>(`${this.apiUrl}/profile  `);

  }
}
