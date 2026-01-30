import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { Car, CarType, CarTypeLabels } from '../../../core/models';
import { AuthService } from '../../../core/services/auth.service';
import { BookCarDialogComponent } from '../../reservations/book-car-dialog/book-car-dialog.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-car-details-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './car-details-dialog.component.html',
  styleUrls: ['./car-details-dialog.component.scss']
})
export class CarDetailsDialogComponent {
  private dialogRef = inject(MatDialogRef<CarDetailsDialogComponent>);
  private router = inject(Router);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);

  car: Car = inject(MAT_DIALOG_DATA);

  close(): void {
    this.dialogRef.close();
  }

  bookCar(): void {
    this.dialogRef.close();
    if (this.authService.checkIsAuthenticated()) {
      // User is logged in, open booking dialog
      this.dialog.open(BookCarDialogComponent, {
        data: this.car,
        width: '750px',
        maxWidth: '95vw',
        maxHeight: '90vh',
        panelClass: 'book-car-dialog-panel'
      });
    } else {
      // User is not logged in, redirect to login with return URL
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: `/cars` }
      });
    }
  }

  // Get car type label
  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  // Check if car is available
  isCarAvailable(): boolean {
    if (!this.car.isActive) return false;

    const now = new Date();
    return !this.car.reservations?.some(reservation => {
      const start = new Date(reservation.startDateTime);
      const end = new Date(reservation.endDateTime);
      return now >= start && now <= end;
    });
  }

  // Default placeholder image
  private readonly placeholderImage = 'https://via.placeholder.com/600x400?text=No+Image';

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
