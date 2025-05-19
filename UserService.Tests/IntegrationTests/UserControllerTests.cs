using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UserService.Tests.IntegrationTests
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WithNotEmptyList()
        {
            var response = await _client.GetAsync("api/users");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            Assert.NotEmpty(content);
        }

        [Fact]
        public async Task GetUser_ReturnsOkResult_WithNotNullUser()
        {
            var response = await _client.GetAsync("api/users/3b6e3995-056a-4c52-a65a-a5d419e23783");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<UserDTO>();

            Assert.NotNull(content);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("api/users/3b6e3995-056a-4c52-a65a-a5d419e23783");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var user = await _client.GetAsync("api/users/3b6e3995-056a-4c52-a65a-a5d419e23783");

            user.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent()
        {
            var dto = new UserForUpdateDTO
            {
                UserName = "R0sale",
                Email = "kvusov@bk.ru",
                FirstName = "K0rill",
                LastName = "Vusov",
                Roles = new List<string> { "Admin" }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PutAsync("api/users/3b6e3995-056a-4c52-a65a-a5d419e23783", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var userResponse = await _client.GetAsync("api/users/3b6e3995-056a-4c52-a65a-a5d419e23783");

            var user = await userResponse.Content.ReadFromJsonAsync<UserDTO>();

            Assert.Equal("K0rill", user.FirstName);
        }

        [Fact]
        public async Task PartiallyUpdateUser_ReturnsNoContent()
        {
            var existingUserId = "3b6e3995-056a-4c52-a65a-a5d419e23783";

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/firstName",
                    value = "UpdatedName"
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(patchDoc),
                Encoding.UTF8,
                "application/json-patch+json"
            );

            var response = await _client.PatchAsync($"/api/users/{existingUserId}", content);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getUserResponse = await _client.GetAsync($"/api/users/{existingUserId}");
            var user = await getUserResponse.Content.ReadFromJsonAsync<UserDTO>();

            user.FirstName.Should().Be("UpdatedName");
        }
    }
}
