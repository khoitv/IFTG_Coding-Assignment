using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingSettings
    {
        public const string Key = "BookingSettings";
        public int NumberOfSettlement { get; set; }
        public double SpotTime { get; set; }
    }
}
