# 🚗 Car Rental System

A modern Car Rental API built with **ASP.NET Core** and **.NET 10**, following Clean Architecture principles.

## 📋 Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)

## Overview

Car Rental System is a RESTful API that allows users to browse available cars and make reservations. The application features JWT authentication, Swagger documentation, and CORS support for Angular frontend integration.

## Tech Stack

- **.NET 10** - Target Framework
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM & Data Access
- **SQL Server** - Database
- **Swagger/OpenAPI** - API Documentation
- **JWT** - Authentication

## Project Structure

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or SQL Server Express)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Getting Started

### 1. Clone the Repository

### 2. Install Dependencies

Run the following command from the solution root directory:

```bash
dotnet restore
```

### 3. Apply Database Migrations

Run the following commands from the solution root directory:

```bash
dotnet ef database update
```

### 4. Run the Application

Start the application using your preferred method:

- Visual Studio: Press F5 or click on "IIS Express" button
- VS Code: Press `Ctrl + F5` or use the integrated terminal

The API should be running on `https://localhost:5001` by default.

### 5. Access the API

- **Swagger UI**: https://localhost:{port}/swagger
- **API Base URL**: https://localhost:{port}/api

## API Endpoints

### Cars

| Method | Endpoint              | Description            | Auth Required |
|--------|-----------------------|------------------------|---------------|
| GET    | `/api/cars`           | Get all cars           | No            |
| GET    | `/api/cars/car/{id}`  | Get car by ID          | No            |
| GET    | `/api/cars/type/{carType}` | Get cars by type  | No            |

### Reservations

| Method | Endpoint              | Description                | Auth Required |
|--------|-----------------------|----------------------------|---------------|
| POST   | `/api/reservations`   | Create a reservation       | Yes           |
| GET    | `/api/reservations`   | Get user's reservations    | Yes           |

### Create Reservation Request Body

````````markdown
{
  "carId": 1,
  "userId": 123,
  "startDate": "2023-04-01",
  "endDate": "2023-04-07"
}
````````

# Response

````````markdown
{
  "reservationId": 456,
  "carId": 1,
  "userId": 123,
  "startDate": "2023-04-01",
  "endDate": "2023-04-07",
  "status": "Confirmed"
}
````````

## Configuration

### JWT Settings (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CarRentalDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "ExpireDays": 30
  }
}
```

### CORS Configuration

The API is configured to allow requests from Angular frontend:
- `http://localhost:4200`
- `https://localhost:4200`

## Running Tests

## License

This project is licensed under the MIT License.

---

**Note**: This project is for educational purposes only. Do not use it in production without proper security audits and testing.

How to Run the application

1) Add you Connection string to appsettings.Development.json.
2) Build the solution and got to solution folder and run the following command for Entity Framework Project.
    dotnet ef database update --project CarRental.Infrastructure --startup-project CarRental
3) Run the application in visual studio.

Open the CarRental UI project in VS Code for Front End
Run Ng Serve to run the application
