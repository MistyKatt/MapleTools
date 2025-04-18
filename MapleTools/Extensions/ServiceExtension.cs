﻿using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Services.FileDataServices;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.Cache;
using MapleTools.Services.Initialization;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using MapleTools.Factory;
using MapleTools.Util;
using MapleTools.Models;
using MapleTools.Services.BackgroundServices;

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
            return services;
        }

        public static IServiceCollection AddAggregatorServices(this IServiceCollection services)
        {
            services.AddSingleton<DataServiceFactory>();
            services.AddSingleton<IFileAccessor, FileDataProvider>();
            return services;
        }

        public static IServiceCollection AddPlayerDataBackgroundService(this IServiceCollection services)
        {
            // Register the HTTP client factory
            services.AddHttpClient("PlayerDataApi", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                // You can configure additional settings for the HTTP client here
            });

            // Register the background service
            services.AddHostedService<PlayerBackgroundService>();

            return services;
        }
    }
}
