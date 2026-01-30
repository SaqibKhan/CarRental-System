import { Routes } from '@angular/router';
import { authGuard, adminGuard } from '../../core/guards/auth.guard';

export const RESERVATIONS_ROUTES: Routes = [
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./my-reservations/my-reservations.component').then(m => m.MyReservationsComponent)
  }
];

export const ADMIN_RESERVATIONS_ROUTES: Routes = [
  {
    path: '',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./admin-reservations/admin-reservations.component').then(m => m.AdminReservationsComponent)
  }
];
