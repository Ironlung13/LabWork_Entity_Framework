using System.ComponentModel.DataAnnotations;

namespace LabWork_Entity_Framework.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Id { get; set; }
        [Required]
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
