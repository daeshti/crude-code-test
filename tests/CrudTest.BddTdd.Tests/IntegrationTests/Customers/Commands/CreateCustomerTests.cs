using System;
using System.Threading.Tasks;
using CrudTest.Application.Common.Exceptions;
using CrudTest.Application.Customers.Commands.CreateCustomer;
using CrudTest.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

using static CrudTest.BddTdd.Tests.IntegrationTests.Testing;
namespace CrudTest.BddTdd.Tests.IntegrationTests.Customers.Commands
{
    /**
     * These test need a real database to run.
     */
    public class CreateCustomerTests : TestBase
    {
        private CreateCustomerCommand PrepareCommand()
        {
            return new CreateCustomerCommand
            {
                FirstName = "Ernst",
                LastName = "Greenhow",
                DateOfBirth = DateTime.Parse("9/3/1988"),
                PhoneNumber = "+86 447 365 7383",
                Email = "egreenhow0@oaic.gov.au",
                BankAccountNumber = "LT41 1593 4633 2093 4097"
            };
        }
        
        [Test]
        public async Task ShouldRequireValidPhoneNumber()
        {
            var command = PrepareCommand();
            command.PhoneNumber = "BadNumber";
            
            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
        
        [Test]
        public async Task ShouldRequireValidEmail()
        {
            var command = PrepareCommand();
            command.Email = "BadEmail";

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
        
        [Test]
        public async Task ShouldRequireUniqueEmail()
        {
            var command = PrepareCommand();

            await SendAsync(command);

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
        
        [Test]
        public async Task ShouldRequireValidBankAccountNumber()
        {
            var command = new CreateCustomerCommand();
            command.BankAccountNumber = "BadNumber";
            
            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateCustomers()
        {
            var command = PrepareCommand();

            var customerId = await SendAsync(command);

            var item = await FindAsync<Customer>(customerId);

            item.Should().NotBeNull();
            item!.FirstName.Should().Be(command.FirstName);
            item!.LastName.Should().Be(command.LastName);
            item!.DateOfBirth.Should().Be(command.DateOfBirth);
            item!.PhoneNumber.Should().Be(command.PhoneNumber);
            item!.Email.Should().Be(command.Email);
            item!.BankAccountNumber.Should().Be(command.BankAccountNumber);
        }
    }
}