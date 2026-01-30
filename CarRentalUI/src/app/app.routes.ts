import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'cars',
    loadChildren: () => import('./features/cars/cars.routes').then(m => m.CARS_ROUTES)
  },
  {
    path: 'reservations',
    loadChildren: () => import('./features/reservations/reservations.routes').then(m => m.RESERVATIONS_ROUTES)
  },
  {
    path: 'admin/cars',
    loadChildren: () => import('./features/cars/cars.routes').then(m => m.ADMIN_CARS_ROUTES)
  },
  {
    path: 'admin/reservations',
    loadChildren: () => import('./features/reservations/reservations.routes').then(m => m.ADMIN_RESERVATIONS_ROUTES)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
