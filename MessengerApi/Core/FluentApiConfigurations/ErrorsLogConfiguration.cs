using System.Data.Entity.ModelConfiguration;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.FluentApiConfigurations
{
    public class ErrorsLogConfiguration : EntityTypeConfiguration<ErrorsLog>
    {
        public ErrorsLogConfiguration()
        {
            HasKey(t => t.Id);
        }
    }
}