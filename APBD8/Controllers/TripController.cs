using APBD8.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD8.Controllers;

[ApiController]
[Route("/api/trips")]
public class TripController : ControllerBase
{
    private readonly ITripService _service;

    public TripController(ITripService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [Route("/api/trips/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var trips = await _service.GetAll();
        return Ok(trips);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime dateTime)
    {
        var trips = await _service.GetByStartDate(dateTime);
        return Ok(trips);
    }
}