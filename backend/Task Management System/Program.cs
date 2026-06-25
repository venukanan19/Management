using Task_Management_System.Repositories.Interfaces;
using Task_Management_System.Repositories.Implementations;
using Task_Management_System.Services.Interfaces;
using Task_Management_System.Services.Implementations;

namespace Task_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger/OpenAPI support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Dependency Injection setup
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            // Enable CORS (important if frontend runs on different port)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
                    c.RoutePrefix = "swagger"; // Swagger UI at /swagger
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Enable CORS
            app.UseCors("AllowAll");

            // Serve static files (HTML, CSS, JS from wwwroot)
            app.UseStaticFiles();

            // Map API controllers
            app.MapControllers();

            // Optional: default route
            app.MapGet("/", () => "Task Management System API is running...");

            app.Run();
        }
    }
}
