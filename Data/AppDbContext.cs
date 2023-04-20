using Microsoft.EntityFrameworkCore;
using MovieStore.Models;

namespace MovieStore.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Movies)
                .WithMany()
                .UsingEntity(x => x.ToTable("OrderMovies"));





        }

            public DbSet<Movie> Movies { get; set; }    
            public DbSet<Actor> Actors { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Review> Review { get; set; }

            public DbSet<Order> Order { get; set; }
            
            public DbSet<Coupon> Coupons { get; set; }

            public DbSet<ApplicationUser> ApplicationUsers { get; set; }


    }
}
