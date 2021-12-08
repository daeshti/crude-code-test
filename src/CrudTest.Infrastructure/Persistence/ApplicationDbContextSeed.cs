using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrudTest.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!await context.Customers.AnyAsync())
            {
                context.Customers.AddRange(new List<Customer>
                {
                    new()
                    {
                        CustomerId = 1,
                        FirstName = "Ernst",
                        LastName = "Greenhow",
                        DateOfBirth = DateTime.Parse("9/3/1988"),
                        PhoneNumber = "+864473657383",
                        Email = "egreenhow0@oaic.gov.au",
                        BankAccountNumber = "LT41 1593 4633 2093 4097"
                    },
                    new()
                    {
                        CustomerId = 2,
                        FirstName = "Kari",
                        LastName = "Sherrin",
                        DateOfBirth = DateTime.Parse("2/7/1999"),
                        PhoneNumber = "+639963187652",
                        Email = "ksherrin1@surveymonkey.com",
                        BankAccountNumber = "TR33 0558 2GTA SOWM CZ0U MYHI U2"
                    },
                    new()
                    {
                        CustomerId = 3,
                        FirstName = "Winifred",
                        LastName = "Ranvoise",
                        DateOfBirth = DateTime.Parse("8/23/1989"),
                        PhoneNumber = "+622644398525",
                        Email = "wranvoise2@virginia.edu",
                        BankAccountNumber = "GE84 LW82 5931 9842 7913 22"
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }

}