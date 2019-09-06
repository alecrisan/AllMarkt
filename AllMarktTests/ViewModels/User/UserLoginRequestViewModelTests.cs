using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class UserLoginRequestViewModelTests
    {
        private const string validEmail = "email@email.com";
        private const string validPassword = "123456";

        [Theory]
        [InlineData("email@yahoo.com", validPassword)]
        [InlineData(" testEmail@g.com ", validPassword)]
        [InlineData("  testEmail@g.   com ", validPassword)]
        [InlineData("  test@g.com ", validPassword)]
        public void Email_MustContain_Value(string email, string password)
        {
            GetViewModels(email, password)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("email@yahoo.com", validPassword)]
        [InlineData("email@abc.com", validPassword)]
        [InlineData("email@.com", validPassword)]
        [InlineData("email@com", validPassword)]
        [InlineData("email@gmail.com", validPassword)]
        [InlineData("email@yahoo.fr", validPassword)]
        [InlineData("email@yahoo.abc", validPassword)]
        public void Valid_Email_MustContain_SpecificElementValidation(string email, string password)
        {
            GetViewModels(email, password)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("emailyahoo.com", validPassword)]
        [InlineData("email@", validPassword)]
        [InlineData("email.com", validPassword)]
        [InlineData("@yahoo.com", validPassword)]
        public void Invalid_Email_Should_Retrieve_Error_ValidationResult(string email, string password)
        {
            GetViewModels(email, password)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The Email field is not a valid e-mail address.", new[]{ "Email" })
             });
        }

        [Fact]
        public void Password_CannotBe_LongerThan_64Characters()
        {

            string validPasswordLength = GetFieldWithLength("123456", 64);
            GetViewModels(validEmail, validPasswordLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidPasswordLength = GetFieldWithLength("123456", 65);
            GetViewModels(validEmail, invalidPasswordLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Password must be a string or array type with a maximum length of '64'.", new[]{ "Password"} )
                });
        }

        [Fact]
        public void Password_CannotBe_LessThan_6Characters()
        {

            string validPasswordLength = GetFieldWithLength("123456", 64);
            GetViewModels(validEmail, validPasswordLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidPasswordLength = GetFieldWithLength("12345", 5);
            GetViewModels(validEmail, invalidPasswordLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Password must be a string or array type with a minimum length of '6'.", new[]{ "Password"} )
                });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        public void Email_Password_DisplayName_CannotBe_Null_Empty_OrWhiteSpace(string email, string password)
        {
            GetViewModels(email, password)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The Email field is required.", new[]{ "Email" }),
                    new ValidationResult("The Password field is required.", new[]{ "Password" })
                });
        }

        private static IEnumerable<object> GetViewModels(string email, string password)
        {
            yield return new UserLoginRequestViewModel
            {
                Email = email,
                Password = password
            };
        }

        private string GetFieldWithLength(string text, int length)
        {
            StringBuilder fieldBuild = new StringBuilder();
            string fieldWithCharacters = text;
            int defaultFieldLength = fieldWithCharacters.Length;
            string maxLengthFieldBesideFieldWithCharacters = new string('e', length - defaultFieldLength);
            var field = fieldBuild.Append(fieldWithCharacters).Append(maxLengthFieldBesideFieldWithCharacters);

            return field.ToString();
        }
    }
}
