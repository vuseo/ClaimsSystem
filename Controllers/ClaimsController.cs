using Claims.Api.Dtos;
using Claims.Domain;
using Claims.Domain.Entities;
using Claims.Domain.Interfaces;
using Claims.Domain.Messages;
using Claims.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimRepository _repository;
        private readonly IClaimMessagePublisher _publisher;

        public ClaimsController(IClaimRepository repository, IClaimMessagePublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims()
        {
            var claims = await _repository.GetAllAsync();

            var response = claims.Select(c => new ClaimResponseDto
            {
                Id = c.Id,
                DateOfIncident = c.DateOfIncident.Value,
                VehicleMake = c.Vehicle.Make,
                VehicleModel = c.Vehicle.Model
                // intentionally excluding sensitive fields
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim([FromBody] CreateClaimRequestDto request)
        {
            // Create domain entity with value objects (validations happen inside VOs)
            var claim = new Claim(
                new PhoneNumber(request.CustomerPhoneNumber),
                new SocialSecurityNumber(request.CustomerSocialSecurityNumber),
                new IncidentDate(request.DateOfIncident),
                new Vehicle(request.VehicleMake, request.VehicleModel)
            );

            // Persist claim using repository
            await _repository.AddAsync(claim);

            // Publish integration event AFTER persistence
            var message = new ClaimSubmittedMessage
            {
                ClaimId = claim.Id,
                CustomerPhoneNumber = claim.CustomerPhoneNumber.Value,
                CustomerSocialSecurityNumber = claim.CustomerSocialSecurityNumber.Value,
                DateOfIncident = claim.DateOfIncident.Value,
                VehicleMake = claim.Vehicle.Make,
                VehicleModel = claim.Vehicle.Model,
                SubmittedAt = DateTime.UtcNow
            };

            await _publisher.PublishClaimAsync(message);

            return Accepted(new { claim.Id });
        }
    }
}