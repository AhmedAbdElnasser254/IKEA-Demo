﻿using LinkDev.IKEA.DAL.Entities;
using LinkDev.IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase 
    {
        private protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public IEnumerable<T> GetAll(bool withAsNoTracking = true)
        {
            if (withAsNoTracking)
                return _dbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToList();

            return _dbContext.Set<T>().Where(X => !X.IsDeleted).ToList();
        }

        public IQueryable<T> GetAllAsIQueryable()
        {
            return _dbContext.Set<T>();
        }

        public IEnumerable<T> GetIEnumerable()
        {
            return _dbContext.Set<T>();
        }

        public T? Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
            // return _dbContext.Find<T>(id);


            ///var T = _dbContext.Ts.FirstOrDefault(D => D.Id == id);
            ///if (T == null)
            ///    T = _dbContext.Ts.FirstOrDefault(D => D.Id == id);
            ///return T;

        }


        public void Add(T entity) => _dbContext.Set<T>().Add(entity);

        public void Update(T entity) => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<T>().Remove(entity);          
        }

    }
}
