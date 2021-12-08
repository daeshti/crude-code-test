using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Application.Common.Mappings;
using CrudTest.Application.Common.Models;
using MediatR;

namespace CrudTest.Application.Customers.Queries.GetCustomersWithPagination
{
    public class GetCustomersWithPaginationQuery : IRequest<PaginatedList<CustomerBriefDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    
}