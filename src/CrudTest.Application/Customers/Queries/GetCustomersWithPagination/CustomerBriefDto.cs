using CrudTest.Application.Common.Mappings;
using CrudTest.Domain.Entities;

namespace CrudTest.Application.Customers.Queries.GetCustomersWithPagination
{
    public class CustomerBriefDto : IMapFrom<Customer>
    {
        public int CustomerId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}