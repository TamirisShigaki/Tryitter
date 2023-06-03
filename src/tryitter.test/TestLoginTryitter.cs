using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace tryitter.Models.Tests
{
    public class TestLoginTryitter
    {
        [Fact]
        public void Email_ShouldBeRequired()
        {
            var studentLogin = new StudentLogin();

            var result = studentLogin.ValidateProperty(nameof(StudentLogin.Email));

            result.Should().ContainSingle().Which.ErrorMessage.Should().Be("The Email field is required.");
        }

        [Theory]
        [InlineData("erroremail")]
        [InlineData("emailinvalid.com")]
        public void Email_ShouldBeValidEmailAddress(string email)
        {
            // Arrange
            var studentLogin = new StudentLogin
            {
                Email = email
            };

            // Act
            var result = studentLogin.ValidateProperty(nameof(StudentLogin.Email));

            // Assert
            result.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("The Email field is not a valid e-mail address.");
        }

        [Fact]
        public void Password_ShouldBeRequired()
        {
            // Arrange
            var studentLogin = new StudentLogin();

            // Act
            var result = studentLogin.ValidateProperty(nameof(StudentLogin.Password));

            // Assert
            result.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("The Password field is required.");
        }

        [Theory]
        [InlineData("short")]
        [InlineData("9874561")]
        public void Password_ShouldHaveMinLengthOfEight(string password)
        {
            // Arrange
            var studentLogin = new StudentLogin
            {
                Password = password
            };

            // Act
            var result = studentLogin.ValidateProperty(nameof(StudentLogin.Password));

            // Assert
            result.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Password must be at least 8 characters long");
        }
    }
}