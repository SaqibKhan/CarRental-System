import { Car } from './car.model';

export interface Reservation {
  id: string;
  carId: string;
  userId: string;
  startDateTime: string;
  endDateTime: string;
  numberOfDays: number;
  totalPrice: number;
  car?: Car;
  user?: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
  };
}

export interface CreateReservationRequest {
  carId: string;
  startDateTime: string;
  numberOfDays: number;
}

export interface ReservationFilter {
  startDate?: Date;
  endDate?: Date;
  userId?: string;
}
