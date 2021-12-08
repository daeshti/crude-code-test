using CrudTest.Domain.Common;
using CrudTest.Domain.Entities;

namespace CrudTest.Domain.Events
{
    public class CustomerCreatedEvent : DomainEvent
    {
        public CustomerCreatedEvent(Customer customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; }
    }
}