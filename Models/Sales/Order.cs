using LabWork_Entity_Framework.Enums;
using System;

namespace LabWork_Entity_Framework.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public Store Store { get; set; }
        public Staff Staff { get; set; }
    }
}