using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MapleTools.Models.Ranking;
using MapleApiSimu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace MapleApiSimu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {

            });
            builder.Services.AddSwaggerGen();

            // Register custom services
            builder.Services.AddSingleton<IRankingService, RankingService>();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();
            app.UseAuthentication(); // Add this line
            app.UseAuthorization();

            // Simplified controller mapping
            app.MapControllers();

            app.Run();
        }
    }
}
