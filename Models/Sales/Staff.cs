using System.ComponentModel.DataAnnotations;

namespace LabWork_Entity_Framework.Models
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
        [Required]
        public virtual Store Store { get; set; }
        [Required]
        public virtual Staff Manager { get; set; }
    }
}