using Microsoft.EntityFrameworkCore;
public class ProjectManagerContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Todo> Todos { get; set; }
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

        // Many-to-many: Team <-> Worker
        modelBuilder.Entity<TeamWorker>()
            .HasKey(tw => new { tw.TeamId, tw.WorkerId });

        modelBuilder.Entity<TeamWorker>()
            .HasOne(tw => tw.Team)
            .WithMany()
            .HasForeignKey(tw => tw.TeamId);

        modelBuilder.Entity<TeamWorker>()
            .HasOne(tw => tw.Worker)
            .WithMany()
            .HasForeignKey(tw => tw.WorkerId);

        // Team -> Tasks (1-to-many)
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Team -> CurrentTask (1-to-1 / optional)
        modelBuilder.Entity<Team>()
            .HasOne(t => t.CurrentTask)
            .WithMany()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Worker -> Todos (1-to-many)
        modelBuilder.Entity<Worker>()
            .HasMany(w => w.Todos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Worker -> CurrentTodo (1-to-1 / optional)
        modelBuilder.Entity<Worker>()
            .HasOne(w => w.CurrentTodo)
            .WithMany()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Tasks)
            .WithOne()
            .HasForeignKey("TeamId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Worker>()
            .HasMany(w => w.Todos)
            .WithOne()
            .HasForeignKey("WorkerId")
            .OnDelete(DeleteBehavior.Cascade);

    }
}

public class Task
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public List<Todo> Todos { get; set; }
}

public class Todo
{
    public int TodoId { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}

public class Team
{
    public int TeamId { get; set; }
    public string Name { get; set; }
    public List<Worker> Workers { get; set; }
    public Task? CurrentTask { get; set; }
    public List<Task> Tasks { get; set; }
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
    public Todo? CurrentTodo { get; set; }
    public List<Todo> Todos { get; set; }
}
