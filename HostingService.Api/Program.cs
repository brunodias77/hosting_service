using System.Reflection;
using HostingService.Application.Users.RegisterUser;
using HostingService.Infra.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataDependencyInjection(builder.Configuration);
//builder.Services.AddDataDependencyInjection(builder.Configuration);
builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).GetTypeInfo().Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

