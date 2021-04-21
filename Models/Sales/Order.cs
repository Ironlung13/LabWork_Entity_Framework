using LabWork_Entity_Framework.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabWork_Entity_Framework.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        [Required]
        public virtual Store Store { get; set; }
        [Required]
        public virtual Staff Staff { get; set; }
        [Required]
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}