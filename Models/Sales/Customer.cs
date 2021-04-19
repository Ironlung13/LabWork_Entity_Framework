using LabWork_Entity_Framework.Enums;
using System.Collections.Generic;

namespace LabWork_Entity_Framework.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
}
