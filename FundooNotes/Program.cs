using BusinessLayer.Interface;
using BusinessLayer.Service;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Utility;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    // Bind producer configuration
    var producerConfiguration = new ProducerConfig();
    builder.Configuration.Bind("producerconfiguration", producerConfiguration);
    builder.Services.AddSingleton<ProducerConfig>(producerConfiguration);

    // Directly integrate topic creation logic
    var topicName = builder.Configuration["Kafka:TopicName"];
    var numPartitions = int.Parse(builder.Configuration["Kafka:NumPartitions"]);
    var replicationFactor = short.Parse(builder.Configuration["Kafka:ReplicationFactor"]);

    var config = new AdminClientConfig
    {
        BootstrapServers = builder.Configuration["producerconfiguration:bootstrapservers"]
    };

    using (var adminClient = new AdminClientBuilder(config).Build())
    {
        try
        {
            adminClient.CreateTopicsAsync(new List<TopicSpecification> {
            new TopicSpecification { Name = topicName, NumPartitions = numPartitions, ReplicationFactor = replicationFactor }
        }).GetAwaiter().GetResult();
            Console.WriteLine($"Topic {topicName} created successfully");
        }
        catch (CreateTopicsException e)
        {
            if (e.Results[0].Error.IsError)
            {
                Console.WriteLine($"An error occurred creating topic {topicName}: {e.Results[0].Error.Reason}");
            }
            else
            {
                Console.WriteLine($"Topic {topicName} already exists");
            }
        }
    }


    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var myspecificPolicy = "_myspecificPolicy";
    var myspecificPolicy2 = "_myspecificPolicy2";

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(10);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.Name = "FundooCookie";
    });
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("http://google.com",
                                          "http://www.facebook.com");
            });
        options.AddPolicy(name: myspecificPolicy,
                          policy =>
                          {
                              policy.WithOrigins("http://google.com",
                                                  "http://www.facebook.com")
                                                    .WithMethods("PUT", "DELETE", "GET");
                          });
        options.AddPolicy(name: myspecificPolicy2,
                              policy =>
                              {
                                  policy.WithOrigins("http://google.com",
                                                  "http://www.facebook.com")
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod();
                              });
    });

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
        options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
    });

    builder.Services.AddControllers();
    builder.Services.AddDbContext<FundooContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });

    
    builder.Services.AddScoped<IUserRL, UserRL>();
    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<INoteRL, NoteRL>();
    builder.Services.AddScoped<INoteBL, NoteBL>();
    builder.Services.AddScoped<ILabelRL, LabelRL>();
    builder.Services.AddScoped<ILabelBL, LabelBL>();
    builder.Services.AddScoped<INoteLabelRL, NoteLabelRL>();
    builder.Services.AddScoped<INoteLabelBL, NoteLabelBL>();
    builder.Services.AddScoped<ICollaboratorRL, CollaboratorRL>();
    builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>();
    builder.Services.AddScoped<Token>();
    builder.Services.AddScoped<Email>();
    builder.Services.AddScoped<RabbitDemo>();
    builder.Services.AddScoped<ReddisDemo>();



    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddAuthorization();



    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseCors();
    app.UseCors(myspecificPolicy);
    app.UseCors(myspecificPolicy2);

    app.UseAuthentication();

    app.UseSession();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}