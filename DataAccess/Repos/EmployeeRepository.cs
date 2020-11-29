using DataAccess.Interfaces;
using System;

namespace DataAccess.Repos
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository 
    {
        private readonly EmployeeEntities _context;
        
        public EmployeeRepository(EmployeeEntities context) : base(context)
        {
            _context = context;
        }

    }

}
