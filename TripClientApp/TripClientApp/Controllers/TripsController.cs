using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TripClientApp.DTOs;
using TripClientApp.Exceptions;
using TripClientApp.Services;

namespace TripClientApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController(IDbService dbService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetAllTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    { 
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and page size must be greater than 0.");
        }

        var trips = await dbService.GetAllTripsAsync(page, pageSize);
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int idTrip, [FromBody] ClientTripCreateDto clientDto)
    {
        try
        {
            return CreatedAtAction(nameof(AddClientToTrip),await dbService.AddClientToTrip(idTrip, clientDto));
        }
        catch (ClientAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (TripAlreadyStartedException e)
        {
            return Conflict(e.Message);
        }
        catch (FormatException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An unexpected error has occured: {e.Message}");
        }
    }
    
}