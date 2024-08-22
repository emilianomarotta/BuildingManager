using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain;
using Task = Domain.Task;

namespace DataAccess
{
    public class BuildingManagerContext : DbContext
    {
        public BuildingManagerContext() { }
        public BuildingManagerContext(DbContextOptions options) : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<CompanyAdmin> CompanyAdmins { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Apartments)
                .WithOne(a => a.Building)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Building>()
                .HasOne(b => b.Manager)
                .WithMany()
                .HasForeignKey(b => b.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Role")
                .HasValue<Administrator>("Administrator")
                .HasValue<Manager>("Manager")
                .HasValue<Staff>("Staff")
                .HasValue<CompanyAdmin>("CompanyAdmin");

            modelBuilder.Entity<Apartment>()
                  .HasOne(b => b.Building)
                  .WithMany(a => a.Apartments)
                  .HasForeignKey(t => t.BuildingId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Apartment);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.companyAdmin)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string directory = Directory.GetCurrentDirectory();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();

                var connectionString = configuration.GetConnectionString(@"BuildingManagerDB");
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataAccess"));
            }
        }
    }
}