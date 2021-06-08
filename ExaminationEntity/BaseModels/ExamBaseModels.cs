using ExaminationEntity.ExaminationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminationEntity.BaseModels
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }

    public interface IDataChangeTracker : IBaseEntity
    {
        DateTime DateCreated { get; set; }
        DateTime LastDateUpdated { get; set; }
        string CreateUser { get; set; }
        string LastUpdateUser { get; set; }
    }


    public interface ICompanyTransaction : IDataChangeTracker
    {
        Company CompanyDetails { get; set; }
        int CompanyId { get; set; }
    }
}
