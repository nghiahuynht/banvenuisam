

using DAL.Entities;
using DAL.IService;
using System.Linq;


namespace DAL.Service
{
    public class ResultCodeService : BaseService, IResultCodeService
    {
        private EntityDataContext dtx;
        public ResultCodeService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        public ResultCode GetResultCodeByCode(int code)
        {
            var entity = dtx.ResultCode.FirstOrDefault(x => x.Code == code);
            return entity;
        }
    }
}
