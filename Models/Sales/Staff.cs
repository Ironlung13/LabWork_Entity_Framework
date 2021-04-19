using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
        public Store Store { get; set; }
        //[ForeignKey("Manager")]
        //public int? ManagerId { get; set; }
        public Staff Manager { get; set; }
    }
}