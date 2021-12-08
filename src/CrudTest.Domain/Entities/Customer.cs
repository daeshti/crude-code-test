using System;
using System.Collections.Generic;
using CrudTest.Domain.Common;

namespace CrudTest.Domain.Entities
{
    public class Customer : IHasDomainEvent
    {
        // TODO: communicate about requirements and strategies regarding Primary Key
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        /**
         * Date of birth, without any legit time info
         */
        public DateTime DateOfBirth { get; set; }

        /**
         * A syntactically valid email without any other guaranty 
         */
        public string PhoneNumber { get; set; }

        /**
         * A syntactically valid email without any other guaranty
         */
        public string Email { get; set; }

        /**
         * A valid International Bank Account Number without any guaranty of actual existence
         */
        public string BankAccountNumber { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}