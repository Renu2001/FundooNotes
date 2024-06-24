using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Utility;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var myspecificPolicy = "_myspecificPolicy";
var myspecificPolicy2 = "_myspecificPolicy2";




// Add services to the container.
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
