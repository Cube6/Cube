using Cube.User.API.Models;
using Cube.User.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            //Show come from Repository
            return new List<User>()
            {
                new User()
                {
                    Name ="Testing User 001",
                },
                new User()
                {
                    Name="Testing User 002"
                }
            };
        }

        [HttpGet("{id}")]
        public async Task<User> FindUserByIdAsync(long id)
        {
            return await _repository.GetUserByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task DeleteUserByIdAsync(long id)
        {
            await _repository.DeleteUserByIdAsync(id);
        }
    }
}
