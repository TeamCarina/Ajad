using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.Comm.Contracts
{
    public interface ISqlRepository<TEntity> where TEntity : class
    {
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity GetById(int id);        
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        void Insert(TEntity entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IQueryable<TEntity> Table { get; }
        T GetCurrentDbContext<T>();
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken);
        DbQuery<TEntity> Include(string path);
        void InsertList(List<TEntity> entityList);
    }
    public interface ISampleAppRepo<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> TableObject { get; }
    }
}
