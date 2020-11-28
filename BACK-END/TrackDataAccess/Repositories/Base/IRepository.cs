using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Repositories.Base {
    public interface IRepository<TEntity>
        where TEntity : Entity {

        TEntity Get(params object[] id);
        Task<TEntity> GetAsync(params object[] id);
        IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> TakeAsync<TOrderbyProp>(int skip, int take, Func<TEntity, TOrderbyProp> orderbyPropSelector, bool desc);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        Task SaveAsync();

    }
}
