using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add the ProductRepository to the container
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// add the GanericRepository to the container
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try {
    // seed the database
    // this is to ensure that the database is seeded with the data from the json file
    using var scope = app.Services.CreateScope(); // this is to create a scope for the services
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync(); // this is to apply the migrations to the database
    Console.WriteLine("Migrations applied");
    await StoreContextSeed.SeedAsync(context); // this is to seed the database with the data from the json file
} catch (Exception ex) {
    Console.WriteLine(ex);
}

app.Run();
