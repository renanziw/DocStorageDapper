using AutoMapper;
using DocStorage.Api;
using DocStorage.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DocStorage.Tests.Helpers
{
    public static class TestContext
    {
        public static IMapper Mapper { get; } = new MapperConfiguration(cfg => cfg.CreateMapper()).CreateMapper();
        public static HttpClient Client 
        { 
            get
            {
                return new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>())
                .CreateClient();
            }
        } 

        public static HttpClient AdminAuthenticatedClient
        {
            get
            {
                var newClient = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>())
                .CreateClient();

                newClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2Njc2NzU1NDksImV4cCI6MTY2ODUzOTU0OSwiaWF0IjoxNjY3Njc1NTQ5fQ.vBM29FRkfTjBAz01kIVYG_wFVsH1DGtvLbo8jK0cTVo");
                
                return newClient;
            }
        }

        public static HttpClient ManagerAuthenticatedClient
        {
            get
            {
                var newClient = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>())
                .CreateClient();

                newClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJuYmYiOjE2Njc2NjkwMTAsImV4cCI6MTY2ODUzMzAxMCwiaWF0IjoxNjY3NjY5MDEwfQ.jzZd0b8F-ILqVP0HGtf9WRsrfyY-DvlRHbHt5m3w2A4");
                
                return newClient;
            }
        }

        public static HttpClient RegularAuthenticatedClient
        {
            get
            {
                var newClient = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>())
                .CreateClient();

                newClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2Njc2NjkwNTIsImV4cCI6MTY2ODUzMzA1MiwiaWF0IjoxNjY3NjY5MDUyfQ.nlxc26EZIkkQ2pCwsvRe0ZO4WXZnD6U29LmNQKr51c8");
                
                return newClient;
            }
        }
    }
}
