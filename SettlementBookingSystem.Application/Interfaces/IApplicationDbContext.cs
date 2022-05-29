using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SettlementBookingSystem.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Booking> Bookings { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
