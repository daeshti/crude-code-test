using System.Threading;
using System.Threading.Tasks;
using CrudTest.Application.Common.Interfaces;
using CrudTest.Application.Customers.Common;
using FluentValidation;
using IbanNet;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;

namespace CrudTest.Application.Customers.Commands.CreateCustomer
{
    /**
     * A validator for CreateCustomerCommand. Uses an injected instance of DbContext to cheque uniqueness
     * constraints. Uses Google's Phone Number library to validate mobile(and only mobile, not
     * fixed-line numbers. Uses Ibannet library to validate account number as an IBAN. must
     * be always called async.
     */
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly PhoneNumberUtil _phoneNumberUtil;
        private readonly IbanValidator _ibanValidator;

        public CreateCustomerCommandValidator(IApplicationDbContext context)
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

        private bool BePhoneNumber(CreateCustomerCommand x, string phoneNumberStr)
        {
            try
            {
                const string internationalRegion = "ZZ";
                var phoneNumber = _phoneNumberUtil.Parse(phoneNumberStr, internationalRegion);
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

        private Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return _context.Customers
                .AllAsync(c => c.Email != email, cancellationToken);
        }
    }
}