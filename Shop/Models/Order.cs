﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalPrice {  get; set; }
        public string PostOffice { get; set; }
        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public Order() => OrderItems = new List<OrderItem>();
    }
}
