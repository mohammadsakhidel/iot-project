using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Repositories.Base {
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class {

        TEntity Get(params object[] id);
        Task<TEntity> GetAsync(params object[] id);
        Task<TEntity> GetAsync(object id, bool refresh);
        IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> TakeAsync<TOrderbyProp>(int skip, int take, Func<TEntity, TOrderbyProp> orderbyPropSelector, bool desc);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Reload(TEntity entity);
        Task SaveAsync();

    }
}
