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

        //Step 2 - Query and display data
        using (var context = new ProjectManagerContext())
        {
            var tasks = context.Tasks
                .Include(t => t.Todos);

            foreach (var task in tasks)
            {
                Console.WriteLine($"Task: {task.Name}");
                foreach (var todo in task.Todos)
                {
                    Console.WriteLine($"\tTodo: {todo.Name}, Completed: {todo.IsComplete}");
                }
            }
        }

        // Step 3 - Query and display incomplete Tasks and Todos using Linq
        using (var context = new ProjectManagerContext())
        {
            var incompleteTasks = context.Tasks
                .Where(t => t.Todos.Any(td => !td.IsComplete))
                .Include(t => t.Todos);

            Console.WriteLine("\nIncomplete Tasks and their Todos:");
            foreach (var task in incompleteTasks)
            {
                Console.WriteLine($"Task: {task.Name}");
                foreach (var todo in task.Todos.Where(td => !td.IsComplete))
                {
                    Console.WriteLine($"\tTodo: {todo.Name}, Completed: {todo.IsComplete}");
                }
            }
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

    public static async void seedTasks(ProjectManagerContext db)
    {
        db.Add(new Task
        {
            Name = "Produce software",
            Todos = new List<Todo>
            {
                new Todo { Name = "Write code", IsComplete = false },
                new Todo { Name = "Compile source", IsComplete = false },
                new Todo { Name = "Test program", IsComplete = false }
            }
        });

        db.Add(new Task
        {
            Name = "Brew coffee",
            Todos = new List<Todo>
            {
                new Todo { Name = "Pour water", IsComplete = false },
                new Todo { Name = "Pour coffee", IsComplete = false },
                new Todo { Name = "Turn on", IsComplete = false }
            }
        });

        await db.SaveChangesAsync();
    }
}