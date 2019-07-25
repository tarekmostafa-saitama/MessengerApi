using System.Data.Entity.ModelConfiguration;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.FluentApiConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            HasMany(i => i.Relations).WithRequired(c => c.User).HasForeignKey(c => c.user_id).WillCascadeOnDelete(true);
        }
    }
}