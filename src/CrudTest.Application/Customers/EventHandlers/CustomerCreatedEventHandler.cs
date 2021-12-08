using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Models;
using CrudTest.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrudTest.Application.Customers.EventHandlers
{
    /**
     * This event handler is currently in a showcase state and has no other use.
     */
    public class CustomerCreatedEventHandler : INotificationHandler<DomainEventNotification<CustomerCreatedEvent>>
    {
        private readonly ILogger<CustomerCreatedEventHandler> _logger;

        public CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<CustomerCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CrudTest Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}