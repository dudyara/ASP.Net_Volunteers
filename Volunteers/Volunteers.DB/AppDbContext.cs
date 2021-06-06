namespace Volunteers.DB
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Volunteers.Entities;

    /// <summary>
    /// AppDbContext - контекст подключения к бд
    /// </summary>
    public sealed class AppDbContext : IdentityDbContext<User, Role, long>
    {
        /// <summary>
        /// AppDbContext
        /// </summary>
        /// <param name="options">options</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Funds - таблица фондов.
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Requests - таблица запросов.
        /// </summary>
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// ActivityTypes - таблица типов помощи.
        /// </summary>
        public DbSet<ActivityType> ActivityTypes { get; set; }

        /// <summary>
        /// Инициализация начальных данных
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Request>().Property(u => u.FIO).HasColumnType("varchar(500)");
            modelBuilder.Entity<Request>().Property(u => u.Description).HasColumnType("varchar(500)");
            modelBuilder.Entity<Request>().Property(u => u.Comment).HasColumnType("varchar(500)");
            modelBuilder.Entity<Organization>().Property(u => u.Name).HasColumnType("varchar(500)");
            modelBuilder.Entity<Organization>().Property(u => u.Description).HasColumnType("varchar(500)");
            modelBuilder.Entity<Organization>().Property(u => u.ChiefFIO).HasColumnType("varchar(500)");
            modelBuilder.Entity<Organization>().Property(u => u.Mail).HasColumnType("varchar(500)");
            modelBuilder.Entity<Organization>().Property(u => u.Address).HasColumnType("varchar(500)");

            modelBuilder
                .Entity<Role>().HasData(
                new Role[]
                {
                     new Role { Id = 1, Name = "Admin" },
                     new Role { Id = 2, Name = "Organization" }
                })
                ;
            modelBuilder
                .Entity<Organization>()
                .HasMany(a => a.ActivityTypes)
                .WithMany(o => o.Organizations)
                .UsingEntity<ActivityTypeOrganization>(
                    j => j
                    .HasOne(pt => pt.ActivityType)
                    .WithMany(t => t.ActivityTypeOrganizations)
                    .HasForeignKey(pt => pt.ActivityTypeId),
                    j => j
                    .HasOne(pt => pt.Organization)
                    .WithMany(p => p.ActivityTypeOrganizations)
                    .HasForeignKey(pt => pt.OrganizationId));
        }
    }
}
