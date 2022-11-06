using DocStorage.Domain;
using Microsoft.EntityFrameworkCore;
using DocStorage.Util;

namespace DocStorage.Api.Configuration
{
    public static class StartDatabaseConfiguration
    {
        public static IApplicationBuilder StartDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.CreateDatabase();
            }

            return app;
        }
    }
}
