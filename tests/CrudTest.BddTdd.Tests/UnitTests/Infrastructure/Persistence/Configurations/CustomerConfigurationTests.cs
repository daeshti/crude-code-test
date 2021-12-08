using System.Runtime.InteropServices;
using CrudTest.Infrastructure.Persistence.Configurations;
using FluentAssertions;
using Xunit;

namespace CrudTest.BddTdd.Tests.UnitTests.Infrastructure.Persistence.Configurations
{
    
    public class CustomerConfigurationTests
    {

        [Theory]
        [InlineData("+989029058590", 989029058590)]
        public void PhoneNumber_ShouldMapToUlong(string phoneNumber, ulong u64)
        {
            var result = CustomerConfiguration.PhoneNumberAsULong(phoneNumber);
            result.Should().Be(u64);
        }

        [Theory]
        [InlineData(989029058590, "+989029058590")]
        public void ULong_ShouldMapToPhoneNumber(ulong u64, string phoneNumber)
        {
            var result = CustomerConfiguration.ULongAsPhoneNumber(u64);
            result.Should().Be(phoneNumber);
        }
        
    }
}