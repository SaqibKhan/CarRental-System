import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { CarService } from '../../../core/services/car.service';
import { Car, CarType, CarTypeLabels } from '../../../core/models';
import { ToastService } from '../../../shared/services/toast.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-admin-car-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatChipsModule,
    LoadingComponent
  ],
  templateUrl: './admin-car-list.component.html',
  styleUrls: ['./admin-car-list.component.scss']
})
export class AdminCarListComponent implements OnInit {
  private carService = inject(CarService);
  private toastService = inject(ToastService);
  private dialog = inject(MatDialog);

  cars = signal<Car[]>([]);
  isLoading = signal(true);

  displayedColumns = ['image', 'carName', 'modelYear', 'carType', 'dailyPrice', 'numberPlate', 'status', 'actions'];

  ngOnInit(): void {
    this.loadCars();
  }

  loadCars(): void {
    this.carService.getCars().subscribe({
      next: (cars) => {
        this.cars.set(cars);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  // Get car type label
  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  deleteCar(car: Car): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Car',
        message: `Are you sure you want to delete ${car.carName}?`,
        confirmText: 'Delete',
        type: 'danger'
      } as ConfirmDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.carService.deleteCar(car.id).subscribe({
          next: () => {
            this.toastService.success('Car deleted successfully');
            this.loadCars();
          },
          error: () => {
            this.toastService.error('Failed to delete car');
          }
        });
      }
    });
  }

  // Default placeholder image
  private readonly placeholderImage = 'https://via.placeholder.com/100x70?text=No+Image';

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
