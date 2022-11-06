using DocStorage.Model;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class DocumentAccessControllerTests
    {
        [Fact]
        internal async Task ShouldList()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("DocumentAccess");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldPost()
        {
            var request = new DocumentAccessCreateModel
            {
                DocumentId = 1,
                GroupId = 1,
                UserId = 2
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.AdminAuthenticatedClient.PostAsync("DocumentAccess", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotPostAsManager()
        {
            var request = new DocumentAccessCreateModel
            {
                DocumentId = 1,
                GroupId = 1,
                UserId = 2
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.PostAsync("DocumentAccess", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDelete()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.DeleteAsync("DocumentAccess/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteUneuthenticated()
        {
            var response = await Helpers.TestContext.Client.DeleteAsync("DocumentAccess/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.DeleteAsync("DocumentAccess/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.DeleteAsync("DocumentAccess/4");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
