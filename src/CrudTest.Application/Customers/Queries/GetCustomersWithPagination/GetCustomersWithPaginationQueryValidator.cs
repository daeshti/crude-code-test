using FluentValidation;

namespace CrudTest.Application.Customers.Queries.GetCustomersWithPagination
{
    public class GetCustomersWithPaginationQueryValidator : AbstractValidator<GetCustomersWithPaginationQuery>
    {
        public GetCustomersWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("'Page Number' at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("'Page Size' at least greater than or equal to 1.");
        }
    }
}