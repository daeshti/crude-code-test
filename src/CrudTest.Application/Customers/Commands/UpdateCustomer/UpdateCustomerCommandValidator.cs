using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Application.Customers.Commands.CreateCustomer;
using CrudTest.Application.Customers.Common;
using FluentValidation;
using IbanNet;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;

namespace CrudTest.Application.Customers.Commands.UpdateCustomer
{
    /**
     * A validator for UpdateCustomerCommand. Uses an injected instance of DbContext to check uniqueness
     * constraints. Accepts old email as unique email. Uses Google's Phone Number library to validate
     * mobile(and only mobile, not fixed-line numbers. Uses Ibannet library to validate account
     * number as an IBAN. must be always called async.
     */
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly PhoneNumberUtil _phoneNumberUtil;
        private readonly IbanValidator _ibanValidator;

        public UpdateCustomerCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
            _ibanValidator = new IbanValidator();

            // TODO: no validation rules for properties FirstName, LastName, DateOfBirth was discussed

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Must(BePhoneNumber).WithMessage(CustomerValidationErrorMessages.PhoneNumberErrorMessage);

            RuleFor(x => x.BankAccountNumber)
                .NotEmpty()
                .Must(BeIban).WithMessage(CustomerValidationErrorMessages.IbanErrorMessage);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(BeUniqueEmail).WithMessage(CustomerValidationErrorMessages.UniqueEmailErrorMessage);
        }

        private bool BePhoneNumber(UpdateCustomerCommand x, string phoneNumberStr)
        {
            try
            {
                var phoneNumber = _phoneNumberUtil.Parse(phoneNumberStr, x.PhoneNumberNationality);
                return _phoneNumberUtil.IsValidNumber(phoneNumber) &&
                       _phoneNumberUtil.GetNumberType(phoneNumber) == PhoneNumberType.MOBILE;
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

        private bool BeIban(string ibanStr)
        {
            var validationResult = _ibanValidator.Validate(ibanStr);
            return validationResult.IsValid;
        }

        private async Task<bool> BeUniqueEmail(UpdateCustomerCommand x, string newEmail,
            CancellationToken cancellationToken)
        {
            var currentEmail = await _context.Customers
                .Where(c => c.CustomerId == x.CustomerId)
                .Select(c => c.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.Equals(currentEmail, newEmail)) return true;

            return await _context.Customers
                .AllAsync(c => c.Email != newEmail, cancellationToken);
        }
    }
}