using RetailManager.Core.Interfaces;

namespace RetailManager.Core.Internal.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseConnector _db;

        public UnitOfWork(IDatabaseConnector db)
        {
            _db = db;
        }

        public void Begin()
        {
            _db.BeginTransaction();
        }

        public void Commit()
        {
            _db.CommitTransaction();
        }

        public void Rollback()
        {
            _db.RollbackTransaction();
        }

        public void Dispose()
        {
            _db.CommitTransaction();
        }
    }
}
