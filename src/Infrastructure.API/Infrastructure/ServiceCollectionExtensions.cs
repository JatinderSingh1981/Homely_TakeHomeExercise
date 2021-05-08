using Business.API;
using Common.API;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Repository.API;
using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using ViewModels.API;

namespace Infrastructure.API
{
    public static class ServiceCollectionExtensions
    {
        // Adds all services here
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services
                .AddOptions()
                .Configure<AppSettings>(configuration)
                .AddResponseCaching()
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


            services
                .AddSwaggerGen()
                .AddBusinessLayer()
                .AddRepositoryLayer()
                .AddResponseLayer()
                .AddCachingLayer();
            return services;
        }

        private static IServiceCollection AddBusinessLayer(this IServiceCollection services) =>
            services
            .AddTransient<IPropertyListingBusiness, PropertyListingBusiness>();
        //.AddTransient<IPBM, PropertyBusinessMethods >


        private static IServiceCollection AddRepositoryLayer(this IServiceCollection services) =>
            services.AddTransient<IPropertyListingRepository, PropertyListingRepository>();

        private static IServiceCollection AddResponseLayer(this IServiceCollection services) =>
            services.AddTransient<PropertyListingResponse>();

        private static IServiceCollection AddCachingLayer(this IServiceCollection services) =>
            services
            .AddMemoryCache()
            .AddSingleton<ICacheManager, CacheManager>();

        //Methods commented out as not required for now
        //private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
        //                                                                    retryAttempt)));
        //}

        //private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        //}
    }
}
