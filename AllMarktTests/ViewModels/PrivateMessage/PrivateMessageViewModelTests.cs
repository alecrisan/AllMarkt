using AllMarkt.ViewModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.PrivateMessage
{
    public class PrivateMessageViewModelTests
    {
        private const string ValidTitle = "Title";
        private const string ValidText = "Text";
        private const string ValidSentDate = "2019-08-05 14:52";
        private const string ValidReceivedDate = ValidSentDate;
        private const string ValidSender = "user0";
        private const string ValidReceiver = "user0";
        private static IdAndDisplayNameUserViewModel userViewModel = new IdAndDisplayNameUserViewModel() { Id = 0, DisplayName = "user0" };

        [Theory]
        [InlineData("Title", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("   Title    ", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("title", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        public void Title_MustContain_ActualValue(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null, ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(" ", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("\t", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("\r", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData("\n", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        public void Title_CannotBe_Null_Empty_OrWhiteSpace(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Title field is required.", new[]{ "Title" })
                    });
        }

        [Theory]
        [InlineData("1234567890123456789012345", ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        public void Title_CannotExceed_20Characters(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The field Title must be a string or array type with a maximum length of '20'.", new[]{ "Title" })
                    });
        }

        [Theory]
        [InlineData(ValidTitle, "Text", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "   Text    ", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "text", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        public void Text_MustContain_ActualValue(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(ValidTitle, null, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, " ", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "\t", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "\r", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, "\n", ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        public void Text_CannotBe_Null_Empty_OrWhiteSpace(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Text field is required.", new[]{ "Text" })
                    });
        }

        [Theory]
        [InlineData(ValidTitle, ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, ValidText, "2019-05-08 15:46", ValidReceivedDate, ValidSender, ValidReceiver)]
        public void DateSent_MustContain_ActualValue(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(ValidTitle, ValidText, ValidSentDate, ValidReceivedDate, ValidSender, ValidReceiver)]
        [InlineData(ValidTitle, ValidText, ValidSentDate, "2019-05-08 15:47", ValidSender, ValidReceiver)]
        public void DateReceived_MustContain_ActualValue(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(ValidTitle, ValidText, ValidSentDate, ValidReceivedDate, null, ValidReceiver)]
        public void Sender_CannotBe_Null(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Sender field is required.", new[]{ "Sender" })
                    });
        }

        [Theory]
        [InlineData(ValidTitle, ValidText, ValidSentDate, ValidReceivedDate, ValidSender, null)]
        public void Receiver_CannotBe_Null(string title, string text, string dateSent, string dateRead, string sender, string receiver)
        {
            GetViewModels(title, text, dateSent, dateRead, sender, receiver)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Receiver field is required.", new[]{ "Receiver" })
                    });
        }

        private static IEnumerable<object> GetViewModels(string title, string text, string dateSent, string dateRead, string senderName, string receiverName)
        {
            yield return new PrivateMessageViewModel
            {
                Title = title,
                Text = text,
                DateSent = DateTime.ParseExact(dateSent, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                DateRead = DateTime.ParseExact(dateRead, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Sender = senderName != null ? new IdAndDisplayNameUserViewModel() { Id = 0, DisplayName = senderName } : null,
                Receiver = receiverName != null ? new IdAndDisplayNameUserViewModel() { Id = 0, DisplayName = receiverName } : null
            };
        }
    }
}
