using Cube.User.API.Models;
using Cube.User.Domain;
using System.Threading.Tasks;

namespace Board.API.Infrastructure.Responsitories
{
    public class SQLUserRepository : IUserRepository
    {
        public Task<User> GetUserByIdAsync(long userId)
        {
            return Task.FromResult(new User()
            {
                Id = userId,
                Name = "John",
                AvatarUrl = "./Tiger.png"
            });
        }

        public Task<bool> DeleteUserByIdAsync(long userId)
        {
            return Task.FromResult(true);
        }
    }
}
