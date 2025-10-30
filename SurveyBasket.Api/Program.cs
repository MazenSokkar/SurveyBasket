using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using SurveyBasket.Api;
using SurveyBasket.Core.Entities;
using SurveyBasket.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
