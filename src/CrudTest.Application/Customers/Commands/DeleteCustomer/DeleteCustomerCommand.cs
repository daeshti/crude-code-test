using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Exceptions;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Domain.Entities;
using MediatR;

namespace CrudTest.Application.Customers.Commands.DeleteCustomer
{
    /**
     * A command for deleting a <see cref="Customer"/>
     */
    public class DeleteCustomerCommand : IRequest
    {
        public int CustomerId { get; set; }
    }

    /**
     * A Handler for <see cref="DeleteCustomerCommand"/>. Retreives and checks for
     * existence of the customer therefore calls the DB twice.
     */
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers
                .FindAsync(new object[] { request.CustomerId }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.CustomerId);
            }

            _context.Customers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;;
        }
    }
}