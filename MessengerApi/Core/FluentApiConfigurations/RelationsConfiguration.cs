using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MessengerApi.Core.DbEntities;


namespace MessengerApi.Core.FluentApiConfigurations
{
    public class RelationsConfiguration :EntityTypeConfiguration<Relation>
    {
        public RelationsConfiguration()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasMany(i => i.Messages).WithRequired(c => c.Relation).HasForeignKey(c => c.relation_id).WillCascadeOnDelete(true);
        }
    }
}