using DocStorage.Service.Authorization;
using DocStorage.Service.Interfaces;
using DocStorage.Service.Services;

namespace DocStorage.Api.Configuration
{
    public static class AddServicesConfiguration
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IDocumentAccessService, DocumentAccessService>();

            services.AddTransient<JwtMiddleware>();

            return services;
        }
    }
}
