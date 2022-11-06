using DocStorage.Model;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class UserGroupControllerTests
    {
        [Fact]
        internal async Task ShouldList()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("UserGroup");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldPost()
        {
            var request = new UserGroupCreateModel
            {
                GroupId = 1,
                UserId = 2
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.AdminAuthenticatedClient.PostAsync("UserGroup", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotPostAsManager()
        {
            var request = new UserGroupCreateModel
            {
                GroupId = 1,
                UserId = 2
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.PostAsync("UserGroup", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDelete()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.DeleteAsync("UserGroup/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteUneuthenticated()
        {
            var response = await Helpers.TestContext.Client.DeleteAsync("UserGroup/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.DeleteAsync("UserGroup/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.DeleteAsync("UserGroup/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
