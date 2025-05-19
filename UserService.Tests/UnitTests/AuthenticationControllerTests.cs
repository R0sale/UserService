using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Controllers;
using Xunit;
using Entities.Exceptions;

namespace UserService.Tests.UnitTests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async Task RegisterUser_ReturnsStatus201_WhenRegistrationIsSuccessful()
        {
            var userForRegistration = new UserForRegistrationDTO
            {
                Email = "test@mail.com",
                Password = "Secure123!",
            };

            var user = new UserDTO
            {
                Id = Guid.NewGuid(),
                Email = "test@mail.com",
                FirstName = "Dada"
            };

            var identityResult = IdentityResult.Success;

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(x => x.AuthenticationService.RegisterUser(userForRegistration))
                .ReturnsAsync(identityResult);

            mockService.Setup(x => x.UserService.GetUserByEmailAsync(userForRegistration.Email))
                .ReturnsAsync(user);

            var controller = new AuthenticationController(mockService.Object);

            var result = await controller.RegisterUser(userForRegistration);

            var statusResult = Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenRegistrationFails()
        {
            var userForRegistration = new UserForRegistrationDTO
            {
                Email = "test@mail.com",
                Password = "Secure123!",
            };

            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Code = "DuplicateEmail", Description = "Email already taken." }
            };
            var failedResult = IdentityResult.Failed(identityErrors.ToArray());

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(x => x.AuthenticationService.RegisterUser(userForRegistration))
                .ReturnsAsync(failedResult);

            var controller = new AuthenticationController(mockService.Object);

            var result = await controller.RegisterUser(userForRegistration);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkResult_WithToken_WhenCredentialsAreValid()
        {
            var userForAuth = new UserForAuthenticationDTO
            {
                UserName = "userName",
                Password = "Secure123!"
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(x => x.AuthenticationService.ValidateUser(userForAuth))
                .Returns(Task.CompletedTask);

            mockService.Setup(x => x.AuthenticationService.CreateToken())
                .ReturnsAsync("mocked-jwt-token");

            var controller = new AuthenticationController(mockService.Object);

            var result = await controller.Authenticate(userForAuth);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenObj = Assert.IsAssignableFrom<TokenResponse>(okResult.Value);
            Assert.Equal("mocked-jwt-token", tokenObj.Token);
        }

        [Fact]
        public async Task Authenticate_ThrowsUnauthorizedException_WhenCredentialsAreInvalid()
        {
            var userForAuth = new UserForAuthenticationDTO
            {
                UserName = "userName",
                Password = "WrongPassword"
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(x => x.AuthenticationService.ValidateUser(userForAuth))
                .Throws(new InvalidUserNameOrPasswordException("userName"));

            var controller = new AuthenticationController(mockService.Object);

            await Assert.ThrowsAsync<InvalidUserNameOrPasswordException>(() => controller.Authenticate(userForAuth));
        }

    }
}
