using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CliveBot.Application
{
    public static class ConfigureApplicationExtensions
    {
        public static IServiceCollection RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
