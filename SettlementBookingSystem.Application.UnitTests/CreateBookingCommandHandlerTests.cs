using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Application.Interfaces;
using SettlementBookingSystem.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IOptions<BookingSettings> _bookingSettings;
        private readonly int NumberOfSettlement = 4;
        //Initial
        public CreateBookingCommandHandlerTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddInfrastructure();
            services.AddApplication();
            _bookingSettings = Options.Create<BookingSettings>(new BookingSettings()
            {
                NumberOfSettlement = this.NumberOfSettlement,
                SpotTime = 1
            });

            var provider = services.BuildServiceProvider();
            _applicationDbContext = provider.GetRequiredService<IApplicationDbContext>();
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };


            var handler = new CreateBookingCommandHandler(_applicationDbContext, _bookingSettings);

            var result = await handler.Handle(command, CancellationToken.None);

            result.BookingId.Should().NotBeEmpty();
        }

        [Fact]
        public void GivenOutOfHoursBookingTime_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "00:00",
            };

            var handler = new CreateBookingCommandHandler(_applicationDbContext, _bookingSettings);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenBookingIsFull_ThenConflictThrown()
        {
            //Mockup for full 4 slots in db
            for (int i = 0; i < NumberOfSettlement; i++)
            {
                var command = new CreateBookingCommand
                {
                    Name = "test",
                    BookingTime = "09:15",
                };

                var handler = new CreateBookingCommandHandler(_applicationDbContext, _bookingSettings);
                var result = await handler.Handle(command, CancellationToken.None);
            }

            var conflictCommand = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };

            var handlerConflictCommand = new CreateBookingCommandHandler(_applicationDbContext, _bookingSettings);

            Func<Task> act = async () => await handlerConflictCommand.Handle(conflictCommand, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }
    }
}
