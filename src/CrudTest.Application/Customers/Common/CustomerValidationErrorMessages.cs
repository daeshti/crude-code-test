namespace CrudTest.Application.Customers.Common
{
    public static class CustomerValidationErrorMessages
    {
        // TODO: Globalize the error message solution using resources at some point.
        // DO NOT use constants because of .net constant resolution issues.
        
        public static readonly string PhoneNumberErrorMessage = "'Phone Number' is not a valid mobile number.";
        public static readonly string IbanErrorMessage = "'Bank Account Number' is not a valid iban.";
        public static readonly string UniqueEmailErrorMessage = "'Email' is not a unique email address.";
    }
}