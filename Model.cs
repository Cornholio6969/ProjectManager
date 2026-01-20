using Microsoft.EntityFrameworkCore;
public class ProjectManagerContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<Task> Tasks {get; set;} 

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
}
public class Todo
{
    public int TodoId { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}
public class Task
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public List<Todo> Todos { get; set; }
}