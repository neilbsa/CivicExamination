using CivicExamProcedures.Context;
using CivicExamProcedures.ExternalContext;
using ExaminationEntity.BaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CivicExamProcedures.Methods
{
    public interface ITa3BaseMethod<TEntity> where TEntity : class
    {
        TA3DBContext Context { get; set; }
        DbSet<TEntity> Dbset { get; set; }
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> t);
        TEntity GetDetails(Expression<Func<TEntity, bool>> t);
        bool CheckIfExist(Expression<Func<TEntity, bool>> t);
    }

    public interface ITA3BaseControlMethod<TEntity> where TEntity : class, IBaseEntity
    {
        Ta3DbConnect<TEntity> BaseMethods { get; set; }
    }


    public interface IBaseMethod<TEntity> where TEntity : class, IBaseEntity
    {
        ApplicationContext Context { get; set; }
        DbSet<TEntity> Dbset { get; set; }
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> t);
        TEntity GetDetails(int Id);
        TEntity GetDetails(Expression<Func<TEntity, bool>> t);
        TEntity AddtoContext(TEntity t);
        void DeletetoContext(TEntity t);
        void UpdatetoContext(Expression<Func<TEntity, object>> T, TEntity newT);
        bool CheckIfExist(Expression<Func<TEntity, bool>> t);
    }

    public interface IBaseControlMethod<TEntity> where TEntity : class, IBaseEntity 
    {
        DbConnect<TEntity> BaseMethods { get;set; }
    }

    
}
