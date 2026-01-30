import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ReservationService } from '../../../core/services/reservation.service';
import { Reservation, CarTypeLabels, CarType } from '../../../core/models';
import { ToastService } from '../../../shared/services/toast.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';

export type ReservationStatus = 'Upcoming' | 'Active' | 'Completed';

@Component({
  selector: 'app-my-reservations',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDialogModule,
    LoadingComponent
  ],
  templateUrl: './my-reservations.component.html',
  styleUrls: ['./my-reservations.component.scss']
})
export class MyReservationsComponent implements OnInit {
  private reservationService = inject(ReservationService);
  private toastService = inject(ToastService);
  private dialog = inject(MatDialog);

  reservations = signal<Reservation[]>([]);
  isLoading = signal(true);

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations(): void {
    this.reservationService.getMyReservations().subscribe({
      next: (reservations) => {
        this.reservations.set(reservations);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  getReservationStatus(reservation: Reservation): ReservationStatus {
    const now = new Date();
    const start = new Date(reservation.startDateTime);
    const end = new Date(reservation.endDateTime);

    if (now < start) {
      return 'Upcoming';
    } else if (now >= start && now <= end) {
      return 'Active';
    } else {
      return 'Completed';
    }
  }

  getTotalPrice(reservation: Reservation): number {
    // Use totalPrice from API if available, otherwise calculate
    if (reservation.totalPrice) {
      return reservation.totalPrice;
    }
    if (!reservation.car) return 0;
    return reservation.numberOfDays * reservation.car.dailyPrice;
  }

  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  canCancelReservation(reservation: Reservation): boolean {
    return this.getReservationStatus(reservation) === 'Upcoming';
  }

  cancelReservation(reservation: Reservation): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Cancel Reservation',
        message: 'Are you sure you want to cancel this reservation?',
        confirmText: 'Yes, Cancel',
        type: 'warning'
      } as ConfirmDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.reservationService.deleteReservation(reservation.id).subscribe({
          next: () => {
            this.toastService.success('Reservation cancelled successfully');
            this.loadReservations();
          },
          error: () => {
            this.toastService.error('Failed to cancel reservation');
          }
        });
      }
    });
  }
}
