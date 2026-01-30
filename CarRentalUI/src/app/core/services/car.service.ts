import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Car, CarFilter, CreateCarRequest, UpdateCarRequest, PaginatedResponse } from '../models';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private readonly apiUrl = `${environment.apiUrl}/cars`;

  constructor(private http: HttpClient) {}

  /**
   * Get paginated cars with optional filtering
   */
  getCarsPaginated(filter?: CarFilter): Observable<PaginatedResponse<Car>> {
    let params = new HttpParams();

    if (filter) {
      if (filter.carType !== undefined) params = params.set('carType', filter.carType.toString());
      if (filter.minPrice) params = params.set('minPrice', filter.minPrice.toString());
      if (filter.maxPrice) params = params.set('maxPrice', filter.maxPrice.toString());
      if (filter.startDate) params = params.set('startDate', filter.startDate.toISOString());
      if (filter.endDate) params = params.set('endDate', filter.endDate.toISOString());
      if (filter.searchTerm) params = params.set('searchTerm', filter.searchTerm);
      if (filter.page !== undefined) params = params.set('page', filter.page.toString());
      if (filter.pageSize !== undefined) params = params.set('pageSize', filter.pageSize.toString());
    }

    // Try paginated endpoint first, fallback to wrapping non-paginated response
    return this.http.get<PaginatedResponse<Car> | Car[]>(this.apiUrl, { params }).pipe(
      map(response => {
        // If response is already paginated
        if (this.isPaginatedResponse(response)) {
          return response;
        }
        // Fallback: wrap array response in paginated format
        const items = response as Car[];
        const page = filter?.page ?? 1;
        const pageSize = filter?.pageSize ?? 12;

        // Apply client-side filtering if needed
        let filteredItems = items;

        if (filter?.searchTerm) {
          const term = filter.searchTerm.toLowerCase();
          filteredItems = filteredItems.filter(car =>
            car.carName.toLowerCase().includes(term) ||
            car.numberPlate.toLowerCase().includes(term)
          );
        }

        if (filter?.carType !== undefined) {
          filteredItems = filteredItems.filter(car => car.carType === filter.carType);
        }

        if (filter?.maxPrice) {
          filteredItems = filteredItems.filter(car => car.dailyPrice <= filter.maxPrice!);
        }

        const totalCount = filteredItems.length;
        const totalPages = Math.ceil(totalCount / pageSize);

        // Apply client-side pagination
        const startIndex = (page - 1) * pageSize;
        const paginatedItems = filteredItems.slice(startIndex, startIndex + pageSize);

        return {
          items: paginatedItems,
          totalCount,
          page,
          pageSize,
          totalPages,
          hasNextPage: page < totalPages,
          hasPreviousPage: page > 1
        };
      })
    );
  }

  /**
   * Get all cars
   */
  getCars(filter?: CarFilter): Observable<Car[]> {
    let params = new HttpParams();

    if (filter) {
      if (filter.carType !== undefined) params = params.set('carType', filter.carType.toString());
      if (filter.minPrice) params = params.set('minPrice', filter.minPrice.toString());
      if (filter.maxPrice) params = params.set('maxPrice', filter.maxPrice.toString());
      if (filter.startDate) params = params.set('startDate', filter.startDate.toISOString());
      if (filter.endDate) params = params.set('endDate', filter.endDate.toISOString());
      if (filter.searchTerm) params = params.set('searchTerm', filter.searchTerm);
    }

    return this.http.get<Car[]>(this.apiUrl, { params });
  }

  private isPaginatedResponse(response: unknown): response is PaginatedResponse<Car> {
    return (
      response !== null &&
      typeof response === 'object' &&
      'items' in response &&
      'totalCount' in response &&
      'page' in response
    );
  }

  getCarById(id: string): Observable<Car> {
    return this.http.get<Car>(`${this.apiUrl}/${id}`);
  }

  createCar(request: CreateCarRequest): Observable<Car> {
    return this.http.post<Car>(this.apiUrl, request);
  }

  updateCar(id: string, request: UpdateCarRequest): Observable<Car> {
    return this.http.put<Car>(`${this.apiUrl}/${id}`, request);
  }

  deleteCar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  checkAvailability(carId: string, startDate: Date, endDate: Date): Observable<boolean> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<boolean>(`${this.apiUrl}/${carId}/availability`, { params });
  }
}
