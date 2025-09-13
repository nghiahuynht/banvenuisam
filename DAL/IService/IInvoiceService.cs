using DAL.Entities;
using DAL.Models;
using DAL.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IInvoiceService
    {
        Task<SaveResultModel> CreateInvoice(InvoiceModel model, List<InvoiceDetailGridModel> lstDetai, string userName);
        Task<List<ObjAutoCompleteModel>> SearcObjAutocomplte(string keyword, string invoiceType);
        Task<InvoiceModel> GetInvoiceById(int id);
        Task<ObjAutoCompleteModel> GetObjSelected(int objId, string invoiceType);
        Task<SaveResultModel> UpdateInvoice(InvoiceModel model, List<InvoiceDetailGridModel> lstDetai, string userName);
        Task<List<InvoiceDetailGridModel>> SearchProductAutoComplete(string keyword);
        Task<InvoiceDetailGridModel> PickupProductToInvoiceDetail(int productId);
        Task<bool> SaveInvoiceDetail(int invoiceId, List<InvoiceDetailGridModel> lstProducts);
        Task<List<InvoiceDetailGridModel>> GetInvoiceItemDetail(int invoiceId);
        Task<DataTableResultModel<InvoiceSearchResultModel>> SearchInvoice(InvoiceFilterModel filter);


    }
}
