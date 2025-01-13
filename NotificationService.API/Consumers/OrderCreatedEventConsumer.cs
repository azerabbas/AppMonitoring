using MassTransit;
using NotificationService.Notifications;
using Shared.Events;

namespace NotificationService.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreated = context.Message;

            var notificationMessage = new OrderCreatedNotification
            {
                Message = orderCreated.Message
            };

            await SendNotification(notificationMessage);

        }

        private async Task SendNotification(OrderCreatedNotification notification)
        {
            Console.WriteLine(notification.Message);
        }
    }
}
