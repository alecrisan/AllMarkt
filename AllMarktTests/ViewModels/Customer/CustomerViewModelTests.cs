using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.Customer
{
    public class CustomerViewModelTests
    {
        [Theory]
        [InlineData("0123456789")]
        [InlineData("0987657789")]
        public void PhoneNumber_MustBegin_WithZero(string phoneNumber)
        {
            GetViewModelCustomers(phoneNumber, "")
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Fact]
        public void PhoneNumber_MustHave_TenDigits()
        {
            var tenDigitsLength = new string('0', 10);
            var lessThanTenDigitsLength = new string('0', 9);
            var moreThanTenDigitsLength = new string('0', 11);

            GetViewModelCustomers(lessThanTenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[] { new ValidationResult("The field PhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[] { "PhoneNumber" }) });

            GetViewModelCustomers(moreThanTenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[] { new ValidationResult("The field PhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[] { "PhoneNumber" }) });

            GetViewModelCustomers(tenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Fact]
        public void Address_CannotBe_LongerThan255()
        {
            var maxLengthAddress = new string('d', 255);
            var invalidAddress = new string('d', 256);

            GetViewModelCustomers("", maxLengthAddress)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModelCustomers("",invalidAddress)
                 .GetValidationResults()
                 .Should()
                 .AllBeEquivalentTo(new[]{
                        new ValidationResult("The field Address must be a string or array type with a maximum length of '255'.", new[]{ "Address" }) });
        }


        private static IEnumerable<object> GetViewModelCustomers(string phoneNumber= null, string address= null)
        {
            yield return new CustomerViewModel
            {
                Address = address,
                PhoneNumber = phoneNumber
            };
        }
    }
}
