using Libary.Application.Interfaces;
using Library.Application.Interfaces.Repos;
using Library.Domain.Entities;
using Library.Infastructure.Context;

namespace Libary.Infastructure.Repos
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(LibraryDbContext context) : base(context) { }
    }
}
