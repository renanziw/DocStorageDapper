using DocStorage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DocStorage.Tests.Api
{
    public class GroupControllerTests
    {
        [Fact]
        internal async Task ShouldListGroups()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("Group");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldNotListGroupsAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.GetAsync("Group");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldNotListGroupsAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.GetAsync("Group");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        internal async Task ShouldListGroup()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.GetAsync("Group/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldPostGroup()
        {
            var request = new Group
            {
                Name = "Group1"
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.AdminAuthenticatedClient.PostAsync("Group", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteGroup()
        {
            var response = await Helpers.TestContext.AdminAuthenticatedClient.DeleteAsync("Group/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteGroupUneuthenticated()
        {
            var response = await Helpers.TestContext.Client.DeleteAsync("Group/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteGroupAsManager()
        {
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.DeleteAsync("Group/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotDeleteGroupAsRegular()
        {
            var response = await Helpers.TestContext.RegularAuthenticatedClient.DeleteAsync("Group/1");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotPostGroupAsManager()
        {
            var request = new Group
            {
                Name = "Group 2"
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Helpers.TestContext.ManagerAuthenticatedClient.PostAsync("Group", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
