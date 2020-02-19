using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApi.Models;

namespace WebApi.Repository
{
    public class GenericRepository<TEntity>:IRepository<TEntity> where TEntity:class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            logger.Info(Environment.NewLine + entity.GetType().Name + " ID: " + GetPropValue(entity, entity.GetType().GetProperties()[0].Name) + " added by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

        }
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            logger.Info(Environment.NewLine + entityToDelete.GetType().Name + " ID: " + GetPropValue(entityToDelete, entityToDelete.GetType().GetProperties()[0].Name) + " deleted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            logger.Info(Environment.NewLine + entityToUpdate.GetType().Name + " ID: " + GetPropValue(entityToUpdate, entityToUpdate.GetType().GetProperties()[0].Name) + " editted by " + CookieHandler.GetUserNameFromCookie("LoginCookie") + " " + DateTime.Now);

        }
    }
}
