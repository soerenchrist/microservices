using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepareDb
{
    public static void Populate(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        if (context == null)
            throw new Exception("No AppDbContext found!");
        
        SeedData(context);
    }

    private static void SeedData(AppDbContext context)
    {
        if (!context.Platforms.Any())
        {
            Console.WriteLine("Seeding data...");
            context.Platforms.AddRange(new []
            {
                new Platform
                {
                    Name = ".Net",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform
                {
                    Name = "SQL Server",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform
                {
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free"
                }
            });
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("We already have data in the db");
        }
    }
}