using DocStorage.Model;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class UserControllerTests
    {
        [Fact]
        public async Task ShouldPostToAuthenticate()
        {
            var userRequest = new AuthenticateUserRequest
            {
                UserName = "Admin",
                Password = "admin"
            };

            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.Client.PostAsync("User/Authenticate", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldListUsers()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("User");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldListUser()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("User/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldPostUser()
        {
            var userRequest = new User
            {
                UserName = "Test",
                Password = "test",
                Role = Util.Role.Admin,
            };

            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.AdminAuthenticatedClient.PostAsync("User", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteUser()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.DeleteAsync("User/4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteUserUneuthenticated()
        {
            var response = await Helpers.TestContext.Client.DeleteAsync("User/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteUserAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.DeleteAsync("User/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotPostUserAsManager()
        {
            var userRequest = new User
            {
                UserName = "Test",
                Password = "test",
                Role = Util.Role.Admin,
            };

            var content = new StringContent(JsonSerializer.Serialize(userRequest), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.PostAsync("User", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
