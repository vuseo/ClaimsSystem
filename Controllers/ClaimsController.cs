using Claims.Api.Dtos;
using Claims.Api.Services;
using Claims.Domain.Data;
using Claims.Domain.Entities;
using Claims.Domain.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ClaimsDbContext _db;
        private readonly IClaimMessagePublisher _publisher;

        public ClaimsController(ClaimsDbContext db, IClaimMessagePublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        /// <summary>
        /// returns a list of claims with basic information (ID, date of incident, vehicle make/model).
        /// There is no sensitive information (like customer phone number or social security number) included in the response, 
        /// as this endpoint is intended for general listing purposes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetClaims()
        {
            var claims = await _db.Claims.Select(c => new ClaimResponseDto
            {
                Id = c.Id,
                DateOfIncident = c.DateOfIncident,
                VehicleMake = c.VehicleMake,
                VehicleModel = c.VehicleModel
            }).ToListAsync();

            return Ok(claims);
        }

        /// <summary>
        /// accepts a claim submission, creates a new claim in the database, and then publishes a ClaimSubmittedMessage.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitClaim([FromBody] CreateClaimRequestDto request)
        {
            // Create domain entity (ID is generated inside Claim)
            var claim = new Claim(
                request.CustomerPhoneNumber,
                request.CustomerSocialSecurityNumber,
                request.DateOfIncident,
                request.VehicleMake,
                request.VehicleModel);

            // For Future: use a transactional outbox or messaging pattern to ensure individual units between database and message broker operations.
            // Along with dead-letter queue and retry policies to handle failures in message publishing without losing data integrity.
            _db.Claims.Add(claim);
            await _db.SaveChangesAsync();

            // Publish integration event AFTER persistence
            var message = new ClaimSubmittedMessage
            {
                ClaimId = claim.Id,
                CustomerPhoneNumber = claim.CustomerPhoneNumber,
                CustomerSocialSecurityNumber = claim.CustomerSocialSecurityNumber,
                DateOfIncident = claim.DateOfIncident,
                VehicleMake = claim.VehicleMake,
                VehicleModel = claim.VehicleModel,
                SubmittedAt = DateTime.UtcNow
            };

            await _publisher.PublishClaimAsync(message);

            return Accepted(new { claim.Id });
        }
    }
}
