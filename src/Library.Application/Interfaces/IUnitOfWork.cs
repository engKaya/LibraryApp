using Library.Application.Interfaces.Repos;

namespace Libary.Infastructure.Uof
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    }
}
