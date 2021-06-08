using CivicExamProcedures.Methods;
using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using CivicExamProcedures.ExternalContext;

namespace CivicExamProcedures.Context
{
    public class Ta3DbConnect<TEntity> : ITa3BaseMethod<TEntity> where TEntity : class
    {
        public TA3DBContext Context { get;set; }
        public DbSet<TEntity> Dbset { get;set; }


        public Ta3DbConnect()
        {
            this.Context = new TA3DBContext();
            this.Dbset = Context.Set<TEntity>();
        }


        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> t = null)
        {
            var query = Dbset.ToList();
            if (null != t)
            {
                query = Dbset.Where(t).ToList();
            }
            return query;
        }


        public bool CheckIfExist(Expression<Func<TEntity, bool>> t)
        {

            return Dbset.Any(t);
        }

        public void Dispose()
        {
            Context.Dispose();
        }


        public TEntity GetDetails(Expression<Func<TEntity, bool>> t)
        {

            return Dbset.Where(t).FirstOrDefault();
        }
    }



    public class DbConnect<TEntity> : IBaseMethod<TEntity> where TEntity : class, IBaseEntity
    {

        public ApplicationContext Context { get; set; }
        public DbSet<TEntity> Dbset { get; set; }

        public DbConnect()
        {
            this.Context = new ApplicationContext();
            this.Dbset = Context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> t = null)
        {
            var query = Dbset.ToList();
            if (null != t)
            {
                query = Dbset.Where(t).ToList();
            }
            return query;
        }

        public TEntity AddtoContext(TEntity t)
        {
            Dbset.Add(t);
            Context.SaveChanges();
            return t;
        }

        public void DeletetoContext(TEntity t)
        {
            TEntity newT = t;
            newT.IsDeleted = true;
            UpdatetoContext(p => p.Id, newT);
        }

        public void UpdatetoContext(Expression<Func<TEntity, object>> T, TEntity newT)
        {
            Dbset.AddOrUpdate(T, newT);
            Context.SaveChanges();
        }

        public bool CheckIfExist(Expression<Func<TEntity, bool>> t)
        {

            return Dbset.Any(t);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public TEntity GetDetails(int Id)
        {
            return Dbset.Where(x => x.Id == Id).First();
        }


        public TEntity GetDetails(Expression<Func<TEntity, bool>> t)
        {
        
            return Dbset.Where(t).FirstOrDefault();
        }
    }

    
}
