using Catalog.Data;
//using Catalog.RabbitMq;
using Catalog.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)
    )
);

//builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

builder.Services.AddSingleton<RabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBusConnection"],
        Port = Convert.ToInt32(builder.Configuration["EventBusConnectionPort"])
    };

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusUserName"]))
    {
        factory.UserName = builder.Configuration["EventBusUserName"];
    }

    if (!string.IsNullOrEmpty(builder.Configuration["EventBusPassword"]))
    {
        factory.Password = builder.Configuration["EventBusPassword"];
    }

    return new RabbitMQPersistentConnection(factory);
});

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

app.UseRabbitListener();

app.Run();

public static class ApplicationBuilderExtentions
{
    public static RabbitMQPersistentConnection Listener { get; set; }

    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
    {
        Listener = app.ApplicationServices.GetService<RabbitMQPersistentConnection>();
        var life = app.ApplicationServices.GetService<Microsoft.Extensions.Hosting.IApplicationLifetime>();
        life.ApplicationStarted.Register(OnStarted);

        //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
        life.ApplicationStopping.Register(OnStopping);
        return app;
    }

    private static void OnStarted()
    {
        Listener.CreateConsumerChannel();
    }

    private static void OnStopping()
    {
        Listener.Disconnect();
    }
}