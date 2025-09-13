
using DAL.Models;
using DAL.Models.GatePermission;
using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IGatePermissionService
    {
        DataTableResultModel<GatePermissionGridModel> GetGatePermissionPaging(GatePermissionFilterModel filter);
        bool SaveGatePermission(GatePermissionModel model);
        bool DeleteGate(string gateCode);
        bool CreateGate(GateModel model);
    }
      
}
