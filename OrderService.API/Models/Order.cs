using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.API.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class Order
    {
        [BsonId]
        [BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy)] 
        [BsonElement(Order = 0)] 
        public Guid OrderId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 1)] 
        public string ProductName { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)] 
        [BsonElement(Order = 2)] 
        public int Quantity { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)] 
        [BsonElement(Order = 3)] 
        public decimal TotalPrice { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] 
        [BsonElement(Order = 4)] 
        public DateTime CreatedAt { get; set; } 

    }

}
