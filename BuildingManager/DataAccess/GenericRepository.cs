﻿using IDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public virtual IEnumerable<U> GetAll<U>() where U : class
        {
            return Context.Set<U>().ToList();
        }

        public virtual IEnumerable<U> GetAll<U>(Func<U, bool> searchCondition, List<string> includes = null) where U : class
        {
            IQueryable<U> query = Context.Set<U>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(searchCondition).Select(x => x).ToList();
        }


        public virtual void Insert(T entity)
        {
            Context.Set<T>().Add(entity);
            this.Save();
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            this.Save();
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            this.Save();
        }

        public bool CheckConnection()
        {
            return Context.Database.EnsureCreated();
        }

        public virtual T Get(Expression<Func<T, bool>> searchCondition, List<string> includes = null)
        {
            IQueryable<T> query = Context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(searchCondition).Select(x => x)
                .FirstOrDefault();
        }

        private void Save()
        {
            Context.SaveChanges();
        }
    }
}

