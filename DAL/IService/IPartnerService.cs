using DAL.Models;
using DAL.Models.partner;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IService
{
    public interface IPartnerService
    {
        DataTableResultModel<PartnerGridModel> SearchTicket(PartnerFilterModel filter);
        InfoPartnerViewModel GetInfoPartner(int Id);
        bool ApprovalPartner(int PartnerId,string ApprovalBy);
        ResultModel InsertUpdatePartner(PartnerModel model);
        PartnerModelViewModel GetPartnerById(int Id);
        bool CancelApproval(int Id,string Note, string ApprovalBy);
        bool DeletePartner(int PartnerId, string ApprovalBy);
    }
}
