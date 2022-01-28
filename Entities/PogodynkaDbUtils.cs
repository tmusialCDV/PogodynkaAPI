namespace PogodynkaAPI.Entities
{
    public class PogodynkaDbUtils
    {
        private readonly PogodynkaDbContext dbContext;

        public PogodynkaDbUtils(PogodynkaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AutoMigrations()
        {
            if (dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
