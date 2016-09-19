using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SampleApp.Comm.Contracts;

namespace SampleApp.Comm.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope transaction;

        public void StartTransaction()
        {
            this.transaction = new TransactionScope();
        }

        public void CommitTransaction()
        {
            this.transaction.Complete();
        }

        public void Dispose()
        {
            if (this.transaction != null)
                this.transaction.Dispose();
        }
    }
}
