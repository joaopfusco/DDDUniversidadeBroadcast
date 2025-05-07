using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using DDDUniversidadeBroadcast.API.Extensions;
using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Data;
using DDDUniversidadeBroadcast.Service.Interfaces;
using DDDUniversidadeBroadcast.Service.Services;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Subscriber;

#nullable disable

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    .AddOData(opt => opt.Select()
        .Expand()
        .SetMaxTop(null)
        .SkipToken()
        .OrderBy()
        .Count()
        .Filter()
        .EnableQueryFeatures(1000)
    )
    .AddODataNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
{
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
    .WithJobExpirationTimeout(TimeSpan.FromDays(10)));

builder.Services.AddHangfireServer();

builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
    policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocorreu um erro ao atualizar o banco de dados.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

var subscriberDb = scope.ServiceProvider.GetRequiredService<SubscriberDb>();
var subcriberEmail = scope.ServiceProvider.GetRequiredService<SubscriberEmail>();
var subscriberSms = scope.ServiceProvider.GetRequiredService<SubscriberSms>();

//CRONS
RecurringJob.AddOrUpdate(
     "SubscriberDb",
     () => subscriberDb.SubscribeAsync(),
     Cron.Minutely);

RecurringJob.AddOrUpdate(
    "SubscriberEmail",
    () => subcriberEmail.SubscribeAsync(),
    Cron.Minutely);

RecurringJob.AddOrUpdate(
    "SubscriberSms",
    () => subscriberSms.SubscribeAsync(),
    Cron.Minutely);

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
