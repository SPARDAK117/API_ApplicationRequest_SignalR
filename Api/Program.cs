using Application.Handlers;
using Domain.Interfaces;
using Infrastructure.Hubs;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateApplicationRequestHandler).Assembly,
        typeof(GetAllApplicationRequestsHandler).Assembly
    );
});


builder.Services.AddHostedService<ApplicationStatusUpdater>();
builder.Services.AddScoped<IApplicationRequestRepository, ApplicationRequestRepository>();
builder.Services.AddScoped<CreateApplicationRequestHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<ApplicationRequestsHub>("/applicationRequestsHub");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
