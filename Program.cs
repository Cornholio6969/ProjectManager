using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        using var db = new ProjectManagerContext();
        db.Database.EnsureCreatedAsync().Wait();

        // Step 1 - Seed initial data
        seedTasks(db);

        // // Note: This sample requires the database to be created before running.
        // Console.WriteLine($"Database path: {db.DbPath}.");

        // // Create
        // Console.WriteLine("Inserting a new todo");
        // // db.Add(new Todo { Name = "Learn EF Core", IsComplete = false });
        // // await db.SaveChangesAsync();

        // Console.WriteLine("Inserting a new blog");
        // db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
        // await db.SaveChangesAsync();

        // // Read
        // Console.WriteLine("Querying for a blog");
        // var blog = await db.Blogs
        //     .OrderBy(b => b.BlogId)
        //     .FirstAsync();

        // // Update
        // Console.WriteLine("Updating the blog and adding a post");
        // blog.Url = "https://devblogs.microsoft.com/dotnet";
        // blog.Posts.Add(
        //     new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
        // await db.SaveChangesAsync();

        // // Delete
        // Console.WriteLine("Delete the blog");
        // db.Remove(blog);
        // await db.SaveChangesAsync();
    }

    public static async void seedTasks(ProjectManagerContext db)
    {
        try
            {
            db.TeamWorkers.AddRange(
                // Frontend
                new TeamWorker
                {
                    Team = new Team { Name = "Frontend" },
                    Worker = new Worker { Name = "Steen Secher" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Frontend" },
                    Worker = new Worker { Name = "Ejvind Møller" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Frontend" },
                    Worker = new Worker { Name = "Konrad Sommer" }
                },

                // Backend
                new TeamWorker
                {
                    Team = new Team { Name = "Backend" },
                    Worker = new Worker { Name = "Konrad Sommer" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Backend" },
                    Worker = new Worker { Name = "Sofus Lotus" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Backend" },
                    Worker = new Worker { Name = "Remo Lademann" }
                },

                // Testere
                new TeamWorker
                {
                    Team = new Team { Name = "Testere" },
                    Worker = new Worker { Name = "Ella Fanth" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Testere" },
                    Worker = new Worker { Name = "Anne Dam" }
                },
                new TeamWorker
                {
                    Team = new Team { Name = "Testere" },
                    Worker = new Worker { Name = "Steen Secher" }
                }
            );
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        await db.SaveChangesAsync();
    }
}