using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using QuizGame.Service.Interface;
using QuizGame.Service.Service;
using QuizGame.Repository.Contact;
using QuizGame.Repository;
using QuizGame.Repository.Models;
using QuizGame.Player.Data;
using AutoMapper;

namespace QuizGame.Player
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddDbContext<QuizGame2Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDistributedMemoryCache();  // Lưu trữ trong bộ nhớ
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
                options.Cookie.IsEssential = true; // Cấu hình cookie để sử dụng session
            });
            // Register the services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IPlayerAnswerService, PlayerAnswerService>();
            builder.Services.AddAutoMapper(typeof(Mapper));


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapBlazorHub();
            app.MapHub<GameHub>("/gameHub");
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
