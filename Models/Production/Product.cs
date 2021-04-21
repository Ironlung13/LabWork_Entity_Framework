using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("Products", Schema = "Production")]
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public virtual Category Category { get; set; }
        [Required]
        public virtual Brand Brand { get; set; }
        public decimal Price { get; set; }
        public int ModelYear { get; set; }
    }
}
