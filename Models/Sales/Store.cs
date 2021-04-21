using System.ComponentModel.DataAnnotations;

namespace LabWork_Entity_Framework.Models
{
    public class Store
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
}