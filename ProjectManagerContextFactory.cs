using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ProjectManagerContextFactory : IDesignTimeDbContextFactory<ProjectManagerContext>
{
    public ProjectManagerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectManagerContext>();
        // Same SQLite connection as your context uses
        var dbPath = System.IO.Path.Join(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
            "projectmanager.db"
        );

        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new ProjectManagerContext();
    }
}
