import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { ReservationService } from '../../../core/services/reservation.service';
import { ToastService } from '../../../shared/services/toast.service';
import { Car, CarType, CarTypeLabels } from '../../../core/models';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-book-car-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDividerModule
  ],
  templateUrl: './book-car-dialog.component.html',
  styleUrls: ['./book-car-dialog.component.scss']
})
export class BookCarDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<BookCarDialogComponent>);
  private reservationService = inject(ReservationService);
  private toastService = inject(ToastService);

  car: Car = inject(MAT_DIALOG_DATA);
  isLoading = signal(false);
  totalPrice = signal(0);

  minDate = new Date();

  bookingForm: FormGroup = this.fb.group({
    startDateTime: ['', Validators.required],
    numberOfDays: [1, [Validators.required, Validators.min(1)]]
  });

  // Default placeholder image
  private readonly placeholderImage = 'https://via.placeholder.com/600x400?text=No+Image';

  ngOnInit(): void {
    this.bookingForm.valueChanges.subscribe(() => {
      this.calculatePrice();
    });
    // Calculate initial price
    this.calculatePrice();
  }

  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  calculatePrice(): void {
    const { numberOfDays } = this.bookingForm.value;
    if (numberOfDays && this.car && numberOfDays > 0) {
      this.totalPrice.set(numberOfDays * this.car.dailyPrice);
    } else {
      this.totalPrice.set(0);
    }
  }

  getEndDate(): Date | null {
    const { startDateTime, numberOfDays } = this.bookingForm.value;
    if (startDateTime && numberOfDays > 0) {
      const endDate = new Date(startDateTime);
      endDate.setDate(endDate.getDate() + numberOfDays);
      return endDate;
    }
    return null;
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

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    const { startDateTime, numberOfDays } = this.bookingForm.value;

    this.reservationService.createReservation({
      carId: this.car.id,
      startDateTime: new Date(startDateTime).toISOString(),
      numberOfDays: numberOfDays
    }).subscribe({
      next: () => {
        this.toastService.success('Booking confirmed successfully!');
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isLoading.set(false);
        this.toastService.error(error.error?.message || 'Failed to create booking');
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
