using Microsoft.EntityFrameworkCore;
using WebBookShell.Entities;


namespace WebBookShell.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<MembershipCard> MembershipCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<BookVoucher> BookVouchers { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Quan hệ giữa User và Role (1-N)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Roles)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Role>().HasData(
          new Role { RoleId = 1, RoleName = "Admin" },
          new Role { RoleId = 2, RoleName = "Staff" },
          new Role { RoleId = 3, RoleName = "Customer" }
      );

            // Quan hệ giữa User và Cart (1-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Carts)
                .WithOne(c => c.Users)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa Cart và CartItem (1-N)
            modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)  // CartItem có Cart
            .WithMany(c => c.CartItems)  // Cart có nhiều CartItems
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa CartItem và Book (N-1)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Book)  // CartItem có Book
                .WithMany(b => b.CartItems)  // Book có nhiều CartItems
                .HasForeignKey(ci => ci.BookId)
                .OnDelete(DeleteBehavior.Cascade);


            // Quan hệ giữa Book và Author (N-N)
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorName }); // Cập nhật khóa chính

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<BookAuthor>(
                    j => j.HasOne(ba => ba.Author).WithMany().HasForeignKey(ba => ba.AuthorName),
                    j => j.HasOne(ba => ba.Book).WithMany().HasForeignKey(ba => ba.BookId)
                );

            // Quan hệ giữa Book và Genre (N-N)
            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreName });
            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookGenres)
                .WithOne(bg => bg.Books)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<Genre>()
                .HasMany(g => g.BookGenres)
                .WithOne(bg => bg.Genres)
                .HasForeignKey(bg => bg.GenreName);


            // Quan hệ giữa Order và User (N-1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Users)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa Order và OrderDetail (1-N)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Orders)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa Book và OrderDetail (N-N)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Books)
                .WithMany(b => b.OrderDetails)
                .HasForeignKey(od => od.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa Order và Voucher (N-1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Vouchers)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.VoucherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Quan hệ giữa Book và BookVoucher (N-N)
            modelBuilder.Entity<BookVoucher>()
                .HasKey(bv => new { bv.BookId, bv.VoucherId });
            modelBuilder.Entity<BookVoucher>()
                .HasOne(bv => bv.Books)
                .WithMany(b => b.BookVouchers)
                .HasForeignKey(bv => bv.BookId);
            modelBuilder.Entity<BookVoucher>()
                .HasOne(bv => bv.Vouchers)
                .WithMany(v => v.BookVouchers)
                .HasForeignKey(bv => bv.VoucherId);

            // Quan hệ giữa MembershipCard và User (1-1)
            modelBuilder.Entity<MembershipCard>()
                .HasOne(mc => mc.Users)
                .WithMany(u => u.MembershipCards)
                .HasForeignKey(mc => mc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ giữa Voucher và BookVoucher (1-N)
            modelBuilder.Entity<Voucher>()
                .HasMany(v => v.BookVouchers)
                .WithOne(bv => bv.Vouchers)
                .HasForeignKey(bv => bv.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);
            

        }
    } 
}
