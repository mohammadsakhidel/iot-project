using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Repositories.Base {
    public abstract class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : Entity {

        // Constructor:
        protected Repository(DbContext context) {
            Context = context;
        }

        public DbContext Context { get; set; }

        public void Add(TEntity entity) {
            Context.Add<TEntity>(entity);
        }

        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> expression) {
            return Context.Set<TEntity>().Where(expression);
        }

        public TEntity Get(params object[] id) {
            return Context.Find<TEntity>(id);
        }

        public void Remove(TEntity entity) {
            Context.Remove<TEntity>(entity);
        }

        public void Remove(SoftEntity entity) {
            entity.IsDeleted = true;
        }

        public async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }
    }
}
