using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Program
{
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        using var db = new ProjectManagerContext();
        await db.Database.EnsureCreatedAsync();

        // Step 1 - Seed initial data
        await seedTasks(db);

        //Step 2 - Query and print teams without tasks
        var teamsWithoutTasks = PrintTeamsWithoutTasks(db);
        foreach (var team in teamsWithoutTasks)
        {
            Console.WriteLine($"Team without tasks: {team.Name}");
        }

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

    public static List<Team> PrintTeamsWithoutTasks(ProjectManagerContext db)
    {
        return db.Teams
            .Where(t => !db.Tasks.Any(tsk => EF.Property<int>(tsk, "TeamId") == t.TeamId))
            .ToList();
    }

    public static async System.Threading.Tasks.Task seedTasks(ProjectManagerContext db)
    {
        var frontend = new Team { Name = "Frontend" };
        var backend  = new Team { Name = "Backend" };
        var testere  = new Team { Name = "Testere" };

        // Workers
        var steen  = new Worker { Name = "Steen Secher" };
        var ejvind = new Worker { Name = "Ejvind Møller" };
        var konrad = new Worker { Name = "Konrad Sommer" };
        var sofus  = new Worker { Name = "Sofus Lotus" };
        var remo   = new Worker { Name = "Remo Lademann" };
        var ella   = new Worker { Name = "Ella Fanth" };
        var anne   = new Worker { Name = "Anne Dam" };

        db.TeamWorkers.AddRange(
            new TeamWorker { Team = frontend, Worker = steen },
            new TeamWorker { Team = frontend, Worker = ejvind },
            new TeamWorker { Team = frontend, Worker = konrad },

            new TeamWorker { Team = backend, Worker = konrad },
            new TeamWorker { Team = backend, Worker = sofus },
            new TeamWorker { Team = backend, Worker = remo },

            new TeamWorker { Team = testere, Worker = ella },
            new TeamWorker { Team = testere, Worker = anne },
            new TeamWorker { Team = testere, Worker = steen }
        );

        var feTask = new Task { Name = "Byg landingpage + navigation" };
        var beTask = new Task { Name = "API til projects/workers" };
        var qaTask = new Task { Name = "Testplan + smoke tests" };

        frontend.Tasks = new List<Task> { feTask };
        backend.Tasks  = new List<Task> { beTask };
        // testere.Tasks = new List<Task> { qaTask }; // hvis du vil give dem en senere

        // 👇 VIGTIGT: sæt IKKE CurrentTask endnu
        // frontend.CurrentTask = feTask;
        // backend.CurrentTask = beTask;

        var ejvindCurrentTodo = new Todo { Name = "Implementer navbar", IsComplete = false };
        ejvind.Todos = new List<Todo> { ejvindCurrentTodo };

        var konradCurrentTodo = new Todo { Name = "Sæt API skeleton op", IsComplete = false };
        konrad.Todos = new List<Todo>
        {
            konradCurrentTodo,
            new Todo { Name = "Lav DB queries", IsComplete = false }
        };

        var sofusCurrentTodo = new Todo { Name = "Lav endpoints for workers", IsComplete = false };
        sofus.Todos = new List<Todo> { sofusCurrentTodo };

        var remoCurrentTodo = new Todo { Name = "Auth/validation på API", IsComplete = false };
        remo.Todos = new List<Todo> { remoCurrentTodo };

        db.Teams.AddRange(frontend, backend, testere);
        db.Workers.AddRange(steen, ejvind, konrad, sofus, remo, ella, anne);

        // ✅ Save step 1: indsæt alt uden CurrentTask
        await db.SaveChangesAsync();

        // ✅ Save step 2: nu kan CurrentTask sættes uden cycle
        frontend.CurrentTask = feTask;
        backend.CurrentTask  = beTask;
        // testere.CurrentTask = qaTask; // hvis du giver dem opgaven senere

        // ✅ Save step 3: sæt CurrentTodo efter todos er skrevet
        ejvind.CurrentTodo = ejvindCurrentTodo;
        konrad.CurrentTodo = konradCurrentTodo;
        sofus.CurrentTodo  = sofusCurrentTodo;
        remo.CurrentTodo   = remoCurrentTodo;

        await db.SaveChangesAsync();
    }
}
