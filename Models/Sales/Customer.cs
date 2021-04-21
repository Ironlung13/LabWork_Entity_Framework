using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabWork_Entity_Framework.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public string State { get; set; }
        [Required]
        public int ZipCode { get; set; }
        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"First Name: {FirstName}");
            sb.AppendLine($"Last Name: {LastName}");
            if (!(PhoneNumber is null))
            {
                sb.AppendLine($"Phone Number: {PhoneNumber}");
            }
            sb.AppendLine($"Email: {Email}");
            sb.AppendLine($"City: {City}");
            sb.AppendLine($"Address: {Address}");
            if (!(State is null))
            {
                sb.AppendLine($"Phone Number: {State}");
            }
            sb.AppendLine($"Zip code: {ZipCode}");
            return sb.ToString();
        }
    }
}
