using Microsoft.EntityFrameworkCore;
public class ProjectManagerContext : DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamWorker> TeamWorkers { get; set; }
    public DbSet<Worker> Workers { get; set; }

    public string DbPath { get; }

    public ProjectManagerContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "projectmanager.db");
        DbPath = "projectmanager.db";
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TeamWorker>()
            .HasKey(tw => new { tw.TeamId, tw.WorkerId });
    }
}

public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; }
    public List<Worker> Workers { get; set; }
}

public class TeamWorker
{
    public int TeamId { get; set; }
    public Team Team { get; set; }
    public int WorkerId { get; set; }
    public Worker Worker { get; set; }
}

public class Worker
{
    public int WorkerId { get; set; }
    public string Name { get; set; }
    public List<Team> Teams { get; set; }
}