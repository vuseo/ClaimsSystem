using System.ComponentModel.DataAnnotations;

namespace Claims.Api.Dtos
{
    public class CreateClaimRequestDto
    {
        /// <summary>
        /// Dto claim request 
        /// </summary>
        /// 
        [Required]
        //[Phone] For future validation, we can use the [Phone] attribute to ensure the phone number is in a valid format. For now, we will just require it to be non-empty.
        public string CustomerPhoneNumber { get; set; } = default!;
        [Required]
        //[StringLength(20, MinimumLength = 6)] for future validation, we can use the [StringLength] attribute to ensure the social security number is of a valid length. For now, we will just require it to be non-empty.
        public string CustomerSocialSecurityNumber { get; set; } = default!;
        [Required]
        public DateTime DateOfIncident { get; set; }
        [Required]
        public string VehicleMake { get; set; } = default!;
        [Required]
        public string VehicleModel { get; set; } = default!;
    }
}
