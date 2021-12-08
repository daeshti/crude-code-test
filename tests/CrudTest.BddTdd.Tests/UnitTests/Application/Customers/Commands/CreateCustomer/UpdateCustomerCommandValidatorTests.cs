using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Application.Customers.Commands.UpdateCustomer;
using CrudTest.Application.Customers.Common;
using CrudTest.Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CrudTest.BddTdd.Tests.UnitTests.Application.Customers.Commands.CreateCustomer
{
    public class UpdateCustomerCommandValidatorTests
    {
        private readonly ICollection<Customer> _customers = new List<Customer>
        {
            new()
            {
                CustomerId = 1,
                FirstName = "Ernst",
                LastName = "Greenhow",
                DateOfBirth = DateTime.Parse("9/3/1988"),
                PhoneNumber = "+86 447 365 7383",
                Email = "egreenhow0@oaic.gov.au",
                BankAccountNumber = "LT41 1593 4633 2093 4097"
            },
            new()
            {
                CustomerId = 2,
                FirstName = "Kari",
                LastName = "Sherrin",
                DateOfBirth = DateTime.Parse("2/7/1999"),
                PhoneNumber = "+63 996 318 7652",
                Email = "ksherrin1@surveymonkey.com",
                BankAccountNumber = "TR33 0558 2GTA SOWM CZ0U MYHI U2"
            },
            new()
            {
                CustomerId = 3,
                FirstName = "Winifred",
                LastName = "Ranvoise",
                DateOfBirth = DateTime.Parse("8/23/1989"),
                PhoneNumber = "+62 264 439 8525",
                Email = "wranvoise2@virginia.edu",
                BankAccountNumber = "GE84 LW82 5931 9842 7913 22"
            }
        };

        private readonly IApplicationDbContext _context;

        private readonly UpdateCustomerCommandValidator _validator;

        public UpdateCustomerCommandValidatorTests()
        {
            var mockDbSet = _customers.AsQueryable().BuildMockDbSet();
            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(x => x.Customers).Returns(mockDbSet.Object);
            _context = mockDbContext.Object;

            _validator = new UpdateCustomerCommandValidator(_context);
        }

        private UpdateCustomerCommand PrepareCommand()
        {
            return new UpdateCustomerCommand()
            {
                CustomerId = 4,
                FirstName = "Any",
                LastName = "Belt",
                DateOfBirth = DateTime.Parse("9/20/1988"),
                PhoneNumber = "+989029058590",
                Email = "abelt9@wikispaces.com",
                BankAccountNumber = "BA17 7369 5676 5293 3471"
            };
        }

        /**
         * All test in this class are valid, if and only if the integrated validator actually accepts
         * our <see cref="PrepareCommand"/> as a valid command.
         */
        [Fact]
        public async Task PreparedCommand_ShouldPass()
        {
            var validationResult = await _validator.ValidateAsync(PrepareCommand());
            validationResult.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public async Task EmptyStringAsPhoneNumber_ShouldFail()
        {
            var command = PrepareCommand();
            command.PhoneNumber = "";
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Phone Number' must not be empty.",
                CustomerValidationErrorMessages.PhoneNumberErrorMessage
            });
        }

        [Theory]
        [InlineData("RandomString")]
        public async Task RandomStringAsPhoneNumber_ShouldFail(string phoneNumber)
        {
            var command = PrepareCommand();
            command.PhoneNumber = phoneNumber;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                CustomerValidationErrorMessages.PhoneNumberErrorMessage
            });
        }

        [Theory]
        [InlineData("+31717805045")]
        public async Task FixedLineNumber_ShouldFail(string phoneNumber)
        {
            var command = PrepareCommand();
            command.PhoneNumber = phoneNumber;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                CustomerValidationErrorMessages.PhoneNumberErrorMessage
            });
        }

        [Theory]
        [InlineData("+989029058590")]
        public async Task MobileNumber_ShouldPass(string phoneNumber)
        {
            var command = PrepareCommand();
            command.PhoneNumber = phoneNumber;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task EmptyStringAsEmail_ShouldFail()
        {
            var command = PrepareCommand();
            command.Email = "";
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Email' must not be empty.",
                "'Email' is not a valid email address."
            });
        }

        [Theory]
        [InlineData("RandomString")]
        public async Task RandomStringAsEmail_ShouldFail(string email)
        {
            var command = PrepareCommand();
            command.Email = email;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Email' is not a valid email address."
            });
        }

        [Theory]
        [InlineData("egreenhow0@oaic.gov.au")]
        public async Task PreviouslyUsedEmailByOthers_ShouldFail(string email)
        {
            var command = PrepareCommand();
            command.Email = email;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                CustomerValidationErrorMessages.UniqueEmailErrorMessage
            });
        }
        
        [Theory]
        [InlineData("egreenhow0@oaic.gov.au")]
        public async Task PreviouslyUsedEmailBySelf_ShouldPass(string email)
        {
            var command = PrepareCommand();
            command.CustomerId = 1;
            command.Email = email;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("abelt9@wikispaces.com")]
        public async Task CorrectAndUnusedEmail_ShouldPass(string email)
        {
            var command = PrepareCommand();
            command.Email = email;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task EmptyStringAsAccountNumber_ShouldFail()
        {
            var command = PrepareCommand();
            command.BankAccountNumber = "";
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                "'Bank Account Number' must not be empty.",
                CustomerValidationErrorMessages.IbanErrorMessage,
            });
        }
        
        [Theory]
        [InlineData("RandomString")]
        public async Task RandomStringAsAccountNumber_ShouldFail(string accountNumber)
        {
            var command = PrepareCommand();
            command.BankAccountNumber = accountNumber;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
            {
                CustomerValidationErrorMessages.IbanErrorMessage,
            });
        }

        [Theory]
        [InlineData("BA17 7369 5676 5293 3471")]
        public async Task IbanAsAccountNumber_ShouldFail(string accountNumber)
        {
            var command = PrepareCommand();
            command.BankAccountNumber = accountNumber;
            var validationResult = await _validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "", "", 6)]
        public async Task MultipleInvalidProperties_ShouldAccumulatesErrors(
            string phoneNumber, string email, string accountNumber, int failureCount)
        {
            var command = PrepareCommand();
            command.PhoneNumber = phoneNumber;
            command.Email = email;
            command.BankAccountNumber = accountNumber;
            var validationResult =await _validator.ValidateAsync(command);
            validationResult.Errors.Count.Should().Be(failureCount);
        }
    }
}