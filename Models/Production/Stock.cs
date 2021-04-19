using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("stocks", Schema = "Production")]
    public class Stock
    {
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Store Store { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
