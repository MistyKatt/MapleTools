using MapleTools.Abstraction;
using MapleTools.Services.Aggregator;
using MapleTools.Services.Cache;
using MapleTools.Services.Initialization;
using MapleTools.Services.Localization;
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
            services.AddSingleton<ILocalizationManager, LocalizationManager>();
            return services;
        }

        public static IServiceCollection AddAggregatorServices(this IServiceCollection services)
        {
            services.AddSingleton<BanListAggregator>();
            services.AddSingleton<FarmingAggregator>();
            services.AddSingleton<TrendingAggregator>();

            services.AddSingleton<Func<AggregatorType, IDataAggregator>>(serviceProvider => type =>
            {
                return type switch
                {
                    AggregatorType.BanList => serviceProvider.GetService<BanListAggregator>(),
                    AggregatorType.Farming => serviceProvider.GetService<FarmingAggregator>(),
                    AggregatorType.Trending => serviceProvider.GetService<TrendingAggregator>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            return services;
        }
    }
}
