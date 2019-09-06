using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AllMarkt.ViewModels;
using FluentAssertions;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class UserEditViewModelTests
    {
        [Theory]
        [InlineData("email@yahoo.com", "parola", "parola2", "display name")]
        [InlineData("email@abc.com", "parola", "parola2", "display name")]
        [InlineData("email@.com", "parola", "parola2", "display name")]
        [InlineData("email@com", "parola", "parola2", "display name")]
        [InlineData("email@gmail.com", "parola", "parola2", "display name")]
        [InlineData("email@yahoo.fr", "parola", "parola2", "display name")]
        [InlineData("email@yahoo.abc", "parola", "parola2", "display name")]
        public void Valid_Email_MustContain_SpecificElementValidation(string validEmail, string validOldPassword, string validPassword, string validDisplayName)
        {
            GetViewModels(validEmail, validOldPassword, validPassword, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("emailyahoo.com", "parola2", "parola", "display name")]
        [InlineData("email@", "parola2", "parola", "display name")]
        [InlineData("email.com", "parola2", "parola", "display name")]
        [InlineData("@yahoo.com", "parola2", "parola", "display name")]
        public void Invalid_Email_MustContain_SpecificElementValidation(string invalidEmail, string validOldPassword, string validPassword, string validDisplayName)
        {
            GetViewModels(invalidEmail, validOldPassword, validPassword, validDisplayName)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The Email field is not a valid e-mail address.", new[]{ "Email" })
             });
        }

        [Fact]
        public void Email_CannotBeLongerThan_80Characters()
        {
            string validPassword = "parola";
            string validDisplayName = "display name";
            string validOldPassword = "parola";

            string validEmailLength = GetFieldWithLength("abc@yahoo.com", 80);
            GetViewModels(validEmailLength, validOldPassword, validPassword, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidEmailLength = GetFieldWithLength("abc@yahoo.com", 81);
            GetViewModels(invalidEmailLength, validOldPassword, validPassword, validDisplayName)
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
            string validOldPasswordLength = GetFieldWithLength("123456", 64);

            string validPasswordLength = GetFieldWithLength("123456", 64);
            GetViewModels(validEmail, validOldPasswordLength, validPasswordLength, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidPasswordLength = GetFieldWithLength("123456", 65);
            GetViewModels(validEmail, validOldPasswordLength, invalidPasswordLength, validDisplayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field NewPassword must be a string or array type with a maximum length of '64'.", new[]{ "NewPassword"} )
                });
        }


        [Fact]
        public void DisplayName_CannotBe_LongerThan_50Characters()
        {
            string validEmail = "emailtest@u.com";
            string validPassword = "12345";
            string validOldPassword = "145236";

            string validDisplayNameLength = GetFieldWithLength("nume display", 50);

            GetViewModels(validEmail, validOldPassword, validPassword, validDisplayNameLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidDisplayNameLength = GetFieldWithLength("nume display", 51);

            GetViewModels(validEmail, validOldPassword, validPassword, invalidDisplayNameLength)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field DisplayName must be a string or array type with a maximum length of '50'.", new[]{ "DisplayName"} )
                }
                );
        }

        private static IEnumerable<object> GetViewModels(string email, string oldPassword, string password, string displayName)
        {
            yield return new UserEditViewModel
            {
                Email = email,
                OldPassword = oldPassword,
                NewPassword = password,
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
