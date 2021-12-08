using System.Threading.Tasks;
using CrudTest.Domain.Common;

namespace CrudTest.Application.Common.Interfaces
{
    /**
     * An interface for domain services, all events are published through this interface
     * to first, decouple the application from MediatR and second to act as a wrapper.
     */
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}