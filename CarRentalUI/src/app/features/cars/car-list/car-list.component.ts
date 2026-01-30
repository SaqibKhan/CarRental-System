import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { CarService } from '../../../core/services/car.service';
import { AuthService } from '../../../core/services/auth.service';
import { Car, CarType, CarTypeLabels, CarFilter, PaginationState } from '../../../core/models';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';
import { CarDetailsDialogComponent } from '../car-details-dialog/car-details-dialog.component';
import { BookCarDialogComponent } from '../../reservations/book-car-dialog/book-car-dialog.component';
import { environment } from '../../../../environments/environment';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-car-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatChipsModule,
    MatPaginatorModule,
    LoadingComponent
  ],
  templateUrl: './car-list.component.html',
  styleUrls: ['./car-list.component.scss']
})
export class CarListComponent implements OnInit {
  private carService = inject(CarService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private dialog = inject(MatDialog);

  // Data signals
  cars = signal<Car[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  // Pagination state
  pagination = signal<PaginationState>({
    currentPage: 1,
    pageSize: 12,
    totalItems: 0,
    totalPages: 0,
    pageSizeOptions: [6, 12, 24, 48]
  });

  // Filter state
  searchTerm = '';
  selectedCarType: CarType | null = null;
  maxPrice: number | null = null;

  // Debounce subject for search
  private searchSubject = new Subject<string>();

  // Car types for dropdown
  carTypes = [
    { value: CarType.Sedan, label: CarTypeLabels[CarType.Sedan] },
    { value: CarType.SUV, label: CarTypeLabels[CarType.SUV] },
    { value: CarType.Van, label: CarTypeLabels[CarType.Van] }
  ];

  // Computed properties
  displayedCarsRange = computed(() => {
    const { currentPage, pageSize, totalItems } = this.pagination();
    if (totalItems === 0) return { start: 0, end: 0, total: 0 };
    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalItems);
    return { start, end, total: totalItems };
  });

  hasResults = computed(() => this.cars().length > 0);

  ngOnInit(): void {
    // Setup debounced search
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.resetToFirstPage();
      this.loadCars();
    });

    this.loadCars();
  }

  loadCars(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const filter = this.buildFilter();

    this.carService.getCarsPaginated(filter).subscribe({
      next: (response) => {
        this.cars.set(response.items);
        this.pagination.update(state => ({
          ...state,
          totalItems: response.totalCount,
          totalPages: response.totalPages,
          currentPage: response.page,
          pageSize: response.pageSize
        }));
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading cars:', error);
        this.errorMessage.set('Failed to load cars. Please try again.');
        this.cars.set([]);
        this.isLoading.set(false);
      }
    });
  }

  private buildFilter(): CarFilter {
    const { currentPage, pageSize } = this.pagination();

    const filter: CarFilter = {
      page: currentPage,
      pageSize: pageSize
    };

    if (this.searchTerm?.trim()) {
      filter.searchTerm = this.searchTerm.trim();
    }

    if (this.selectedCarType !== null) {
      filter.carType = this.selectedCarType;
    }

    if (this.maxPrice && this.maxPrice > 0) {
      filter.maxPrice = this.maxPrice;
    }

    return filter;
  }

  // Search with debounce
  onSearchChange(term: string): void {
    this.searchSubject.next(term);
  }

  // Filter change handlers
  onCarTypeChange(): void {
    this.resetToFirstPage();
    this.loadCars();
  }

  onPriceChange(): void {
    this.resetToFirstPage();
    this.loadCars();
  }

  // Pagination handlers
  onPageChange(event: PageEvent): void {
    this.pagination.update(state => ({
      ...state,
      currentPage: event.pageIndex + 1,
      pageSize: event.pageSize
    }));
    this.loadCars();
    this.scrollToTop();
  }

  goToPage(page: number): void {
    const { totalPages } = this.pagination();
    if (page >= 1 && page <= totalPages) {
      this.pagination.update(state => ({
        ...state,
        currentPage: page
      }));
      this.loadCars();
      this.scrollToTop();
    }
  }

  goToPreviousPage(): void {
    const { currentPage } = this.pagination();
    if (currentPage > 1) {
      this.goToPage(currentPage - 1);
    }
  }

  goToNextPage(): void {
    const { currentPage, totalPages } = this.pagination();
    if (currentPage < totalPages) {
      this.goToPage(currentPage + 1);
    }
  }

  goToFirstPage(): void {
    this.goToPage(1);
  }

  goToLastPage(): void {
    this.goToPage(this.pagination().totalPages);
  }

  private resetToFirstPage(): void {
    this.pagination.update(state => ({
      ...state,
      currentPage: 1
    }));
  }

  private scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedCarType = null;
    this.maxPrice = null;
    this.resetToFirstPage();
    this.loadCars();
  }

  refreshData(): void {
    this.loadCars();
  }

  openCarDetails(car: Car): void {
    this.dialog.open(CarDetailsDialogComponent, {
      data: car,
      width: '600px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      panelClass: 'car-details-dialog-panel'
    });
  }

  // Book car - opens booking dialog, redirects to login if not authenticated
  bookCar(car: Car): void {
    if (!this.isCarAvailable(car)) {
      return;
    }

    // Check authentication using AuthService (properly validates session)
    if (this.authService.checkIsAuthenticated()) {
      // User is logged in, open booking dialog
      const dialogRef = this.dialog.open(BookCarDialogComponent, {
        data: car,
        width: '750px',
        maxWidth: '95vw',
        maxHeight: '90vh',
        panelClass: 'book-car-dialog-panel',
        autoFocus: false
      });

      // Refresh cars list if booking was successful
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.loadCars();
        }
      });
    } else {
      // User is not logged in, redirect to login with return URL
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: `/cars` }
      });
    }
  }

  // Pagination helper methods
  getPageNumbers(): number[] {
    const { currentPage, totalPages } = this.pagination();
    const maxVisiblePages = 5;
    const pages: number[] = [];

    if (totalPages <= maxVisiblePages) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      const halfVisible = Math.floor(maxVisiblePages / 2);
      let startPage = Math.max(1, currentPage - halfVisible);
      let endPage = Math.min(totalPages, startPage + maxVisiblePages - 1);

      if (endPage - startPage < maxVisiblePages - 1) {
        startPage = Math.max(1, endPage - maxVisiblePages + 1);
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }

    return pages;
  }

  isFirstPage(): boolean {
    return this.pagination().currentPage === 1;
  }

  isLastPage(): boolean {
    const { currentPage, totalPages } = this.pagination();
    return currentPage === totalPages || totalPages === 0;
  }

  // Get car type label
  getCarTypeLabel(carType: CarType): string {
    return CarTypeLabels[carType] || 'Unknown';
  }

  // Check if car is available (not currently reserved)
  isCarAvailable(car: Car): boolean {
    if (!car.isActive) return false;

    // If no reservations, car is available
    if (!car.reservations || car.reservations.length === 0) return true;

    // Check if any reservation is currently active
    const now = new Date();
    const hasActiveReservation = car.reservations.some(reservation => {
      const start = new Date(reservation.startDateTime);
      const end = new Date(reservation.endDateTime);
      return now < start && now > end;
    });

    return hasActiveReservation;
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
