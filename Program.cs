using Restaurants.API.Controllers;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Application.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Microsoft.OpenApi.Models;
using Restaurants.API.Extensions;
using Restaurants.Application.Users;
using Restaurants.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddle>();
// Configure the HTTP request pipeline.

app.MapGroup("api/identity")
.WithTags("Identity")
.MapIdentityApi<User>();

app.UseAuthorization();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();
app.MapControllers();


app.Run();
