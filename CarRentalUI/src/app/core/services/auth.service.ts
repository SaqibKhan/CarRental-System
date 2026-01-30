import { Injectable, signal, computed, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, RegisterRequest, AuthResponse } from '../models';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly USER_KEY = 'user';
  private readonly SESSION_EXPIRY_KEY = 'session_expiry';
  private readonly SESSION_DURATION_MS = 20 * 60 * 1000; // 20 minutes in milliseconds

  private currentUserSignal = signal<User | null>(this.getStoredUser());
  private sessionCheckInterval: ReturnType<typeof setInterval> | null = null;
  private activityListenerBound = false;

  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isAuthenticated = computed(() => !!this.currentUserSignal() && !!this.getToken() && !this.isSessionExpired());
  readonly isAdmin = computed(() => this.currentUserSignal()?.role === 'Admin');

  // Method to check authentication status directly (more reliable than computed in some cases)
  checkIsAuthenticated(): boolean {
    const token = localStorage.getItem(this.TOKEN_KEY);
    const userJson = localStorage.getItem(this.USER_KEY);

    if (!token || !userJson) {
      return false;
    }

    // Check session expiry
    const expiryTime = localStorage.getItem(this.SESSION_EXPIRY_KEY);
    if (expiryTime && Date.now() > parseInt(expiryTime, 10)) {
      // Session expired
      return false;
    }

    // If no expiry set but we have token and user, consider valid (extend session)
    if (!expiryTime) {
      this.extendSession();
    }

    return true;
  }

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Initialize session management if user is already logged in
    const token = localStorage.getItem(this.TOKEN_KEY);
    const userJson = localStorage.getItem(this.USER_KEY);
    const hasToken = !!token;
    const hasUser = !!userJson;

    if (hasToken && hasUser) {
      // User has valid auth data in localStorage
      const expiryTime = localStorage.getItem(this.SESSION_EXPIRY_KEY);

      if (!expiryTime) {
        // No expiry set (legacy session or first load after login)
        // Set expiry now and continue as valid session
        this.extendSession();
        this.startSessionManagement();
      } else if (Date.now() <= parseInt(expiryTime, 10)) {
        // Session is still valid, extend it on page load
        this.extendSession();
        this.startSessionManagement();
      } else {
        // Session has expired, clean up but don't redirect (let guards handle that)
        this.clearAuthData();
      }
    } else if (hasToken || hasUser) {
      // Partial auth data (corrupted state), clean up
      this.clearAuthData();
    }
  }

  ngOnDestroy(): void {
    this.stopSessionManagement();
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, request)
      .pipe(
        tap(response => this.handleAuthResponse(response)),
        catchError(error => throwError(() => error))
      );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, request)
      .pipe(
        tap(response => this.handleAuthResponse(response)),
        catchError(error => throwError(() => error))
      );
  }

  logout(): void {
    this.clearAuthData();
    this.router.navigate(['/auth/login']);
  }

  // Clear auth data without redirecting (used internally)
  private clearAuthData(): void {
    this.stopSessionManagement();
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    localStorage.removeItem(this.SESSION_EXPIRY_KEY);
    this.currentUserSignal.set(null);
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, { refreshToken })
      .pipe(
        tap(response => this.handleAuthResponse(response)),
        catchError(error => {
          this.logout();
          return throwError(() => error);
        })
      );
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  private handleAuthResponse(response: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, response.token);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
    localStorage.setItem(this.USER_KEY, JSON.stringify(response.user));
    this.currentUserSignal.set(response.user);

    // Set session expiry and start session management
    this.extendSession();
    this.startSessionManagement();
  }

  private getStoredUser(): User | null {
    try {
      const token = localStorage.getItem(this.TOKEN_KEY);
      const userJson = localStorage.getItem(this.USER_KEY);

      // If either token or user is missing, just return null
      // Don't clear data here - let the constructor handle cleanup decisions
      if (!token || !userJson) {
        return null;
      }

      const user = JSON.parse(userJson) as User | null;

      // Basic shape check to guard against corrupted values
      if (!user || typeof user !== 'object') {
        return null;
      }

      return user;
    } catch {
      // Storage contained invalid JSON – just return null
      return null;
    }
  }

  // Session management methods
  private isSessionExpired(): boolean {
    const expiryTime = localStorage.getItem(this.SESSION_EXPIRY_KEY);
    if (!expiryTime) {
      // No expiry set - if we have a token, consider it valid (will be set on next activity)
      return !this.getToken();
    }
    return Date.now() > parseInt(expiryTime, 10);
  }

  private extendSession(): void {
    const newExpiry = Date.now() + this.SESSION_DURATION_MS;
    localStorage.setItem(this.SESSION_EXPIRY_KEY, newExpiry.toString());
  }

  private startSessionManagement(): void {
    // Set up activity listeners to extend session on user activity
    if (!this.activityListenerBound) {
      this.bindActivityListeners();
      this.activityListenerBound = true;
    }

    // Check session expiry periodically (every minute)
    if (!this.sessionCheckInterval) {
      this.sessionCheckInterval = setInterval(() => {
        if (this.isSessionExpired()) {
          this.logout();
        }
      }, 60 * 1000); // Check every minute
    }
  }

  private stopSessionManagement(): void {
    if (this.sessionCheckInterval) {
      clearInterval(this.sessionCheckInterval);
      this.sessionCheckInterval = null;
    }
    if (this.activityListenerBound) {
      this.unbindActivityListeners();
      this.activityListenerBound = false;
    }
  }

  private onUserActivity = (): void => {
    // Only extend session if user is still authenticated
    if (this.getToken() && !this.isSessionExpired()) {
      this.extendSession();
    }
  };

  private bindActivityListeners(): void {
    // Listen for user activity events to keep session alive
    document.addEventListener('click', this.onUserActivity);
    document.addEventListener('keypress', this.onUserActivity);
    document.addEventListener('scroll', this.onUserActivity);
    document.addEventListener('mousemove', this.throttle(this.onUserActivity, 30000)); // Throttle mousemove to every 30 seconds
  }

  private unbindActivityListeners(): void {
    document.removeEventListener('click', this.onUserActivity);
    document.removeEventListener('keypress', this.onUserActivity);
    document.removeEventListener('scroll', this.onUserActivity);
    document.removeEventListener('mousemove', this.throttle(this.onUserActivity, 30000));
  }

  private throttle(func: () => void, limit: number): () => void {
    let inThrottle = false;
    return () => {
      if (!inThrottle) {
        func();
        inThrottle = true;
        setTimeout(() => inThrottle = false, limit);
      }
    };
  }
}
