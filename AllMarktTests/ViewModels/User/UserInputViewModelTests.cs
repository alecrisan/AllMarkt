using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AllMarkt.ViewModels;
using FluentAssertions;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class UserInputViewModelTests
    {
        [Theory]
        [InlineData("email@yahoo.com", "123456", "dn")]
        [InlineData(" testEmail@g.com ", " 123456 ", " dn ")]
        [InlineData("  testEmail@g.   com ", " 123456 ", " dn ")]
        public void Email_Password_DisplayName_MustContain_Value(string email, string password, string displayName)
        {
            GetViewModels(email, password, displayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("email@yahoo.com", "parola", "display name")]
        [InlineData("email@abc.com", "parola", "display name")]
        [InlineData("email@.com", "parola", "display name")]
        [InlineData("email@com", "parola", "display name")]
        [InlineData("email@gmail.com", "parola", "display name")]
        [InlineData("email@yahoo.fr", "parola", "display name")]
        [InlineData("email@yahoo.abc", "parola", "display name")]
        public void Valid_Email_MustContain_SpecificElementValidation(string validEmail, string validPassword, string validDisplayName)
        {
            GetViewModels(validEmail, validPassword, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("emailyahoo.com", "parola", "display name")]
        [InlineData("email@", "parola", "display name")]
        [InlineData("email.com", "parola", "display name")]
        [InlineData("@yahoo.com", "parola", "display name")]
        public void Invalid_Email_MustContain_SpecificElementValidation(string invalidEmail, string validPassword, string validDisplayName)
        {
            GetViewModels(invalidEmail, validPassword, validDisplayName)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The Email field is not a valid e-mail address.", new[]{ "Email" })
             });
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        [InlineData(" ", " ", " ")]
        public void EmailAndPasswordAndDisplayName_CannotBe_Null_Empty_OrWhiteSpace(string email, string password, string displayName)
        {
            GetViewModels(email, password, displayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The Email field is required.", new[]{ "Email" }),
                    new ValidationResult("The Password field is required.", new[]{ "Password" }),
                    new ValidationResult("The DisplayName field is required.", new[]{ "DisplayName" })
                });
        }

        [Fact]
        public void Email_CannotBeLongerThan_80Characters()
        {
            string validPassword = "parola";
            string validDisplayName = "display name";

            string validEmailLength = GetFieldWithLength("abc@yahoo.com", 80);
            GetViewModels(validEmailLength, validPassword, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidEmailLength = GetFieldWithLength("abc@yahoo.com", 81);
            GetViewModels(invalidEmailLength, validPassword, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Email must be a string or array type with a maximum length of '80'.", new[]{ "Email"}),
                });
        }


        [Fact]
        public void Password_CannotBe_LongerThan_64Characters()
        {
            string validEmail = "emailtest@u.com";
            string validDisplayName = "display";

            string validPasswordLength = GetFieldWithLength("123456", 64);
            GetViewModels(validEmail, validPasswordLength, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidPasswordLength = GetFieldWithLength("123456", 65);
            GetViewModels(validEmail, invalidPasswordLength, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Password must be a string or array type with a maximum length of '64'.", new[]{ "Password"} )
                });
        }


        [Fact]
        public void DisplayName_CannotBe_LongerThan_50Characters()
        {
            string validEmail = "emailtest@u.com";
            string validPassword = "12345";

            string validDisplayNameLength = GetFieldWithLength("nume display", 50);

            GetViewModels(validEmail, validPassword, validDisplayNameLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidDisplayNameLength = GetFieldWithLength("nume display", 51);

            GetViewModels(validEmail, validPassword, invalidDisplayNameLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field DisplayName must be a string or array type with a maximum length of '50'.", new[]{ "DisplayName"} )
                }
                );
        }

        private static IEnumerable<object> GetViewModels(string email, string password, string displayName)
        {
            yield return new UserInputViewModel
            {
                Email = email,
                Password = password,
                DisplayName = displayName
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
