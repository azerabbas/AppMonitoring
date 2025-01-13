using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderService.API.Models;
using OrderService.API.Services;
using OrderService.API.ViewModels;
using Shared.Events;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(MongoDbService mongoDbService, IPublishEndpoint publish) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderVM orderVM)
        {
            if (orderVM == null)
            {
                return BadRequest("Order cannot be null.");
            }
            // Get the collection
            var collection = mongoDbService.GetCollection<Order>();

            Order order = new()
            {
                OrderId = Guid.NewGuid(),
                ProductName = orderVM.ProductName,
                Quantity = orderVM.Quantity,
                TotalPrice = orderVM.TotalPrice,
                CreatedAt = orderVM.CreatedAt
            };

            // Insert the order into MongoDB
            await collection.InsertOneAsync(order);


            OrderCreatedEvent orderCreatedEvent = new()
            {
                Message = "Order Created"
            };
            await publish.Publish(orderCreatedEvent);

            // Return a success response
            return CreatedAtAction(nameof(CreateOrder), new { id = order.OrderId }, order); 

        }

        // Get Order by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var collection = mongoDbService.GetCollection<Order>();
            var order = await collection.Find(o => o.OrderId == id).FirstOrDefaultAsync();

            if (order == null)
                return NotFound($"Order with id {id} not found.");
            
            return Ok(order);
        }
    }
}
