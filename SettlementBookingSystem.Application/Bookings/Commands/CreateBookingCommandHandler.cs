using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SettlementBookingSystem.Application.Exceptions;
using FluentValidation;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IOptions<BookingSettings> _bookingSettings;

        public CreateBookingCommandHandler(IApplicationDbContext applicationDbContext, IOptions<BookingSettings> bookingSettings)
        {
            _applicationDbContext = applicationDbContext;
            _bookingSettings = bookingSettings;
        }

        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // TODO Implement CreateBookingCommandHandler.Handle() and confirm tests are passing. See InfoTrack Global Team - Tech Test.pdf for business rules.
            //throw new NotImplementedException();
            var dt = System.Convert.ToDateTime($"{request.BookingTime}:00");
            if (dt > System.Convert.ToDateTime("16:00:00") || dt < System.Convert.ToDateTime("09:00:00"))
            {
                throw new ValidationException("Booking time must be in 9:00-16:00");
            }
            var numberOfSettlement = _bookingSettings.Value.NumberOfSettlement;
            var spotTime = _bookingSettings.Value.SpotTime;

            var newBooking = new BookingDto();
            var id = newBooking.BookingId.ToString();
            var bookingTime = Convert.ToDateTime($"{request.BookingTime}:00");
            var name = request.Name;

            //Looking for all of on going Settlements
            var onGoingSettlements = await _applicationDbContext.Bookings
                                                                        .Where(b => bookingTime >= b.BookingTime && bookingTime <= b.EndTime
                                                                                   ||
                                                                                    bookingTime.AddHours(1) >= b.BookingTime && bookingTime.AddHours(1) <= b.EndTime
                                                                                ).ToListAsync();
            if (onGoingSettlements?.Count >= 4)
            {
                throw new ConflictException("Full of 4 slots");
            }

            _applicationDbContext.Bookings.Add(new Entities.Booking()
            {
                BookingId = id,
                BookingTime = bookingTime,
                EndTime = bookingTime.AddHours(spotTime),
                Name = name
            });
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return newBooking;
        }
    }
}
