using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface ISupplierService
    {
        Task<Supplier> GetSupplierById(int id);
    }
}
