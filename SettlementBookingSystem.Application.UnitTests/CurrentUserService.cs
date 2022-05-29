using SettlementBookingSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => "TestUser";
    }
}
