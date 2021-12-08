using CrudTest.Domain.Common;
using MediatR;

namespace CrudTest.Application.Common.Models
{
    /**
     * A "Template" for all domain event notifications. Helps creating and maintaining
     * them easier.
     */
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}