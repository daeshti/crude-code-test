using System;
using CrudTest.Application.Common.Mappings;
using CrudTest.Domain.Entities;

namespace CrudTest.Application.Customers.Queries.GetCustomerById
{
    public class CustomerDto : IMapFrom<Customer>
    {
        public int CustomerId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Email { get; set; }
        
        public string BankAccountNumber { get; set; }
    }
}