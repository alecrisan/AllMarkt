using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.Shop
{
    public class ShopViewModelTests
    {
        [Theory]
        [InlineData("0123456789")]
        [InlineData("0987657789")]
        public void PhoneNumber_MustBegin_WithZero(string phoneNumber)
        {
            GetViewModelShops(phoneNumber, "", "", "")
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

            GetViewModelShops(lessThanTenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[] { new ValidationResult("The field PhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[] { "PhoneNumber" }) });

            GetViewModelShops(moreThanTenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[] { new ValidationResult("The field PhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[] { "PhoneNumber" }) });

            GetViewModelShops(tenDigitsLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("RO2540803400012C")]
        [InlineData("RO25408034000122")]
        public void CUI_MustBe_OfFormat(string cui)
        {
            GetViewModelShops("", cui)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[] { new ValidationResult("The field CUI must match the regular expression '^RO[0-9]{1,9}[0-9a-zA-Z]{1}$'.", new[] { "CUI" }) });
        }

       
        [Fact]
        public void Address_CannotBe_LongerThan255()
        {
            var maxLengthAddress = new string('d', 255);
            var invalidAddress = new string('d', 256);

            GetViewModelShops("", "", "", maxLengthAddress)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModelShops("", "", "", invalidAddress)
                 .GetValidationResults()
                 .Should()
                 .AllBeEquivalentTo(new[]{
                        new ValidationResult("The field Address must be a string or array type with a maximum length of '255'.", new[]{ "Address" }) });
        }

        private static IEnumerable<object> GetViewModelShops(string phoneNumber= null, string cui = null, string iban = null, string address = null)
        {
            yield return new ShopViewModel
            {
                Address = address,
                CUI = cui,
                IBAN = iban, 
                PhoneNumber = phoneNumber
            };
        }
    }
}
