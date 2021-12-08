using System.Threading;
using System.Threading.Tasks;
using CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudTest.Application.Common.Interfaces
{
    /**
     * Since entity framework provides a class instead of an interface this
     * interface is created to decouple DbContext as a dependency so parts
     * depending on it can be tested.
     */
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}