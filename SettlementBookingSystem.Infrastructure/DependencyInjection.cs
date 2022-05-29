using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Application.Interfaces;

namespace SettlementBookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("SettlementBookingSystemDbContext"));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            return services;
        }
    }
}
