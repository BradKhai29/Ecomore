using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Orders.Incomings
{
    public class OrderBillingDetailDto : IDtoNormalization
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        public bool SaveInformation { get; set; }

        public string UserNote { get; set; }

        public void NormalizeAllProperties()
        {
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            Email = Email.Trim();
            PhoneNumber = PhoneNumber.Trim();
            DeliveryAddress = DeliveryAddress.Trim();
            UserNote = string.IsNullOrEmpty(UserNote) ? "None" : UserNote.Trim();
        }
    }
}
