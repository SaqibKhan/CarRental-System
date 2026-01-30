import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ReservationService } from '../../../core/services/reservation.service';
import { Reservation, CarTypeLabels, CarType } from '../../../core/models';
import { ToastService } from '../../../shared/services/toast.service';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';

export type ReservationStatus = 'Upcoming' | 'Active' | 'Completed';

@Component({
  selector: 'app-admin-reservations',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDialogModule,
    LoadingComponent
  ],
  templateUrl: './admin-reservations.component.html',
  styleUrls: ['./admin-reservations.component.scss']
})
export class AdminReservationsComponent implements OnInit {
  private reservationService = inject(ReservationService);
  private toastService = inject(ToastService);
  private dialog = inject(MatDialog);

  reservations = signal<Reservation[]>([]);
  filteredReservations = signal<Reservation[]>([]);
  isLoading = signal(true);

  statusFilter: ReservationStatus | null = null;
  statuses: ReservationStatus[] = ['Upcoming', 'Active', 'Completed'];

  displayedColumns = ['id', 'car', 'customer', 'dates', 'total', 'status', 'actions'];

  ngOnInit(): void {
    this.loadReservations();
  }

  loadReservations(): void {
    this.reservationService.getReservations().subscribe({
      next: (reservations) => {
        this.reservations.set(reservations);
        this.filteredReservations.set(reservations);
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

  applyFilter(): void {
    if (this.statusFilter) {
      this.filteredReservations.set(
        this.reservations().filter(r => this.getReservationStatus(r) === this.statusFilter)
      );
    } else {
      this.filteredReservations.set(this.reservations());
    }
  }

  canDeleteReservation(reservation: Reservation): boolean {
    return this.getReservationStatus(reservation) === 'Upcoming';
  }

  deleteReservation(reservation: Reservation): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Reservation',
        message: 'Are you sure you want to delete this reservation?',
        confirmText: 'Yes, Delete',
        type: 'danger'
      } as ConfirmDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.reservationService.deleteReservation(reservation.id).subscribe({
          next: () => {
            this.toastService.success('Reservation deleted successfully');
            this.loadReservations();
          },
          error: () => {
            this.toastService.error('Failed to delete reservation');
          }
        });
      }
    });
  }
}
