using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.API.ViewModels
{
    public class OrderVM
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
