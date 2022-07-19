using dating_app_api.Entities;

namespace dating_app_api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
