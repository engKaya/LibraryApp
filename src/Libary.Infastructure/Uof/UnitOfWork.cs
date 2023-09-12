using Libary.Infastructure.Extensions;
using Library.Infastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Libary.Infastructure.Uof
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _libDbContext;
        private readonly ILogger<UnitOfWork> logger;
        private readonly IMediator mediator;
        private IDbContextTransaction currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => currentTransaction;
        public bool HasActiveTransaction => currentTransaction != null;
        public UnitOfWork(LibraryDbContext context, ILogger<UnitOfWork> _logger, IMediator mediator)
        {
            this._libDbContext = context;
            this.mediator = mediator;
            logger = _logger;
            currentTransaction = _libDbContext.Database.BeginTransaction();
        }
        public async void Dispose()
        {
            logger.LogInformation($"Disposing UnitOfWork.{nameof(_libDbContext)}");
            await _libDbContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            try
            {
                logger.LogInformation($"Saving Changes. Transaction Id: {this.currentTransaction.TransactionId}");
                await mediator.DispatchDomainEventsAsync(_libDbContext);
                int hasChanges = await _libDbContext.SaveChangesAsync();
                await currentTransaction.CommitAsync(cancellation);
                logger.LogInformation($"Changes Commited. Transaction Id: {this.currentTransaction.TransactionId}, {hasChanges} changes committed!");
                return hasChanges;
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Error occured at commiting changes. Transaction Id: {this.currentTransaction.TransactionId}.\n Ex: {ex.ToString()}\n Stack Trace: {ex.StackTrace} ");
                this.RollbackTransaction();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                currentTransaction?.Rollback();
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = default;
                }
            }
        }



        //private IUserRepository userRepository;
        //public IUserRepository UserRepository
        //{
        //    get
        //    {
        //        userRepository = new UserRepository(_userDbContext);
        //        return userRepository;
        //    }
        //}
    }
}
