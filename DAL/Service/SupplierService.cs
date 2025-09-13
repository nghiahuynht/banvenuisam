using DAL.Entities;
using DAL.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class SupplierService:BaseService, ISupplierService
    {
        private EntityDataContext dtx;
        public SupplierService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        public async Task<Supplier> GetSupplierById(int id)
        {
            var res = await dtx.Supplier.FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }




    }
}
