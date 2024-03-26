
using ApplicationCore.Interfaces.Repository;
using BackendLab01;
using Infrastructure.Memory;
using Infrastructure.Memory.Repository;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IGenericGenerator<int>, IntGenerator>();
            builder.Services.AddSingleton<IGenericRepository<Quiz, int>, MemoryGenericRepository<Quiz, int>>();
            builder.Services.AddSingleton<IGenericRepository<QuizItem, int>, MemoryGenericRepository<QuizItem, int>>();
            builder.Services.AddSingleton<IGenericRepository<QuizItemUserAnswer, string>, MemoryGenericRepository<QuizItemUserAnswer, string>>();
            builder.Services.AddSingleton<IQuizUserService, QuizUserService>();
            builder.Services.AddSingleton<IQuizAdminService, QuizAdminService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();
            //app.Seed();
            app.Run();
        }
    }
}