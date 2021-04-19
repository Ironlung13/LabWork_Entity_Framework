using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("products", Schema = "Production")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public decimal Price { get; set; }
        public int ModelYear { get; set; }
    }
}
