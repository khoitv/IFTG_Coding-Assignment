using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Application.Interfaces;
using SettlementBookingSystem.Infrastructure;
using SettlementBookingSystem.ProblemDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IConfiguration Configuration { get; }
        private static IMediator mediator;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddSingleton<ICurrentUserService, CurrentUserService>();
                services.AddInfrastructure();
                services.AddApplication();

                // Create a new service provider.
                var serviceProvider = new ServiceCollection().BuildServiceProvider();
                mediator = serviceProvider.GetRequiredService<IMediator>();
            }) ;
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return await mediator.Send(request);
        }
    }
}
