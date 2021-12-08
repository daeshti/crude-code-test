using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Domain.Common;
using CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudTest.Infrastructure.Persistence
{
    /**
     * This implementation of IApplicationDbContext watches entities that implement <see cref="IHasDomainEvent"/>
     * when it's SaveChangeAsync method is called, and publishes their unpublished events to
     * the domain event service after they've been saved successfully. It also calls
     * entity framework's method to search for all entity type configurations while
     * model creating. 
     */
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(DbContextOptions options, IDomainEventService domainEventService) : base(options)
        {
            _domainEventService = domainEventService;
        }

        public DbSet<Customer> Customers => Set<Customer>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents(events);

            return result;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents(IEnumerable<DomainEvent> events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }
    }
}