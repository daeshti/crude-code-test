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
}