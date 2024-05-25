using APBD8.Exceptions;
using APBD8.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD8.Controllers;

[ApiController]
[Route("/api/clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _service;

    public ClientController(IClientService service)
    {
        _service = service;
    }

    [HttpDelete]
    [Route("/api/clients/{idClient}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute] int idClient)
    {
        try
        {
            if (await _service.Delete(idClient))
            {
                return NoContent();
            }
            return Conflict("Cannot delete client");
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}