using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        // TeamWorkers (kobling)
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

        // Tasks pr team (sørger for at alle teams har en opgave)
        var feTask = new Task
        {
            Name = "Byg landingpage + navigation",
            Todos = new List<Todo>
            {
                new Todo { Name = "Lav header + menu", IsComplete = false },
                new Todo { Name = "Implementer hero + CTA", IsComplete = false },
                new Todo { Name = "Styling (responsive)", IsComplete = false }
            }
        };

        var beTask = new Task
        {
            Name = "API til projects/workers",
            Todos = new List<Todo>
            {
                new Todo { Name = "CRUD endpoints", IsComplete = false },
                new Todo { Name = "Validering + errors", IsComplete = false },
                new Todo { Name = "Seed data routes/test", IsComplete = false }
            }
        };

        var qaTask = new Task
        {
            Name = "Testplan + smoke tests",
            Todos = new List<Todo>
            {
                new Todo { Name = "Skriv testcases", IsComplete = false },
                new Todo { Name = "Kør smoke test", IsComplete = false },
                new Todo { Name = "Rapportér bugs", IsComplete = false }
            }
        };

        // Knyt tasks til teams + sæt CurrentTask
        frontend.Tasks = new List<Task> { feTask };
        frontend.CurrentTask = feTask;

        backend.Tasks = new List<Task> { beTask };
        backend.CurrentTask = beTask;

        testere.Tasks = new List<Task> { qaTask };
        testere.CurrentTask = qaTask;

        // Todos pr worker + sæt CurrentTodo (så alle har en)
        steen.Todos = new List<Todo>
        {
            new Todo { Name = "Review UI + find fejl", IsComplete = false },
            new Todo { Name = "Fix små UI issues", IsComplete = false }
        };
        steen.CurrentTodo = steen.Todos[0];

        ejvind.Todos = new List<Todo>
        {
            new Todo { Name = "Implementer navbar", IsComplete = false }
        };
        ejvind.CurrentTodo = ejvind.Todos[0];

        konrad.Todos = new List<Todo>
        {
            new Todo { Name = "Sæt API skeleton op", IsComplete = false },
            new Todo { Name = "Lav DB queries", IsComplete = false }
        };
        konrad.CurrentTodo = konrad.Todos[0];

        sofus.Todos = new List<Todo>
        {
            new Todo { Name = "Lav endpoints for workers", IsComplete = false }
        };
        sofus.CurrentTodo = sofus.Todos[0];

        remo.Todos = new List<Todo>
        {
            new Todo { Name = "Auth/validation på API", IsComplete = false }
        };
        remo.CurrentTodo = remo.Todos[0];

        ella.Todos = new List<Todo>
        {
            new Todo { Name = "Skriv testcases til Frontend", IsComplete = false }
        };
        ella.CurrentTodo = ella.Todos[0];

        anne.Todos = new List<Todo>
        {
            new Todo { Name = "Kør smoke tests på API", IsComplete = false }
        };
        anne.CurrentTodo = anne.Todos[0];

        // Tilføj top-level entities (EF tracker resten via relations)
        db.Teams.AddRange(frontend, backend, testere);
        db.Workers.AddRange(steen, ejvind, konrad, sofus, remo, ella, anne);

        await db.SaveChangesAsync();
    }
}