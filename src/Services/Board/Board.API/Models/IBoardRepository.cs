using Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Models
{
    public interface IBoardRepository
    {
        Task<IEnumerable<DisscussionBoardItem>> GetBoardItemAsync(long boardId);
        Task<bool> DeleteBoardAsync(long id);
    }
}
