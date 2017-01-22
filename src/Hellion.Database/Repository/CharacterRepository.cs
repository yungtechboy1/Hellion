using Microsoft.EntityFrameworkCore;
using Hellion.Database.Structures;

namespace Hellion.Database.Repository
{
    public class CharacterRepository : RepositoryBase<DbCharacter>
    {
        public CharacterRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
