import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CarService } from '../../../core/services/car.service';
import { ToastService } from '../../../shared/services/toast.service';
import { Car, CarType, CarTypeLabels, CreateCarRequest } from '../../../core/models';

@Component({
  selector: 'app-car-form',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './car-form.component.html',
  styleUrls: ['./car-form.component.scss']
})
export class CarFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private carService = inject(CarService);
  private toastService = inject(ToastService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = signal(false);
  isLoading = signal(false);
  carId: string | null = null;

  // Car types for dropdown
  carTypes = [
    { value: CarType.Sedan, label: CarTypeLabels[CarType.Sedan] },
    { value: CarType.SUV, label: CarTypeLabels[CarType.SUV] },
    { value: CarType.Van, label: CarTypeLabels[CarType.Van] }
  ];


  carForm: FormGroup = this.fb.group({
    carName: ['', Validators.required],
    numberPlate: ['', Validators.required],
    modelYear: ['', Validators.required],
    carType: [CarType.Sedan, Validators.required],
    dailyPrice: ['', [Validators.required, Validators.min(1)]],
    imageUrl: [''],
    description: ['']
  });

  ngOnInit(): void {
    this.carId = this.route.snapshot.paramMap.get('id');
    if (this.carId) {
      this.isEditMode.set(true);
      this.loadCar(this.carId);
    }
  }

  loadCar(id: string): void {
    this.carService.getCarById(id).subscribe({
      next: (car) => {
        this.carForm.patchValue({
          carName: car.carName,
          numberPlate: car.numberPlate,
          modelYear: car.modelYear,
          carType: car.carType,
          dailyPrice: car.dailyPrice,
          imageUrl: car.imageUrl,
          description: car.description
        });
      }
    });
  }

  onSubmit(): void {
    if (this.carForm.invalid) {
      this.carForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    const formValue = this.carForm.value;

    const carData: CreateCarRequest = {
      carName: formValue.carName,
      numberPlate: formValue.numberPlate,
      modelYear: formValue.modelYear,
      carType: formValue.carType,
      dailyPrice: formValue.dailyPrice,
      imageUrl: formValue.imageUrl,
      description: formValue.description
    };

    const request = this.isEditMode()
      ? this.carService.updateCar(this.carId!, { ...carData, id: this.carId! })
      : this.carService.createCar(carData);

    request.subscribe({
      next: () => {
        this.toastService.success(`Car ${this.isEditMode() ? 'updated' : 'created'} successfully!`);
        this.router.navigate(['/admin/cars']);
      },
      error: (error) => {
        this.isLoading.set(false);
        this.toastService.error(error.error?.message || 'Failed to save car');
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/admin/cars']);
  }
}
