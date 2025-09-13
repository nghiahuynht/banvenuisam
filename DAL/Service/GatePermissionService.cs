using DAL.IService;
using DAL.Models;
using DAL.Models.GatePermission;
using DAL.Models.Ticket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class GatePermissionService : BaseService, IGatePermissionService
    {
        private EntityDataContext dtx;
        public GatePermissionService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        public DataTableResultModel<GatePermissionGridModel> GetGatePermissionPaging(GatePermissionFilterModel filter)
        {
            var res = new DataTableResultModel<GatePermissionGridModel>();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Keyword", filter.Keyword),
                    new SqlParameter("@Start", filter.start),
                    new SqlParameter("@Length", filter.length),
                    new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                var lstData = dtx.GatePermissionModel.FromSql("sp_GetGatePermissionPaging @Keyword,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt16(param[3].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<GatePermissionGridModel>();
            }

            return res;
        }

        public bool SaveGatePermission(GatePermissionModel model)
        {
            
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@UserName", model.UserName),
                        new SqlParameter("@GateCode",  model.GateCode)
                        };


                ValidNullValue(param);
              var rs=  dtx.Database.ExecuteSqlCommand("EXEC sp_SaveGatePermission @UserName,@GateCode", param);

                if(rs==1)
                { return true; }
                else
                { return false; }
                
            }
             catch(Exception ex)
            {
                return false;
            }
        } 
      
        public bool DeleteGate(string gateCode)
        {

            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@GateCode",  gateCode)
                        };


                ValidNullValue(param);
                var rs = dtx.Database.ExecuteSqlCommand("EXEC sp_DeleteGate @GateCode", param);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateGate(GateModel model)
        {

            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@GateCode", model.GateCode),
                        new SqlParameter("@GateName",  model.GateName)
                        };


                ValidNullValue(param);
                var rs = dtx.Database.ExecuteSqlCommand("EXEC sp_CreateGate @GateCode,@GateName", param);

                if (rs == 1)
                { return true; }
                else
                { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
