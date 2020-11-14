using IronMacbeth.BFF.Contract;
using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Periodical> Periodicals { get; set; }

        public DbSet<Thesis> Thesises { get; set; }

        public DbSet<Newspaper> Newspapers { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<StoreBook> StoreBook { get; set; }

        public DbSet<Memory> Memories { get; set; }

        public DbSet<Motherboard> Motherboards { get; set; }

        public DbSet<Processor> Processors { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<StoreMemory> StoreMemories { get; set; }

        public DbSet<StoreMotherboard> StoreMotherboards { get; set; }

        public DbSet<StoreProcessor> StoreProcessors { get; set; }

        public DbSet<StoreVideocard> StoreVideoCards { get; set; }

        internal DbSet<User> Users { get; set; }

        public DbSet<Videocard> VideoCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=localhost;Initial Catalog=IronMacbeth.BFF.Database;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Article>()
              .ToTable(nameof(Article));

            modelBuilder.Entity<Book>()
                .ToTable(nameof(Book));

            modelBuilder.Entity<Newspaper>()
              .ToTable(nameof(Newspaper));

            modelBuilder.Entity<Thesis>()
             .ToTable(nameof(Thesis));

            modelBuilder.Entity<Periodical>()
            .ToTable(nameof(Periodical));

            modelBuilder.Entity<Memory>()
                 .ToTable(nameof(Memory));

            modelBuilder.Entity<Motherboard>()
                .ToTable(nameof(Motherboard));

            modelBuilder.Entity<Processor>()
                .ToTable(nameof(Processor));

            modelBuilder.Entity<Purchase>()
                .ToTable(nameof(Purchase));

            modelBuilder.Entity<Store>()
                .ToTable(nameof(Store));

            modelBuilder.Entity<StoreMemory>()
                .ToTable(nameof(StoreMemory));

            modelBuilder.Entity<StoreMotherboard>()
                .ToTable(nameof(StoreMotherboard));

            modelBuilder.Entity<StoreProcessor>()
                .ToTable(nameof(StoreProcessor));

            modelBuilder.Entity<StoreVideocard>()
                .ToTable(nameof(StoreVideocard));

            modelBuilder.Entity<User>()
                .ToTable(nameof(User))
                .HasKey(nameof(User.Login));

            modelBuilder.Entity<User>()
                .Property(x => x.UserRole).HasColumnName("RoleId");


            modelBuilder.Entity<Videocard>()
                .ToTable(nameof(Videocard));
        }
    }
}
