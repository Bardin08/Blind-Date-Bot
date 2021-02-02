using BlindDateBot.Data.Contexts;
using BlindDateBot.Domain.Models;

namespace BlindDateBot.Data.Repositories
{
    public class UsersRepository : BaseRepository<UserModel>
    {
        public UsersRepository(BaseContext baseContext) : base(baseContext)
        {
        }
    }
}
