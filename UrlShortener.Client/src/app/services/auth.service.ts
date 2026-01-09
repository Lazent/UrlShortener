import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { User } from '../models/user';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7277/api/account';
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);

    // Восстанавливаем пользователя только в браузере
    let storedUser: User | null = null;
    if (this.isBrowser) {
      const userJson = localStorage.getItem('currentUser');
      if (userJson) {
        storedUser = JSON.parse(userJson);
      }
    }

    this.currentUserSubject = new BehaviorSubject<User | null>(storedUser);
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  public get isAuthenticated(): boolean {
    return this.currentUserValue !== null;
  }

  public get isAdmin(): boolean {
    return this.currentUserValue?.isAdmin ?? false;
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginRequest)
      .pipe(
        tap((response: LoginResponse) => {
          const user: User = {
            id: response.id,
            login: response.login,
            isAdmin: response.isAdmin
          };

          // Сохраняем только в браузере
          if (this.isBrowser) {
            localStorage.setItem('currentUser', JSON.stringify(user));
          }

          this.currentUserSubject.next(user);
        })
      );
  }
  register(request: { login: string; password: string; isAdmin: boolean }): Observable<any> {
    console.log('Sending registration request:', request);

    return this.http.post(`${this.apiUrl}/register`, request)
      .pipe(
        tap((response: any) => {
          console.log('Registration response:', response);
        })
      );
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem('currentUser');
    }
    this.currentUserSubject.next(null);
  }

}