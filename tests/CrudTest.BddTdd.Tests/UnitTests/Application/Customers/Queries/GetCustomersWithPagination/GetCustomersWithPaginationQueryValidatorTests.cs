using System.Linq;
using System.Threading.Tasks;
using CrudTest.Application.Customers.Queries.GetCustomersWithPagination;
using FluentAssertions;
using Xunit;

namespace CrudTest.BddTdd.Tests.UnitTests.Application.Customers.Queries.GetCustomersWithPagination
{
    public class GetCustomersWithPaginationQueryValidatorTests
    {
        private readonly GetCustomersWithPaginationQueryValidator _validator;

        public GetCustomersWithPaginationQueryValidatorTests()
        {
            _validator = new GetCustomersWithPaginationQueryValidator();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task PageNumberLessThanOne_ShouldFail(int pageNumber)
        {
            var validationResult = await _validator.ValidateAsync(new GetCustomersWithPaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = 100
            });

            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Page Number' at least greater than or equal to 1."
            });
        }
        
        
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task PageSizeLessThanOne_ShouldFail(int pageSize)
        {
            var validationResult = await _validator.ValidateAsync(new GetCustomersWithPaginationQuery
            {
                PageNumber = 100,
                PageSize = pageSize
            });

            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Page Size' at least greater than or equal to 1."
            });
        }

        [Fact]
        public async Task PageNumberAndPageSizeGreaterThanZero_ShouldPass()
        {
            var validationResult = await _validator.ValidateAsync(new GetCustomersWithPaginationQuery
            {
                PageNumber = 100,
                PageSize = 100
            });

            validationResult.IsValid.Should().BeTrue();
        }
    }
}