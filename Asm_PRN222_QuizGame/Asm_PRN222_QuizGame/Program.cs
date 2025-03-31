<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QuizGame.Repository.Contact;
using QuizGame.Repository;
using QuizGame.Repository.Models;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;
using AutoMapper;
using QuizGame.Service.Mapper;

namespace Asm_PRN222_QuizGame.Admin
=======
using Asm_PRN222_QuizGame.Admin.GameHub;
using Microsoft.EntityFrameworkCore;
using QuizGame.Repository;
using QuizGame.Repository.Contact;
using QuizGame.Repository.Models;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;

namespace Asm_PRN222_QuizGame
>>>>>>> origin/NguyenHP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
<<<<<<< HEAD
            builder.Services.AddSignalR();

            builder.Services.AddDbContext<QuizGame2Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IUserService, UserService>();
           

            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });
=======
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
>>>>>>> origin/NguyenHP

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapBlazorHub();
            app.MapHub<GameHub>("/gameHub");

            app.Run();
        }
    }
}
