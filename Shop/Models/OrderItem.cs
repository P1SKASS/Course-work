﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public DateTime DeliveryDate
        {
            get { return DeliveryDate; }
            set => DeliveryDate = value.AddDays(5);
        }
    }
}