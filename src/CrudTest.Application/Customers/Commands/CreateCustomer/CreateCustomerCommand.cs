using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Domain.Entities;
using CrudTest.Domain.Events;
using MediatR;

namespace CrudTest.Application.Customers.Commands.CreateCustomer
{
    /**
     * A command for creation of <see cref="Customer"/> class.
     */
    public class CreateCustomerCommand : IRequest<int>
    {
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
    
    /**
     * A Handler for <see cref="CreateCustomerCommand"/>. Triggers <see cref="CustomerCreatedEvent"/>
     * and returns the Customer's Id.
     */
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public CreateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                BankAccountNumber = request.BankAccountNumber,
            };
            
            
            entity.DomainEvents.Add(new CustomerCreatedEvent(entity));

            _context.Customers.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
            return entity.CustomerId;
        }
    }
}