using MapleTools.Extensions;
using MapleTools.Localization;
using MapleTools.Services;
using MapleTools.Simulation;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MapleTools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Add localization support
            
            

            // 3. Configure Request Localization Options
            var supportedCultures = new[] { "en", "zh-CN" };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(supportedCultures[0]);
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
                options.ApplyCurrentCultureToResponseHeaders = true;
            });
            builder.Services.Configure<LocalizationOptions>(builder.Configuration.GetSection(LocalizationOptions.Name));
            builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection(ServiceOptions.Name));
            builder.Services
                .AddInitializationServices()
                .AddAggregatorServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var path1 = app.Environment.ContentRootPath + @"\Simulation\fake_players.json";
            var path2 = app.Environment.ContentRootPath + @"\Simulation\banned_players.json";
            var path3 = app.Environment.ContentRootPath + @"\Simulation\farming_players.json";
            DummyData.GeneratePlayers(path1, path2, path3);

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
        
            app.Run();
        }
    }
}
