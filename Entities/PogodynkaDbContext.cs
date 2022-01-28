namespace PogodynkaAPI.Entities
{
    public class PogodynkaDbContext : DbContext
    {
        private string _connectionString = 
            "Server=tcp:pogodynka-db-server.database.windows.net,1433;" +
            "Initial Catalog=PogodynkaDb;Persist Security Info=False;" +
            "User ID=pogodynka-admin;Password=Password12345;MultipleActiveResultSets=False;" +
            "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DbSet<Pogodynka> PogodynkaData { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pogodynka>()
                .Property(p => p.Description)
                .HasMaxLength(250);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            //tworzymy migracje w Konsoli menadżera pekietów (Narzędzia -> Menedżer pakietów NuGet) za pomocą "add-migration NAZWA"
            //potem tworzymy bazę danych z tej migracji za pomocą "update-database"
            //logowanie do localdb za pomocą Microsoft SQL Server Management Studio ( server name => (LocalDb)\MSSQLLocalDB )
        }
    }
}
