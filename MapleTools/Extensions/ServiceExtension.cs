using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Services.FileDataServices;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.Cache;
using MapleTools.Services.Initialization;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace MapleTools.Extensions
{
    public enum AggregatorType
    {
        BanList,
        Farming,
        Trending
    }
    public static class ServiceExtension
    {
        public static IServiceCollection AddInitializationServices(this IServiceCollection services)
        {
            services.AddSingleton<FakeDataService>();
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<LocalizationService>();
            services.AddSingleton<BossDataService>();
            services.AddSingleton<ToolDataService>();
            services.AddSingleton<BlogDataService>();
            return services;
        }

        public static IServiceCollection AddAggregatorServices(this IServiceCollection services)
        {
            services.AddSingleton<BanListService>();
            services.AddSingleton<FarmingService>();
            services.AddSingleton<TrendingService>();

            services.AddSingleton<Func<AggregatorType, IDataService>>(serviceProvider => type =>
            {
                return type switch
                {
                    AggregatorType.BanList => serviceProvider.GetService<BanListService>(),
                    AggregatorType.Farming => serviceProvider.GetService<FarmingService>(),
                    AggregatorType.Trending => serviceProvider.GetService<TrendingService>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            return services;
        }
    }
}
