using System.Security.Cryptography.X509Certificates;

using IronMacbeth.Model.ToBeRemoved;

using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Memory> Memories { get; set; }

        public DbSet<Motherboard> Motherboards { get; set; }

        public DbSet<Processor> Processors { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<StoreMemory> StoreMemories { get; set; }

        public DbSet<StoreMotherboard> StoreMotherboards { get; set; }

        public DbSet<StoreProcessor> StoreProcessors { get; set; }

        public DbSet<StoreVideocard> StoreVideoCards { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Videocard> VideoCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=localhost;Initial Catalog=IronMacbeth.BFF.Database;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .ToTable(nameof(Book))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage)
                .Ignore("_description")
                .Ignore(x => x.Description);

            modelBuilder.Entity<Memory>()
                .ToTable(nameof(Memory))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage)
                .Ignore("_description")
                .Ignore(x => x.Description);

            modelBuilder.Entity<Motherboard>()
                .ToTable(nameof(Motherboard))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage)
                .Ignore("_description")
                .Ignore(x => x.Description);

            modelBuilder.Entity<Processor>()
                .ToTable(nameof(Processor))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage)
                .Ignore("_description")
                .Ignore(x => x.Description);

            modelBuilder.Entity<Purchase>()
                .ToTable(nameof(Purchase))
                .Ignore(x => x.IsMarkedAsRead);

            modelBuilder.Entity<Store>()
                .ToTable(nameof(Store))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage);

            modelBuilder.Entity<StoreMemory>()
                .ToTable(nameof(StoreMemory))
                .Ignore(x => x.Modified)
                .Ignore(x => x.SellableId);

            modelBuilder.Entity<StoreMotherboard>()
                .ToTable(nameof(StoreMotherboard))
                .Ignore(x => x.Modified)
                .Ignore(x => x.SellableId);

            modelBuilder.Entity<StoreProcessor>()
                .ToTable(nameof(StoreProcessor))
                .Ignore(x => x.Modified)
                .Ignore(x => x.SellableId);

            modelBuilder.Entity<StoreVideocard>()
                .ToTable(nameof(StoreVideocard))
                .Ignore(x => x.Modified)
                .Ignore(x => x.SellableId);

            modelBuilder.Entity<User>()
                .ToTable(nameof(User))
                .HasNoKey();

            modelBuilder.Entity<Videocard>()
                .ToTable(nameof(Videocard))
                .Ignore("_bitmapImage")
                .Ignore(x => x.BitmapImage)
                .Ignore("_description")
                .Ignore(x => x.Description);
        }

    }
}
