using Microsoft.AspNetCore.Mvc;
using TripClientApp.Exceptions;
using TripClientApp.Services;

namespace TripClientApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IDbService dbService) : ControllerBase
{
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient([FromRoute] int idClient)
    {
        try
        {
            await dbService.DeleteClientAsync(idClient);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ClientHasTripsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"An unexpected error has occured: {e.Message}");
        }
    }
}