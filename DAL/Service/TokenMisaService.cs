using DAL.Models.TokenMisa;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Service
{
    public interface ITokenMisaService
    {
        MisaConfigModel GetConfigMisa (string key);
        TokenMisaModel GetTokenMisa(string key);
        bool UpdateTokenMisa(string key,string value);
    }
    public class TokenMisaService : BaseService, ITokenMisaService
    {
        private EntityDataContext dtx;
        public TokenMisaService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        public MisaConfigModel GetConfigMisa (string key)
        {
            var res = new MisaConfigModel();
            try
            {


                res = dtx.MisaConfigModel.FromSql("SELECT TOP 1 APIAddress as 'apiAddress',appId,UserName as 'user',Password as 'pass',TaxCode as 'taxCode',AddressToken,AddressBienLai,HSM_User as HSMUser,HSM_Pass as HSMPass FROM MisaConfig WHERE Mode={0}", key).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public TokenMisaModel GetTokenMisa(string key)
        {
            TokenMisaModel res = new TokenMisaModel();
            try
            {

                var param = new SqlParameter[] {
                        new SqlParameter("@key", key),
                       
                        };


                ValidNullValue(param);
                // dtx.Database.ExecuteSqlCommand("EXEC sp_SaveGatePermission @UserName,@GateCode", param);
                res = dtx.TokenMisaModel.FromSql("EXEC GetTokenMisa @key", param).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public bool UpdateTokenMisa(string key,string value)
        {
            
            try
            {

                var param = new SqlParameter[] {
                        new SqlParameter("@key", key),
                        new SqlParameter("@value", value),

                        };


                ValidNullValue(param);
                // dtx.Database.ExecuteSqlCommand("EXEC sp_SaveGatePermission @UserName,@GateCode", param);
                dtx.TokenMisaModel.FromSql("EXEC UpdateTokenMisa @key,@value", param).FirstOrDefault();
                return true;

            }
            catch (Exception ex)
            {
                return false;

            }
           
        }
    }
}
