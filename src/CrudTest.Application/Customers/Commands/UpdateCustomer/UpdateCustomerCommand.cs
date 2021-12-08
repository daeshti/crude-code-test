using System;
using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Exceptions;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Domain.Entities;
using MediatR;

namespace CrudTest.Application.Customers.Commands.UpdateCustomer
{
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
}