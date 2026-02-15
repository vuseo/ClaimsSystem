using Claims.Api.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClaims() 
        { 
            return Ok(new List<ClaimResponseDto>());
        }

        [HttpPost]
        public IActionResult 

    }
}
