using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using AuctionHub.Infrastructure.Extensions;
using AuctionHub.Application.Extensions.UseCases;
using AuctionHub.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services
    .AddServices()
    .AddUseCases()
    .ConfigureInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });
}

app.Services.ConfigureMigrations();

app.AddEndpoints();

app.Run();