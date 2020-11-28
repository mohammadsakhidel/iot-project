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

        public async Task<TEntity> GetAsync(params object[] id) {
            return await Context.FindAsync<TEntity>(id);
        }

        public async Task<IEnumerable<TEntity>> TakeAsync<TOrderbyProp>(int skip, int take, Func<TEntity, TOrderbyProp> orderbyPropSelector, bool desc = false) {
            return await Task.Run(() => {
                return (desc ? Context.Set<TEntity>().OrderByDescending(orderbyPropSelector).Skip(skip).Take(take).ToList()
                    : Context.Set<TEntity>().OrderBy(orderbyPropSelector).Skip(skip).Take(take).ToList());
            });
        }

        public void Remove(TEntity entity) {
            if (entity is SoftEntity soft) {
                soft.IsDeleted = true;
                soft.DeleteTime = DateTime.UtcNow;
            } else {
                Context.Remove<TEntity>(entity);
            }
        }

        public async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }
    }
}
