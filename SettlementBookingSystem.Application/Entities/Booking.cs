using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Entities
{
    public class Booking : AuditableEntity
    {
        public string BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
    }
}
