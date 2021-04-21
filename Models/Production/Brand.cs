using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("Brands", Schema = "Production")]
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
