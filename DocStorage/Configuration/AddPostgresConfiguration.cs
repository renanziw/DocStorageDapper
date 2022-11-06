using DocStorage.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocStorage.Api.Configuration
{
    public static class AddPostgresConfiguration
    {
        public static IServiceCollection AddPostgres(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opts => opts.UseNpgsql("Server=localhost;Database=docstorage;Port=5432;User Id=user;Password=user;"));

            return services;
        }
    }
}
