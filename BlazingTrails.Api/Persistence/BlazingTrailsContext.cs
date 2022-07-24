using BlazingTrails.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Persistence;

// DbContext is a combination of the Repository pattern and the Unit of Work pattern.
// We define collections of our entities using properties with the type of DbSet<T>.
// We can then inject it into our application and use it to access and modify data in the database.
public class BlazingTrailsContext : DbContext
{
    // DbSet<T>: Essentially repositories.
    public DbSet<Trail> Trails => Set<Trail>();
    public DbSet<RouteInstruction> RouteInstructions => Set<RouteInstruction>();

    public BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Hook up the entity configuration classes.
        modelBuilder.ApplyConfiguration(new TrailConfig());
        modelBuilder.ApplyConfiguration(new RouteInstructionConfig());
    }
}
