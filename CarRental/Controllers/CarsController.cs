using Azure;
using CarRental.Application.Services;
using CarRental.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _carService.GetAllCarsAsync(cancellationToken);        
        return Ok(result.Value);
    }

    [HttpGet("Car/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _carService.GetCarByIdAsync(id,cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("type/{carType}")]
    public async Task<IActionResult> GetByType(CarType carType, CancellationToken cancellationToken)
    {
        var result = await _carService.GetCarsByTypeAsync(carType, cancellationToken);
        return Ok(result.Value);
    }
}
