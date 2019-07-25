using System.Data.Entity;

namespace MessengerApi.Persistence.Identity
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            base.Seed(context);
        }

     
    }

}