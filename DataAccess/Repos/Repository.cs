using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmployeeEntities _context;
        private readonly DbSet<T> _entities;

        public Repository(EmployeeEntities entities)
        {
            _entities = entities.Set<T>();
            _context = entities;
        }
        public IEnumerable<T> GetAll()
        {
            return _entities;
        }
        public T GetById(int id)
        {
            return _entities.Find(id);
        }
        public IEnumerable<T> Search(Expression<Func<T,bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public void Insert(T entity)
        {
            _entities.Add(entity);
        }
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            _entities.Remove(_entities.Find(id));
        }
    }
}
