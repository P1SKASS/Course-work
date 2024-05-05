using Microsoft.AspNetCore.Mvc;

namespace Shop.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }
}
