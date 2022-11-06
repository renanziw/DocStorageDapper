using DocStorage.Model;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class DocumentControllerTests
    {
        [Fact]
        internal async Task ShouldNotListDocumentsUnauthenricated()
        {
            var response = await Helpers.TestContext.Client.GetAsync("Document");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldListDocumentsAsAdmin()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("Document");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldListDocumentssAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.GetAsync("Document");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldNotListDocumentsAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.GetAsync("Document");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotPostDocumentAsRegular()
        {
            var request = new Document("description 1", "category 1", "filename2.txt", "txt");
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.RegularAuthenticatedClient.PostAsync("Document", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldDownloadDocumentAsRegular()
        {
            var documentAccess = new DocumentAccessCreateModel
            {
                DocumentId = 2,
                UserId = 3
            };

            //add user access to document
            var content = new StringContent(JsonSerializer.Serialize(documentAccess), Encoding.UTF8, "application/json");
            var accessResponse = await Helpers.TestContext.AdminAuthenticatedClient.PostAsync("DocumentAccess", content);
            var getResponse = await Helpers.TestContext.RegularAuthenticatedClient.GetAsync("Document/2");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteDocumentAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.DeleteAsync("Document/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteDocumentAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.DeleteAsync("Document/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteDocumentUneuthenticated()
        {
            var response = await Helpers.TestContext.Client.DeleteAsync("Document/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
