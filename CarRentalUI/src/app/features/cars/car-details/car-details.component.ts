import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { CarService } from '../../../core/services/car.service';
import { AuthService } from '../../../core/services/auth.service';
import { Car, CarType, CarTypeLabels } from '../../../core/models';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';
import { BookCarDialogComponent } from '../../reservations/book-car-dialog/book-car-dialog.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-car-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    LoadingComponent
  ],
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.scss']
})
export class CarDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private carService = inject(CarService);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);

  car = signal<Car | null>(null);
  isLoading = signal(true);

  // Default placeholder image
  private readonly placeholderImage = 'https://via.placeholder.com/600x400?text=No+Image';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadCar(id);
    }
  }

  loadCar(id: string): void {
    this.carService.getCarById(id).subscribe({
      next: (car) => {
        this.car.set(car);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.router.navigate(['/cars']);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/cars']);
  }

  bookCar(): void {
    const carData = this.car();
    if (!carData) return;

    if (this.authService.checkIsAuthenticated()) {
      // User is logged in, open booking dialog
      const dialogRef = this.dialog.open(BookCarDialogComponent, {
        data: carData,
        width: '750px',
        maxWidth: '95vw',
        maxHeight: '90vh',
        panelClass: 'book-car-dialog-panel'
      });

      // Reload car data if booking was successful
      dialogRef.afterClosed().subscribe(result => {
        if (result && carData.id) {
          this.loadCar(carData.id);
        }
      });
    } else {
      // User is not logged in, redirect to login with return URL
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: `/cars/${carData.id}` }
      });
    }
  }

  // Get car type label
  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  // Check if car is available
  isCarAvailable(car: Car | null): boolean {
    if (!car || !car.isActive) return false;

    const now = new Date();
    return !car.reservations?.some(reservation => {
      const start = new Date(reservation.startDateTime);
      const end = new Date(reservation.endDateTime);
      return now >= start && now <= end;
    });
  }

  getImageUrl(imageUrl?: string): string {
    if (!imageUrl) {
      return this.placeholderImage;
    }
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }
    if (imageUrl.startsWith('/')) {
      return `${environment.apiUrl.replace('/api', '')}${imageUrl}`;
    }
    return `${environment.apiUrl.replace('/api', '')}/${imageUrl}`;
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (img.src !== this.placeholderImage) {
      img.src = this.placeholderImage;
    }
  }
}
