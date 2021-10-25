using Board.API.Models;
using Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Infrastructure.Responsitories
{
    public class SQLBoardRepository : IBoardRepository
    {
        public Task<bool> DeleteBoardAsync(long id)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<DisscussionBoardItem>> GetBoardItemAsync(long boardId)
        {
            IEnumerable<DisscussionBoardItem> boardItems = new List<DisscussionBoardItem>()
            {
                new DisscussionBoardItem()
                {
                    Type= BoardItemType.WentWell,
                    Detail = "Finish all stories"
                },
                new DisscussionBoardItem()
                {
                    Type = BoardItemType.NeedsImproved,
                    Detail = "Too many bugs"
                }
            };

            return Task.FromResult(boardItems);
        }
    }
}
