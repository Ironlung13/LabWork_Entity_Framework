using System.ComponentModel.DataAnnotations.Schema;

namespace LabWork_Entity_Framework.Models
{
    [Table("Stocks", Schema = "Production")]
    public class Stock
    {
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
