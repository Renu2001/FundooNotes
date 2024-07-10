using CQRSApplication.Data;
using CQRSApplication.Handlers;
using CQRSApplication.Repositories;
using System.Reflection;

namespace CQRSApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          
            builder.Services.AddDbContext<DbContextClass>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddTransient<CreateStudentHandler>();
            builder.Services.AddTransient<DeleteStudentHandler>();
            builder.Services.AddTransient<UpdateStudentHandler>();
            builder.Services.AddTransient<GetStudentListHandler>();
            builder.Services.AddTransient<GetStudentByIdHandler>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}
