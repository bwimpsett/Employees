using DataAccess.Interfaces;
using System;

namespace DataAccess.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private IEmployeeRepository _employeeRepo;
        private EmployeeEntities _entities;

        public UnitOfWork(EmployeeEntities entities)
        {
            _entities = entities;
        }
        public IEmployeeRepository EmployeeRepo
        {
            get
            {
                if(_employeeRepo == null)
                {
                    _employeeRepo = new EmployeeRepository(_entities);
                }
                return _employeeRepo;
            }
        }
        public void Save()
        {
            _entities.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
