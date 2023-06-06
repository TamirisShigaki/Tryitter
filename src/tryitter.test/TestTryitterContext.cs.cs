using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tryitter.Models;
using tryitter.Repository;

namespace tryitter.Test;

public class TestTryitterContext<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TryitterContext>)
            );

            if (descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<TryitterContext>(options =>
            {
                options.UseInMemoryDatabase("db");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateAsyncScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>())
            {
                try
                {
                    appContext.Database.EnsureDeleted();

                    appContext.Database.EnsureCreated();

                    appContext.Students.AddRange(
                        new Student { Name = "Tamiris", Email = "tamiris@gmail.com", Password = "Senha123*", Status = "Estudando" },
                        new Student { Name = "Joao", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" },
                        new Student { Name = "Binho", Email = "binho@gmail.com", Password = "Senha123*", Status = "Estudando" },
                        new Student { Name = "Frida", Email = "frida@gmail.com", Password = "Senha123*", Status = "Estudando" }
                    );

                    appContext.Posts.AddRange(
                        new Post { Content = "ConteudoAqui 1", CreatAt = new DateTime(2023, 01, 2, 8, 50, 0), UpdatetAt = new DateTime(2023, 06, 4, 6, 35, 0), StudentId = 1 },
                        new Post { Content = "ConteudoAqui 2", CreatAt = new DateTime(2023, 01, 2, 8, 50, 0), UpdatetAt = new DateTime(2023, 06, 4, 6, 35, 0), StudentId = 2 },
                        new Post { Content = "ConteudoAqui 3", CreatAt = new DateTime(2023, 01, 2, 8, 50, 0), UpdatetAt = new DateTime(2023, 06, 4, 6, 35, 0), StudentId = 2 },
                        new Post { Content = "ConteudoAqui 4", CreatAt = new DateTime(2023, 01, 2, 8, 50, 0), UpdatetAt = new DateTime(2023, 06, 4, 6, 35, 0), StudentId = 3 },
                        new Post { Content = "ConteudoAqui 5", CreatAt = new DateTime(2023, 01, 2, 8, 50, 0), UpdatetAt = new DateTime(2023, 06, 4, 6, 35, 0), StudentId = 3 }
                    );

                    appContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        });
    }
}