import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { tap } from 'rxjs/operators';
import { Role } from '../models/enums/role';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7078/api/auth'; // Adjust the API URL as needed
  private tokenSubject = new BehaviorSubject<string | null>(null);
  private currentUserSubject = new BehaviorSubject<any>(null);

  constructor(private http: HttpClient, private router: Router) {
    this.initializeAuthState();
  }

  private initializeAuthState(): void {
    const token = localStorage.getItem('token');
    if (token && !this.isTokenExpired(token)) {
      this.tokenSubject.next(token);
      this.decodeAndSetUser(token);
    } else {
      this.clearAuthState();
    }
  }

  private isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      return decoded.exp < Date.now() / 1000;
    } catch {
      return true;
    }
  }

 register(registerData: FormData): Observable<any> {
  return this.http.post(`${this.apiUrl}/register`, registerData);
}


 login(credentials: { username: string, password: string }): Observable<{ token: string }> {
  return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials).pipe(
    tap(response => {
      console.log('Full login response:', response);
      console.log('Token type:', typeof response.token);
      console.log('Token value:', response.token);
    })
  );
}
// In your AuthService
saveToken(tokenResponse: any): void {
  // First extract the accessToken from the response
  const accessToken = tokenResponse?.accessToken;
  
  if (!accessToken) {
    console.error('No access token found in response:', tokenResponse);
    return;
  }

  console.log('Saving token:', accessToken.substring(0, 10) + '...');
  localStorage.setItem('token', accessToken);
  this.tokenSubject.next(accessToken);
  this.decodeAndSetUser(accessToken); // Now passing just the token string
}

decodeAndSetUser(token: string): void {
  try {
    console.log('Decoding token:', token.substring(0, 10) + '...');
    const decoded: any = jwtDecode(token);
    console.log('Decoded token payload:', decoded);
    this.currentUserSubject.next(decoded);
    
    // Log the role for debugging
    const role = decoded?.role || decoded?.roles?.[0];
    console.log('User role:', role);
  } catch (error) {
    console.error('Error decoding token:', error);
    this.clearAuthState();
  }
}

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  getCurrentUser(): any {
    return this.currentUserSubject.value;
  }

  getCurrentUsername(): string | null {
    const user = this.currentUserSubject.value;
    return user?.username || user?.sub || null;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired(token);
  }

  getRole(): Role | null {
    const user = this.currentUserSubject.value;
    if (!user) return null;
    
    // Handle different possible role representations
    if (user.role) {
      return user.role as any;
    }
    if (user.roles && user.roles.length > 0) {
      return user.roles[0] as any;
    }
    return  Role.User;  
  }

  redirectByRole(): void {
    const role = this.getRole();
    console.log("Current role:", role);

    switch (role) {
      case Role.Admin:
        this.router.navigate(['/admin-dashboard']);
        break;
      case Role.Moderator:
        this.router.navigate(['/moderator-dashboard']);
        break;
      case Role.User:
        this.router.navigate(['/user-dashboard']);
        break;
      default:
        this.router.navigate(['/login']);
        break;
    }
  }

  logout(): void {
    this.clearAuthState();
    this.router.navigate(['/login']);
  }

  private clearAuthState(): void {
    localStorage.removeItem('token');
    this.tokenSubject.next(null);
    this.currentUserSubject.next(null);
  }

 

  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }
}