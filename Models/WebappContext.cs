using Microsoft.EntityFrameworkCore;
 
namespace WebApp.Models
{
    public class WebappContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WebappContext(DbContextOptions<WebappContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Auction> Auctions {get; set;}
        public DbSet<Bid> Bids {get; set;}
        
    }
}