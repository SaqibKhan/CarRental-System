// Simplified reservation for car's embedded reservations list
export interface CarReservation {
  id: string;
  startDateTime: string;
  endDateTime: string;
}

export interface Car {
  id: string;
  carName: string;
  numberPlate: string;
  modelYear: string;
  dailyPrice: number;
  description?: string;
  carType: CarType;
  isActive: boolean;
  imageUrl?: string;
  reservations: CarReservation[];
}

// Car type enum matching backend
export enum CarType {
 Sedan = 0,
  SUV = 1,
  Van = 2
}
// Helper to get car type label
export const CarTypeLabels: Record<CarType, string> = {
  [CarType.Sedan]: 'Sedan',
  [CarType.SUV]: 'SUV',
  [CarType.Van]: 'Van'
};

export interface CarFilter {
  carType?: CarType;
  minPrice?: number;
  maxPrice?: number;
  startDate?: Date;
  endDate?: Date;
  searchTerm?: string;
  page?: number;
  pageSize?: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface PaginationState {
  currentPage: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  pageSizeOptions: number[];
}

export interface CreateCarRequest {
  carName: string;
  numberPlate: string;
  modelYear: string;
  dailyPrice: number;
  imageUrl?: string;
  description?: string;
  carType: CarType;
}

export interface UpdateCarRequest extends CreateCarRequest {
  id: string;
}
