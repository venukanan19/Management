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


            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

   
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


            app.UseCors("AllowAll");


            app.UseStaticFiles();

            app.MapControllers();


            app.MapGet("/", () => Results.Redirect("/dashboard.html"));


            app.Run();
        }
    }
}
