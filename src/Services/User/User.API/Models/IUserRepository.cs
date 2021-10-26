using System.Threading.Tasks;

namespace Cube.User.API.Models
{
    public interface IUserRepository
    {
        Task<Domain.User> GetUserByIdAsync(long userId);
        Task<bool> DeleteUserByIdAsync(long userId);
    }
}
