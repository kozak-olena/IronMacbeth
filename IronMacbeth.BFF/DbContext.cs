using IronMacbeth.BFF.Contract;
using Microsoft.EntityFrameworkCore;

namespace IronMacbeth.BFF
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        internal DbSet<Order> Orders { get; set; }

        public DbSet<ReadingRoomOrder> ReadingRoomOrders { get; set; }

        public DbSet<Periodical> Periodicals { get; set; }

        public DbSet<Thesis> Thesises { get; set; }

        public DbSet<Newspaper> Newspapers { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        internal DbSet<User> Users { get; set; }

        public DbSet<Videocard> VideoCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=localhost;Initial Catalog=IronMacbeth.BFF.Database;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReadingRoomOrder>()
             .ToTable(nameof(ReadingRoomOrder));

            modelBuilder.Entity<Order>()
             .ToTable(nameof(Order));

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
