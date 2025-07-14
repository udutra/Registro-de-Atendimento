using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistroDeAtendimento.Infrastructure.Data;

namespace Api.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class{
    private SqliteConnection _connection;

    public CustomWebApplicationFactory(){
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder){
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services => {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var contextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(AppDbContext));
            if (contextDescriptor != null)
                services.Remove(contextDescriptor);

           

            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlite(_connection);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing){
        base.Dispose(disposing);

        if (disposing){
            _connection?.Dispose();
        }
    }
}
