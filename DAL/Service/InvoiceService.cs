using AutoMapper;
using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Invoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class InvoiceService:BaseService,IInvoiceService
    {
        private EntityDataContext dtx;
        private IMapper mapper;
        public InvoiceService(EntityDataContext dtx, IMapper mapper)
        {
            this.dtx = dtx;
            this.mapper = mapper;
        }

        public async Task<SaveResultModel> CreateInvoice(InvoiceModel model,List<InvoiceDetailGridModel> lstDetai,string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Code", model.Code),
                    new SqlParameter("@ObjId", model.ObjId),
                    new SqlParameter("@StaffCode", model.StaffCode),
                    new SqlParameter("@InvoiceDate", model.InvoiceDate),
                    new SqlParameter("@InvoiceStatus", model.InvoiceStatus),
                    new SqlParameter("@InvoiceType", model.InvoiceType),
                    new SqlParameter("@Note", model.Note),
                    new SqlParameter("@UserName", userName),
                    new SqlParameter { ParameterName = "@InvoiceId", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                await dtx.Database.ExecuteSqlCommandAsync("EXEC sp_InvoiceCreate @Code,@ObjId,@StaffCode,@InvoiceDate,@InvoiceStatus,@InvoiceType,@Note,@UserName,@InvoiceId OUT", param);
                res.ValueReturn = Convert.ToInt16(param[param.Length-1].Value);

                if (res.ValueReturn != 0)
                {
                    await SaveInvoiceDetail(res.ValueReturn, lstDetai);
                }

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
                
            }
            return res;
        }



        public async Task<SaveResultModel> UpdateInvoice(InvoiceModel model, List<InvoiceDetailGridModel> lstDetai, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@Code", model.Code),
                    new SqlParameter("@ObjId", model.ObjId),
                    new SqlParameter("@StaffCode", model.StaffCode),
                     new SqlParameter("@InvoiceStatus", model.InvoiceStatus),
                    new SqlParameter("@InvoiceDate", model.InvoiceDate),
                    new SqlParameter("@Note", model.Note),
                    new SqlParameter("@UserName", userName)
                    
                };
                ValidNullValue(param);
                await dtx.Database.ExecuteSqlCommandAsync("EXEC sp_InvoiceUpdate @Id,@Code,@ObjId,@StaffCode,@InvoiceStatus,@InvoiceDate,@Note,@UserName", param);
                res.ValueReturn = model.Id;

                if (model.Id != 0)
                {
                    await SaveInvoiceDetail(model.Id, lstDetai);
                }



            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;

            }
            return res;
        }




        public async Task<List<ObjAutoCompleteModel>> SearcObjAutocomplte(string keyword,string invoiceType)
        {
            var lst = new List<ObjAutoCompleteModel>();
            if(invoiceType.ToUpper() == Contanst.InvoiceType_SO)
            {
                lst =await dtx.Customer.Where(x => x.CustomerCode.Contains(keyword) || x.CustomerName.Contains(keyword)).Select(x => 
                new ObjAutoCompleteModel
                {
                    Id=x.Id,
                    Code = x.CustomerCode,
                    Name=x.CustomerName,
                    Phone = x.Phone,
                    Email=x.Email,
                    Address=""
                }).ToListAsync();
            }
            return lst;
        }


        public async Task<InvoiceModel> GetInvoiceById(int id)
        {
            var invoice =await dtx.Invoice.FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<InvoiceModel>(invoice);

        }

        public async Task<ObjAutoCompleteModel> GetObjSelected(int objId,string invoiceType)
        {
            var res = new ObjAutoCompleteModel();
            if (invoiceType.ToLower() == Contanst.InvoiceType_SO.ToLower())
            {
                res =await dtx.Customer.Where(x => x.Id == objId && !x.IsDeleted).Select(x => 
                new ObjAutoCompleteModel
                {
                    Id=x.Id,
                    Code=x.CustomerCode,
                    Name=x.CustomerName,
                    Phone=x.Phone,
                    Email=x.Email,
                    Address=""
                }).FirstOrDefaultAsync();

            }
            return res;


        }

        public async Task<List<InvoiceDetailGridModel>> SearchProductAutoComplete(string keyword)
        {
            var lst = await dtx.Product.Where(x => !x.IsDeleted
             && x.IsActive
             && (x.Code.Contains(keyword) || x.Name.Contains(keyword))).Select(x => 
             new InvoiceDetailGridModel {
                 Id=0,
                 ProductId=x.Id,
                 ProductCode=x.Code,
                 ProductName=x.Name,
                 Unit=x.Unit,
                 Price=x.Price.HasValue? x.Price.Value:0,
                 Quanti=1,
                 Total = x.Price.HasValue ? x.Price.Value : 0
             }).ToListAsync();
            return lst;
        }


        public async Task<InvoiceDetailGridModel> PickupProductToInvoiceDetail(int productId)
        {
            var proc =await dtx.Product.Where(x => x.Id == productId).Select(x =>
            new InvoiceDetailGridModel
            {
                Id = 0,
                InvoiceId=0,
                ProductId = x.Id,
                ProductCode = x.Code,
                ProductName = x.Name,
                Unit = x.Unit,
                Price = x.Price.HasValue?x.Price.Value:0,
                Quanti=1,
                Total = x.Price.HasValue ? x.Price.Value : 0


            }).FirstOrDefaultAsync();

            return proc;


        }


        public async Task<bool> SaveInvoiceDetail(int invoiceId, List<InvoiceDetailGridModel> lstProducts)
        {
            var lstDetailsInDatatable = await dtx.InvoiceDetail.Where(x => x.InvoiceId == invoiceId).ToListAsync();
            foreach (var item in lstProducts)
            {
                // update
                var itemExists = lstDetailsInDatatable.FirstOrDefault(x => x.InvoiceId == invoiceId && x.ProductId == item.ProductId);
                if (itemExists != null)
                {
                    itemExists.Unit = item.Unit;
                    itemExists.Price = item.Price;
                    itemExists.Quanti = item.Quanti;
                    itemExists.Total = item.Quanti * item.Price;
                    dtx.InvoiceDetail.Update(itemExists);
                }
                else // insert
                {
                    item.InvoiceId = invoiceId;
                    var entityDetail = new InvoiceDetail
                    {
                        InvoiceId = invoiceId,
                        ProductId = item.ProductId,
                        Unit = item.Unit,
                        Quanti=item.Quanti,
                        Price=item.Price,
                        Total = item.Total
                    };
                    await dtx.InvoiceDetail.AddAsync(entityDetail);
                    lstDetailsInDatatable.Add(entityDetail);
                }
            }

            // Delete remove detail
            if (lstDetailsInDatatable.Any())
            {
                foreach (var item in lstDetailsInDatatable)
                {
                    var itemRemoved = lstProducts.FirstOrDefault(x => x.InvoiceId == invoiceId && x.ProductId == item.ProductId);
                    if (itemRemoved == null)
                    {
                        dtx.InvoiceDetail.Remove(item);
                    }
                }
            }
            await dtx.SaveChangesAsync();
            return true;

        }

        public async Task<List<InvoiceDetailGridModel>> GetInvoiceItemDetail(int invoiceId)
        {
            var lstDetails = await (from ind in dtx.InvoiceDetail
                              join pro in dtx.Product on ind.ProductId equals pro.Id
                              where ind.InvoiceId == invoiceId
                              select new InvoiceDetailGridModel
                              {
                                  Id=ind.Id,
                                  InvoiceId = ind.InvoiceId,
                                  ProductId = ind.ProductId,
                                  ProductCode = pro.Code,
                                  ProductName=pro.Name,
                                  Unit=ind.Unit,
                                  Quanti=ind.Quanti,
                                  Price=ind.Price,
                                  Total = ind.Total


                              }).ToListAsync();
            return lstDetails;
        }

        public async Task<DataTableResultModel<InvoiceSearchResultModel>> SearchInvoice(InvoiceFilterModel filter)
        {
            var res = new DataTableResultModel<InvoiceSearchResultModel>();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@InvoiceType", filter.InvoiceType),
                new SqlParameter("@Status", filter.Status),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@Start", filter.start),
                new SqlParameter("@Length", filter.length),
                new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
            };
                ValidNullValue(param);
                var lstData =await dtx.InvoiceSearchResultModel.FromSql("sp_SearchInvoice @InvoiceType,@Status,@FromDate,@ToDate,@Keyword,@Start,@Length,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[7].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData;
            }
            catch (Exception ex)
            {
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<InvoiceSearchResultModel>();
            }

            return res;
        }




    }
}
