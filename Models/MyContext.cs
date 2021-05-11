using Microsoft.EntityFrameworkCore;
namespace TheOne.Models{

public class MyContext : DbContext{

    public MyContext(DbContextOptions Options) : base(Options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Participant> Players { get; set; }

    public DbSet<Event> Events { get; set; }

}
}