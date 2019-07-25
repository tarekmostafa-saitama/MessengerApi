using System.Data.Entity.ModelConfiguration;
using MessengerApi.Core.DbEntities;

namespace MessengerApi.Core.FluentApiConfigurations
{
    public class EventsTracerConfiguration : EntityTypeConfiguration<EventsTracer>
    {
        public EventsTracerConfiguration()
        {
            HasKey(t => t.Id);

            Property(t => t.EventCode).HasColumnType("int");
        }
    }
}