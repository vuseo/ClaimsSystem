namespace Claims.Api.Dtos
{
    public class CreateClaimRequestDto
    {
        /// <summary>
        /// Dto claim request 
        /// </summary>
        public string CustomerPhoneNumber { get; set; } = default!;
        public string CustomerSocialSecurityNumber { get; set; } = default!;
        public DateTime DateOfIncident { get; set; }
        public string VehicleMakeAndModel { get; set; } = default!;

    }
}
