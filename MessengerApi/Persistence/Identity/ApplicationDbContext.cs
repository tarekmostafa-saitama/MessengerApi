using System.Data.Entity;
using MessengerApi.Core.DbEntities;
using MessengerApi.Core.FluentApiConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MessengerApi.Persistence.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<ErrorsLog> LogFile { get; set; }
        public DbSet<EventsTracer> EventsTracer { get; set; }
        public ApplicationDbContext()
            : base("DbConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        static ApplicationDbContext()
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RelationsConfiguration());
            modelBuilder.Configurations.Add(new MessagesConfiguration());
            modelBuilder.Configurations.Add(new EventsTracerConfiguration());
            modelBuilder.Configurations.Add(new ErrorsLogConfiguration());

            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");

            base.OnModelCreating(modelBuilder);

        }
    }
}
