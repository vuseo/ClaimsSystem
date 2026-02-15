namespace Claims.Api.Dtos
{
    public class ClaimResponseDto
    {
        /// <summary>
        /// Dto for claim response
        /// </summary>
        public Guid Id { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string VehicleMakeAndModel { get; set; } = default!;
    }
}
