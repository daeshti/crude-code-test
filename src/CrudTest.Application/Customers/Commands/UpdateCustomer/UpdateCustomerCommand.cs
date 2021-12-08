using System;
using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Exceptions;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Domain.Entities;
using MediatR;

namespace CrudTest.Application.Customers.Commands.UpdateCustomer
{
    /**
     * A command for update of <see cref="Customer"/> class.
     */
    public class UpdateCustomerCommand : IRequest
    {
        public int CustomerId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }
        
        /**
         * This optional property will help validate the phone number.
         */
        public string PhoneNumberNationality { get; set; }

        public string Email { get; set; }
        
        public string BankAccountNumber { get; set; }
    }
    
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers
                .FindAsync(new object[] { request.CustomerId }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.CustomerId);
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.DateOfBirth = request.DateOfBirth;
            entity.PhoneNumber = request.PhoneNumber;
            entity.Email = request.Email;
            entity.BankAccountNumber = request.BankAccountNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}