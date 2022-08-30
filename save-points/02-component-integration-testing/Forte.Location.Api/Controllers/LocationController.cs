using Forte.Location.Services;
using Forte.Location.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forte.Location.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("locations")]
        public async Task<IActionResult> Post([FromBody] LocationM location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            bool response = await _locationService.AddLocation(location);
            if (response)
            {
                return Ok(new { message = "Location added" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding data");
            }
        }

        [HttpGet("locations")]
        public IActionResult Get()
        {
            return Ok(_locationService.GetLocations());
        }

        [HttpPut("locations/{id}")]
        public async Task<ActionResult> Update(string id, LocationM location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            bool response = await _locationService.UpdateLocation(id, location);
            if (response)
            {
                return Ok(new { message = "Location updated" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpDelete("locations/{id}")]
        public ActionResult Delete(string id)
        {
            bool response = _locationService.DeleteLocation(id);
            if (response)
            {
                return Ok(new { message = "Location deleted" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}