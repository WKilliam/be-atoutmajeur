using be_atoutmajeur.Models.Entities.Comment;
using be_atoutmajeur.Models.Entities.Orders;
using be_atoutmajeur.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace be_atoutmajeur.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrdersEntity> Orders { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
    }
}