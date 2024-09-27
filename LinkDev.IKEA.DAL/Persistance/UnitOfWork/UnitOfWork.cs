using LinkDev.IKEA.DAL.Persistance.Data;
using LinkDev.IKEA.DAL.Persistance.Repositories.Departments;
using LinkDev.IKEA.DAL.Persistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDbContext _dbcontext;
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dbcontext);
        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_dbcontext);


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;       
        }

        public int Complete()
        {
           return _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

    }
}
