using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Shared.DTOObjects;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Tests.IntegrationTests
{
    public class AuthenticationControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_WithValidData_ReturnsConfirmationMessage()
        {
            var dto = new UserForRegistrationDTO
            {
                UserName = "newName1",
                Email = "testuser1@mail.ru",
                Password = "Password2",
                FirstName = "Johny",
                LastName = "Doert",
                Roles = new List<string>() { "Admin" }
            };

            var response = await _client.PostAsJsonAsync("/api/authentication", dto);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("check your email");
        }

        [Fact]
        public async Task RegisterUser_WithInvalidData_ReturnsUnprocessableEntity()
        {
            var dto = new UserForRegistrationDTO
            {
                UserName = "new",
                Email = "testuser1@mail.ru",
                Password = "Password2",
                FirstName = "Johny",
                LastName = "Doert",
                Roles = new List<string>() { "Admin" }
            };

            var response = await _client.PostAsJsonAsync("/api/authentication", dto);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task RegisterUser_WithNull_ReturnsBadRequest()
        {
            UserDTO dto = null;

            var response = await _client.PostAsJsonAsync("/api/authentication", dto);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Authenticate_WithValidData_ReturnsOkWithToken()
        {
            var dto = new UserForAuthenticationDTO
            {
                UserName = "R0sale",
                Password = "Nika 2016"
            };

            var response = await _client.PostAsJsonAsync("/api/authentication/login", dto);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
