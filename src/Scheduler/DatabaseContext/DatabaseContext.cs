using Microsoft.EntityFrameworkCore;
using Scheduler.Models;

namespace Scheduler.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<PersonEvent> PersonEvents { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<User> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Event>()
            .HasMany(e => e.People)
            .WithMany(p => p.Events)
            .UsingEntity<PersonEvent>();
        modelBuilder.Entity<Event>().Navigation(e => e.People).AutoInclude();
        modelBuilder.Entity<Person>().Navigation(p => p.PersonEvents).AutoInclude();
    }
}
