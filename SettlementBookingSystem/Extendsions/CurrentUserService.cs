using SettlementBookingSystem.Application.Interfaces;

namespace SettlementBookingSystem.Extendsions
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => "TestUser";
    }

}
