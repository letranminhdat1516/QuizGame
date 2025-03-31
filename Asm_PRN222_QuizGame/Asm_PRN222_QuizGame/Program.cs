using Asm_PRN222_QuizGame.Admin.GameHub;
using Microsoft.EntityFrameworkCore;
using QuizGame.Repository;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;

namespace Asm_PRN222_QuizGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSignalR();

            builder.Services.AddDbContext<QuizGame2Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Dependency Injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapBlazorHub();
            app.MapHub<GameHub>("/gameHub");

            app.Run();
        }
    }
}
