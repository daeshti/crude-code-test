using CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudTest.Infrastructure.Persistence.Configurations
{
    /**
     * Configures the <see cref="Customer"/> entity for the database.
     * Optimizes the PhoneNumber property for storage.
     */
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(254);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Optimized for storage
            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasConversion(str => PhoneNumberAsULong(str),
                    u64 => ULongAsPhoneNumber(u64))
                .HasColumnType("decimal(15,0)");

            builder.Property(c => c.BankAccountNumber)
                .IsRequired()
                .HasMaxLength(17);
        }

        public static ulong PhoneNumberAsULong(string phoneNumber)
        {
            var plusRemoved = phoneNumber[1..];
            var u64 = ulong.Parse(plusRemoved);
            return u64;
        }

        public static string ULongAsPhoneNumber(ulong u64)
        {
            var plusAdded = $"+{u64}";
            return plusAdded;
        }
    }
}