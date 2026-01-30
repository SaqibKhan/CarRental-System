import { Routes } from '@angular/router';
import { authGuard, adminGuard } from '../../core/guards/auth.guard';

export const CARS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./car-list/car-list.component').then(m => m.CarListComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./car-details/car-details.component').then(m => m.CarDetailsComponent)
  },
  {
    path: ':id/book',
    canActivate: [authGuard],
    loadComponent: () => import('../reservations/book-car/book-car.component').then(m => m.BookCarComponent)
  }
];

export const ADMIN_CARS_ROUTES: Routes = [
  {
    path: '',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./admin-car-list/admin-car-list.component').then(m => m.AdminCarListComponent)
  },
  {
    path: 'new',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./car-form/car-form.component').then(m => m.CarFormComponent)
  },
  {
    path: ':id/edit',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./car-form/car-form.component').then(m => m.CarFormComponent)
  }
];
