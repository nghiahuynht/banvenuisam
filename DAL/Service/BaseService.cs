using DAL.Entities;
using DAL.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class BaseService
    {
       
       

        public void ValidNullValue(SqlParameter[] paramList)
        {
            foreach (SqlParameter p in paramList)
            {
                if (p.Value == null)
                    p.Value = DBNull.Value;
            }
        }

    }
}
