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

    public class
        GetCustomersWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery,
            PaginatedList<CustomerBriefDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetCustomersWithPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<PaginatedList<CustomerBriefDto>> Handle(GetCustomersWithPaginationQuery request,
            CancellationToken cancellationToken)
        {
            return _context.Customers
                .OrderBy(x => x.CustomerId)
                .ProjectTo<CustomerBriefDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}