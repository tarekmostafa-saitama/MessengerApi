using System.Data.Entity.ModelConfiguration;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.FluentApiConfigurations
{
    public class MessagesConfiguration : EntityTypeConfiguration<Message>
    {
        public MessagesConfiguration()
        {
            HasKey(t => t.Id);

            Property(t => t.Sender).HasColumnType("int");

            Property(t => t.Type).HasColumnType("int");

            

        }
    }
}