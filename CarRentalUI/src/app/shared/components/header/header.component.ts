import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule
  ],
  template: `
    <mat-toolbar color="primary" class="app-toolbar">
      <a routerLink="/" class="logo">
        <i class="bi bi-car-front-fill"></i>
        <span class="logo-text">CarRental</span>
      </a>

      <span class="spacer"></span>

      <nav class="nav-links">
        <a mat-button routerLink="/cars" routerLinkActive="active" class="nav-item">
          <i class="bi bi-grid-3x3-gap-fill"></i>
          <span>Cars</span>
        </a>

        @if (authService.isAuthenticated()) {
          <a mat-button routerLink="/reservations" routerLinkActive="active" class="nav-item">
            <i class="bi bi-calendar-check"></i>
            <span>My Reservations</span>
          </a>

          @if (authService.isAdmin()) {
            <button mat-button [matMenuTriggerFor]="adminMenu" class="nav-item">
              <i class="bi bi-shield-lock-fill"></i>
              <span>Admin</span>
              <i class="bi bi-chevron-down"></i>
            </button>
            <mat-menu #adminMenu="matMenu" class="dropdown-menu">
              <a mat-menu-item routerLink="/admin/cars">
                <i class="bi bi-car-front me-2"></i> Manage Cars
              </a>
              <a mat-menu-item routerLink="/admin/reservations">
                <i class="bi bi-list-check me-2"></i> All Reservations
              </a>
            </mat-menu>
          }

          <!-- Professional User Menu Button -->
          <button mat-button [matMenuTriggerFor]="userMenu" class="user-profile-btn">
            <div class="user-avatar">
              {{ userInitials() }}
            </div>
            <div class="user-info">
              <span class="user-name">{{ authService.currentUser()?.firstName }} {{ authService.currentUser()?.lastName }}</span>
              <span class="user-role">{{ authService.currentUser()?.role }}</span>
            </div>
            <i class="bi bi-chevron-down dropdown-arrow"></i>
          </button>
          <mat-menu #userMenu="matMenu" class="user-dropdown-menu" xPosition="before">
            <div class="user-menu-header">
              <div class="user-avatar-large">
                {{ userInitials() }}
              </div>
              <div class="user-details">
                <span class="user-fullname">{{ authService.currentUser()?.firstName }} {{ authService.currentUser()?.lastName }}</span>
                <span class="user-email">{{ authService.currentUser()?.email }}</span>
              </div>
            </div>
            <mat-divider></mat-divider>
            <a mat-menu-item routerLink="/reservations" class="menu-item">
              <i class="bi bi-calendar-check me-2"></i> My Reservations
            </a>
            <mat-divider></mat-divider>
            <button mat-menu-item (click)="authService.logout()" class="menu-item logout-item">
              <i class="bi bi-box-arrow-right me-2"></i> Sign Out
            </button>
          </mat-menu>
        } @else {
          <a mat-button routerLink="/auth/login" routerLinkActive="active" class="nav-item login-btn">
            <i class="bi bi-box-arrow-in-right"></i>
            <span>Sign In</span>
          </a>
          <a mat-raised-button routerLink="/auth/register" class="register-btn">
            <i class="bi bi-person-plus-fill"></i>
            <span>Get Started</span>
          </a>
        }
      </nav>
    </mat-toolbar>
  `,
  styles: [`
    .app-toolbar {
      position: sticky;
      top: 0;
      z-index: 1000;
      padding: 0 24px;
      height: 72px;
      background: linear-gradient(135deg, #475569 0%, #334155 50%, #1e293b 100%) !important;
    }

    .logo {
      display: flex;
      align-items: center;
      gap: 10px;
      text-decoration: none;
      color: inherit;
      font-size: 1.4rem;
      font-weight: 700;
    }

    .logo i {
      font-size: 1.6rem;
    }

    .logo-text {
      background: linear-gradient(90deg, #fff 0%, #e0e7ff 100%);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
    }

    .spacer {
      flex: 1;
    }

    .nav-links {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .nav-item {
      display: flex;
      align-items: center;
      gap: 6px;
      font-weight: 500;
      border-radius: 999px !important;
      padding: 0 16px;
      transition: background 0.2s ease;
      color: white !important;
    }

    .nav-item i {
      font-size: 1.1rem;
    }

    .nav-item.active,
    .nav-item:hover {
      background: rgba(255, 255, 255, 0.2);
    }

    .login-btn {
      border: 1px solid rgba(255, 255, 255, 0.4) !important;
    }

    .login-btn:hover {
      background: rgba(255, 255, 255, 0.15) !important;
      border-color: rgba(255, 255, 255, 0.6) !important;
    }

    /* Professional User Profile Button */
    .user-profile-btn {
      display: flex;
      align-items: center;
      gap: 10px;
      padding: 6px 12px 6px 6px !important;
      border-radius: 50px !important;
      background: rgba(255, 255, 255, 0.1) !important;
      border: 1px solid rgba(255, 255, 255, 0.2) !important;
      transition: all 0.2s ease;
      height: auto !important;
      min-height: 48px;
    }

    .user-profile-btn:hover {
      background: rgba(255, 255, 255, 0.2) !important;
      border-color: rgba(255, 255, 255, 0.4) !important;
    }

    .user-avatar {
      width: 36px;
      height: 36px;
      border-radius: 50%;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 600;
      font-size: 0.85rem;
      color: white;
      text-transform: uppercase;
      flex-shrink: 0;
    }

    .user-info {
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      line-height: 1.2;
    }

    .user-name {
      font-weight: 600;
      font-size: 0.9rem;
      color: white;
    }

    .user-role {
      font-size: 0.7rem;
      color: rgba(255, 255, 255, 0.7);
      text-transform: capitalize;
    }

    .dropdown-arrow {
      font-size: 0.75rem;
      color: rgba(255, 255, 255, 0.7);
      margin-left: 4px;
    }

    .register-btn {
      margin-left: 8px;
      background: linear-gradient(135deg, #f97316 0%, #ea580c 100%) !important;
      color: white !important;
      font-weight: 600;
      border-radius: 999px !important;
      padding: 0 20px;
      box-shadow: 0 4px 14px rgba(249, 115, 22, 0.4);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .register-btn:hover {
      transform: translateY(-2px);
      box-shadow: 0 6px 20px rgba(249, 115, 22, 0.5);
      background: linear-gradient(135deg, #fb923c 0%, #f97316 100%) !important;
    }

    .register-btn i {
      margin-right: 6px;
    }

    .me-2 {
      margin-right: 8px;
    }

    @media (max-width: 768px) {
      .nav-item span,
      .register-btn span {
        display: none;
      }

      .nav-item {
        padding: 0 12px;
      }

      .logo-text {
        display: none;
      }

      .user-info {
        display: none;
      }

      .user-profile-btn {
        padding: 6px !important;
        border-radius: 50% !important;
      }

      .dropdown-arrow {
        display: none;
      }
    }

    /* User Menu Dropdown Styles */
    ::ng-deep .user-dropdown-menu {
      min-width: 280px !important;
      border-radius: 16px !important;
      overflow: hidden;
      box-shadow: 0 10px 40px rgba(0, 0, 0, 0.15) !important;
    }

    ::ng-deep .user-menu-header {
      display: flex;
      align-items: center;
      gap: 14px;
      padding: 20px;
      background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
    }

    ::ng-deep .user-avatar-large {
      width: 50px;
      height: 50px;
      border-radius: 50%;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 700;
      font-size: 1.1rem;
      color: white;
      text-transform: uppercase;
      flex-shrink: 0;
      box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
    }

    ::ng-deep .user-details {
      display: flex;
      flex-direction: column;
    }

    ::ng-deep .user-fullname {
      font-weight: 700;
      font-size: 1rem;
      color: #1e293b;
    }

    ::ng-deep .user-email {
      font-size: 0.85rem;
      color: #64748b;
    }

    ::ng-deep .menu-item {
      padding: 12px 20px !important;
      font-size: 0.95rem;
    }

    ::ng-deep .menu-item i {
      font-size: 1.1rem;
      color: #64748b;
    }

    ::ng-deep .logout-item {
      color: #ef4444 !important;
    }

    ::ng-deep .logout-item i {
      color: #ef4444 !important;
    }
  `]
})
export class HeaderComponent {
  authService = inject(AuthService);

  // Compute user initials for avatar
  userInitials = computed(() => {
    const user = this.authService.currentUser();
    if (!user) return '';
    const first = user.firstName?.charAt(0) || '';
    const last = user.lastName?.charAt(0) || '';
    return (first + last).toUpperCase();
  });
}

