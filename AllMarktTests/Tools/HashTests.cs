using AllMarkt.Tools;
using FluentAssertions;
using System;
using Xunit;

namespace AllMarktTests.Tools
{
    public class HashTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ComputeSha256Hash_CannotBe_NullOrWhiteSpace(string text)
        {
            var errorMessage = Assert.Throws<ArgumentNullException>(() => Hash.ComputeSha256Hash(text));
            errorMessage.Message.Should().Be("Value cannot be null.\r\nParameter name: The row data cannot be empty.");
        }

        [Fact]
        public void ComputeSha256Hash_Return_ExactString()
        {
            //Arrange
            var initialPassword = "5dc088487fb505024591604c00eadbd8607ea049dc46857eb803b45e205640f6";

            //Act
            var hashPassword = Hash.ComputeSha256Hash("1234566");

            //Assert      
            initialPassword.Should().Be(hashPassword);
        }
    }
}
