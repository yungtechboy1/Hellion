using Hellion.Database.Structures;
using Microsoft.EntityFrameworkCore;

namespace Hellion.Database.Repository
{
    public class ItemRepository : RepositoryBase<DbItem>
    {
        public ItemRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
