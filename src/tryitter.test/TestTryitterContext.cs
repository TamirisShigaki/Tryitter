using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tryitter.Models;
using tryitter.Repository;

namespace tryitter.Test;
// ok
public class TestTryitterContext<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<TryitterContext>));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<TryitterContext>(options =>
            {
                options.UseInMemoryDatabase("db");
            });
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>())
            {
                try
                {

                    appContext.Database.EnsureDeleted();

                    appContext.Database.EnsureCreated();

                    appContext.Students.AddRange(
                  new Student { Name = "joao", Email = "joao@email.com", Password = "12345678", Status = "Aceleração C#" },
                  new Student { Name = "tamiris", Email = "tamiris@email.com", Password = "12345678", Status = "Aceleração C#" },
                  new Student { Name = "claudio", Email = "claudio@email.com", Password = "12345678", Status = "Aceleração C#" },
                  new Student { Name = "shigaki", Email = "shigaki@email.com", Password = "12345678", Status = "Aceleração C#" }
                  );

                    appContext.Posts.AddRange(
                  new Post { Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", CreatAt = new DateTime(2022, 10, 2, 8, 35, 0), UpdatetAt = new DateTime(2022, 10, 3, 8, 35, 0), StudentId = 1 },
                  new Post { Content = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).", CreatAt = new DateTime(2022, 11, 3, 9, 40, 10), UpdatetAt = new DateTime(2022, 12, 1, 10, 37, 22), StudentId = 1 },
                  new Post { Content = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...", CreatAt = new DateTime(2022, 12, 4, 10, 35, 0), UpdatetAt = new DateTime(2022, 12, 5, 8, 57, 20), StudentId = 2 },
                  new Post { Content = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.", CreatAt = new DateTime(2023, 01, 3, 10, 45, 10), UpdatetAt = new DateTime(2023, 01, 3, 11, 38, 10), StudentId = 3 },
                  new Post { Content = "The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from de Finibus Bonorum et Malorum by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.", CreatAt = new DateTime(2023, 12, 3, 10, 55, 0), UpdatetAt = new DateTime(2023, 12, 6, 8, 38, 7), StudentId = 3 }
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