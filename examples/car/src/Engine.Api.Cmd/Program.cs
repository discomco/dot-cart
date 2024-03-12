using DotCart.Core;
using DotCart.Drivers.EventStoreDB;
using DotCart.Logging;
using Engine.Context;
using Serilog;

DotEnv.FromEmbedded();

var builder = WebApplication
    .CreateBuilder(args);

// builder.Logging

// .ClearProviders()
// .AddSerilog()
//

builder.Logging
    .ClearProviders()
    .AddSerilog();
builder.Host
    .UseSerilog(
        InjectSerilog.AddLoggerFromSettingsOnly()
    );


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config
    => config.CustomSchemaIds(x => x.FullName?.Replace("+", "_")));

builder.Services.AddConsoleLogger();
builder.Services.AddESDBStore();

builder.Services.AddCartwheel();

//builder.Services.AddEventFeeder();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();