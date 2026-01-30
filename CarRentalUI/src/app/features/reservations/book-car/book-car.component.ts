import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CarService } from '../../../core/services/car.service';
import { ReservationService } from '../../../core/services/reservation.service';
import { ToastService } from '../../../shared/services/toast.service';
import { Car, CarType, CarTypeLabels } from '../../../core/models';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-book-car',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    LoadingComponent
  ],
  templateUrl: './book-car.component.html',
  styleUrls: ['./book-car.component.scss']
})
export class BookCarComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private carService = inject(CarService);
  private reservationService = inject(ReservationService);
  private toastService = inject(ToastService);

  car = signal<Car | null>(null);
  isLoadingCar = signal(true);
  isLoading = signal(false);

  minDate = new Date();

  bookingForm: FormGroup = this.fb.group({
    startDateTime: ['', Validators.required],
    numberOfDays: [1, [Validators.required, Validators.min(1)]]
  });

  totalPrice = signal(0);

  ngOnInit(): void {
    const carId = this.route.snapshot.paramMap.get('id');
    if (carId) {
      this.loadCar(carId);
    }

    this.bookingForm.valueChanges.subscribe(() => {
      this.calculatePrice();
    });
  }

  loadCar(id: string): void {
    this.carService.getCarById(id).subscribe({
      next: (car) => {
        this.car.set(car);
        this.isLoadingCar.set(false);
      },
      error: () => {
        this.router.navigate(['/cars']);
      }
    });
  }

  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  calculatePrice(): void {
    const { numberOfDays } = this.bookingForm.value;
    if (numberOfDays && this.car() && numberOfDays > 0) {
      this.totalPrice.set(numberOfDays * this.car()!.dailyPrice);
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

  onSubmit(): void {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    const { startDateTime, numberOfDays } = this.bookingForm.value;

    this.reservationService.createReservation({
      carId: this.car()!.id,
      startDateTime: new Date(startDateTime).toISOString(),
      numberOfDays: numberOfDays
    }).subscribe({
      next: () => {
        this.toastService.success('Booking confirmed successfully!');
        this.router.navigate(['/reservations']);
      },
      error: (error) => {
        this.isLoading.set(false);
        this.toastService.error(error.error?.message || 'Failed to create booking');
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/cars', this.car()?.id]);
  }
}
