using DocStorage.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DocStorage.Domain
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DbContext> options) : base(options) { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<DocumentAccess> DocumentAccess { get; set; }

        public DataContext CreateDatabase()
        {
            Database.ExecuteSqlRawAsync(Resources.CreateStructure);
                
            var testUsers = new List<User>
            {
                new User { UserName = "Admin", PasswordHash = BCryptNet.HashPassword("admin"), Role = Role.Admin },
                new User { UserName = "Manager", PasswordHash = BCryptNet.HashPassword("manager"), Role = Role.Manager },
                new User { UserName = "Regular", PasswordHash = BCryptNet.HashPassword("regular"), Role = Role.Regular }
            };

            if (!Users.Any(p => testUsers.Select(q => q.UserName).Contains(p.UserName)))
            {
                Users.AddRange(testUsers);
            }

            var groups = new List<Group>
            {
                new Group {Name = "Group 1"},
                new Group {Name = "Group 2"},
                new Group {Name = "Group 3"}
            };

            if (!Groups.Any(p => groups.Select(q => q.Name).Contains(p.Name)))
            {
                Groups.AddRange(groups);
            }

            SaveChanges();

            return this;
        }
    }
}