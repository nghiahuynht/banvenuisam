using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.ConDao;
using DAL.Models.Ticket;
using DAL.Models.TicketOrder;
using DAL.Models.WebHookSePay;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Infrastructure.Configuration;

namespace DAL.Service
{
    public class TicketOrderService:BaseService, ITicketOrderService
    {
        private EntityDataContext dtx;

        public TicketOrderService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }

        public List<TicketOrderSubNum> GetSubOrderCodeByOrderId(long orderId)
        {
            try
            {
                var result = dtx.TicketOrderSubNum.Where(x => x.OrderId == orderId).ToList();
                return result;
            }
            catch(Exception ex)
            {
                string test = ex.Message;
                return new List<TicketOrderSubNum>();
            }
          
        }

        public PrintPdfOrderModel GetPrintPdfSubOrderDetail(long subid)
        {
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@SubOrderId",subid)
                    };


                var result = dtx.PrintPdfOrderModel.FromSql("EXEC sp_GetSubTicketOrderByIdNew @SubOrderId", param).FirstOrDefault();
                return result;
            }
            catch(Exception ex)
            {
                return new PrintPdfOrderModel();
            }
           
        }


        public List<PrintPdfOrderModel> GetListPrintPdfByOrderId(long orderId)
        {
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId",orderId)
                    };


                var result = dtx.PrintPdfOrderModel.FromSql("EXEC sp_GetListSubTicketOrderByOrderId @OrderId", param).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<PrintPdfOrderModel>();
            }

        }





        public SaveResultModel ChangePaymentStatusTicketOrder(long OrderId,int newStatus,string userName,string paymentValue="", long paymentID = 0)
        {
            var res = new SaveResultModel();
            try
            {

                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@OrderId",OrderId),
                    new SqlParameter("@NewsStatus", newStatus),
                    new SqlParameter("@PaymentValue", paymentValue),
                    new SqlParameter("@PaymentId", paymentID),

                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_ChangeStatusPaymentOrder @OrderId,@NewsStatus,@PaymentValue,@PaymentId", param);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        

        public async Task<SaveResultModel> ChangeStatusTicketOrder(long OrderId, string newStatus, string userName)
        {
            var res = new SaveResultModel();
            
            try
            {
                var rsCallVT =  await CancelInvoiceViettel(OrderId);
                if (rsCallVT.IsSuccess == false)
                {
                    res.IsSuccess = false;
                    res.ErrorMessage ="Lỗi xóa trên Viettel: "+ rsCallVT.ErrorMessage;
                    return res;
                }
                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@OrderId",OrderId),
                    new SqlParameter("@NewsStatus", newStatus),
                    new SqlParameter("@UserName", userName)

                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_ChangeStatusOrder @OrderId,@NewsStatus,@UserName", param);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        // delete type 1 : delete TickOrder, delete type 2: Delete SubOrder
        public SaveResultModel DeleteObjectOrder(long id,int deleteType, string userName)
        {
            var res = new SaveResultModel();
            try
            {

                var transaction = dtx.Database.BeginTransaction();
                var param = new SqlParameter[] {
                    new SqlParameter("@Id",id),
                    new SqlParameter("@DeleteType", deleteType),
                    new SqlParameter("@UserName", userName)

                };
                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_DeleteObjectOrder @Id,@DeleteType,@UserName", param);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<DataTableResultModel<OrderGridModel>> SearchOrder(OrderFilterModel filter,bool isExcel)
        {
            var res = new DataTableResultModel<OrderGridModel>();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@IsExcel", isExcel),
                        new SqlParameter("@ChanelId", filter.ChanelId),
                        new SqlParameter("@FromDate", filter.FromDate),
                        new SqlParameter("@ToDate", filter.ToDate),
                        new SqlParameter("@PaymentMethod", filter.PaymentMethod),
                        new SqlParameter("@PaymentStatus", filter.PaymentStatus),
                        new SqlParameter("@GateCode", filter.GateCode),
                        new SqlParameter("@Keyword", filter.Keyword),
                        new SqlParameter("@PartnerCode", filter.PartnerCode),
                        new SqlParameter("@CustomerType", filter.CustomerType),
                        new SqlParameter("@TicketGroup", filter.GroupTicket),
                        new SqlParameter("@ObjType", filter.ObjType),
                        new SqlParameter("@TicketCode", filter.TicketCode),
                        new SqlParameter("@UserSale", filter.UserName),
                        new SqlParameter("@IsFree", filter.IsFree),
                        new SqlParameter("@Start", filter.start),
                        new SqlParameter("@Length", filter.length),
                        new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int16, Direction = System.Data.ParameterDirection.Output }
                    };
                ValidNullValue(param);
                var lstData = await dtx.OrderGridModel.FromSql("sp_SearchOrder @IsExcel,@ChanelId,@FromDate,@ToDate,@PaymentMethod,@PaymentStatus,@GateCode,@Keyword,@PartnerCode,@CustomerType,@TicketGroup,@ObjType,@TicketCode,@UserSale,@IsFree,@Start,@Length,@TotalRow OUT", param).ToListAsync();
                res.recordsTotal = Convert.ToInt16(param[param.Length-1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData;
            }
            catch (Exception ex)
            {
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<OrderGridModel>();
            }

            return res;
        }


        





        public async Task<List<SubOrderPrintModel>> GetSubCodePrintInfo(long orderId)
        {
            var res = new List<SubOrderPrintModel>();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId",orderId)

                    };
                ValidNullValue(param);
                res = await dtx.SubOrderPrintModel.FromSql("EXEC sp_GetOrderPrintInfo @OrderId", param).ToListAsync();
            }
            catch (Exception ex)
            {
                res = new List<SubOrderPrintModel>();
            }

            return res;
        }


        public ReportSaleCounterModel ReportSaleCounterModel(SaleReportFilterModel filter)
        {
            var res = new ReportSaleCounterModel();
            try
            {
                var param = new SqlParameter[] {
                new SqlParameter("@SaleChanelId", filter.SaleChanelId),
                new SqlParameter("@GateCode", filter.GateCode),
                new SqlParameter("@UserName", filter.UserName),
                new SqlParameter("@TicketCode", filter.TicketCode),
                new SqlParameter("@Keyword", filter.Keyword),
                new SqlParameter("@FromDate", filter.FromDate),
                new SqlParameter("@ToDate", filter.ToDate),
                new SqlParameter("@PaymentType", filter.PaymentType),

            };
                ValidNullValue(param);
                res = dtx.ReportSaleCounterModel.FromSql("EXEC sp_CounterReportSaleWeb @SaleChanelId,@GateCode,@UserName,@TicketCode,@Keyword,@FromDate,@ToDate,@PaymentType", param).FirstOrDefault();

            }
            catch (Exception ex)
            {
               
            }

            return res;

        }

        public TicketOrderModel CheckPayment(string Description, double Amount)
        {
            var res = new TicketOrderModel();
            try
            {
                    var param = new SqlParameter[] {
                        new SqlParameter("@Description", Description),
                        new SqlParameter("@Price", Amount),
               

                    };
                ValidNullValue(param);
                res = dtx.TicketOrderModel.FromSql("EXEC sp_CheckPayment @Description,@Price", param).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        
        public async Task<SaveResultModel> SaveTranSePayWebHook(WebHookReceiveModel model)
        {
            var res = new SaveResultModel();
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@Id", model.id),
                    new SqlParameter("@GateWay", model.gateway),
                    new SqlParameter("@TranDate", model.transactionDate),
                    new SqlParameter("@AccNum", model.accountNumber),
                    new SqlParameter("@AmountIn", model.transferAmount),
                    new SqlParameter("@TranContent", model.content),
                    new SqlParameter("@ReferenceNum",model.referenceCode),
                    new SqlParameter { ParameterName = "@NewId", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                await dtx.Database.ExecuteSqlCommandAsync("EXEC sp_SaveSePayWebHook @Id,@GateWay,@TranDate,@AccNum,@AmountIn,@TranContent,@ReferenceNum,@NewId OUT", param);
                res.ValueReturn = Convert.ToInt16(param[param.Length-1].Value);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;

            }
            return res;
        }


        public ResultModel SaveOrderToData(PostOrderSaveModel model, string userName,string gateName)
        {

            var res = new ResultModel();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@Id",0),
                        new SqlParameter("@CustomerCode", model.CustomerCode),
                        new SqlParameter("@CustomerName", string.Empty),
                        new SqlParameter("@CustomerType", model.CustomerType),
                        new SqlParameter("@TicketCode",model.TicketCode),
                        new SqlParameter("@Quanti", model.Quanti),
                        new SqlParameter("@Price", model.Price),
                        new SqlParameter("@UserName", userName),
                        new SqlParameter("@BienSoXe", model.BienSoXe),
                        new SqlParameter("@IsCopy", false),
                        new SqlParameter("@GateName", gateName),
                        new SqlParameter("@Objtype", model.ObjType),
                        new SqlParameter("@IsFree", model.IsFree),
                        new SqlParameter("@PrintType", model.PrintType),
                        new SqlParameter("@DiscountPercent", model.DiscountPercent),
                        new SqlParameter("@DiscountValue",Convert.ToDecimal(model.DiscountValue)),
                        new SqlParameter("@TienKhachDua", model.TienKhachDua),
                        new SqlParameter("@PaymentType", model.PaymentType),
                        
                        new SqlParameter { ParameterName = "@OrderId", DbType = System.Data.DbType.Int64, Direction = System.Data.ParameterDirection.Output }

                    };

                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_SaveOrderTicket @Id,@CustomerCode,@CustomerName,@CustomerType,@TicketCode,@Quanti,@Price,@UserName,@BienSoXe,@IsCopy,@GateName,@Objtype,@IsFree,@PrintType,@DiscountPercent,@DiscountValue,@TienKhachDua,@PaymentType,@OrderId OUT", param);
                

                res.ValueReturn = Convert.ToInt64(param[param.Length - 1].Value);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

            }
            return res;

        }



       public void AssignSubIdForMapping(Int64 orderId)
       {
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId",orderId),

                    };


                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_AssignSubIdForEmptyCard @OrderId", param);

            }
            catch (Exception ex)
            {
               

            }
        }



        private async Task<SaveResultModel> CancelInvoiceViettel(long orderId)
        {
            var res = new SaveResultModel();
            var dataTicketOrderSub = dtx.TicketOrderSubNum.Where(x => x.OrderId == orderId).ToList();
            
            try
            {
                string username = AppSettingServices.Get.ViettelSettings.Username;  
                string taxCode = AppSettingServices.Get.ViettelSettings.Username;  
                string password = AppSettingServices.Get.ViettelSettings.Password;
                string templateCode = AppSettingServices.Get.ViettelSettings.TemplateCode;
                string url = AppSettingServices.Get.ViettelSettings.APICancelInvoice;

                using (var client = new HttpClient())
                {
                    if(dataTicketOrderSub.Count()>0)
                    {
                        foreach(var item in dataTicketOrderSub)
                        {
                            // Mã hóa username:password thành Base64
                            string authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);

                            var content = new FormUrlEncodedContent(new[]
                            {
                            new KeyValuePair<string, string>("supplierTaxCode", taxCode),
                            new KeyValuePair<string, string>("templateCode", templateCode),
                            new KeyValuePair<string, string>("invoiceNo", item.InvoiceNumber),
                            new KeyValuePair<string, string>("additionalReferenceDate",item.InvoiceIssued.ToString()),// 
                            new KeyValuePair<string, string>("additionalReferenceDesc", "GM xóa hóa đơn"),
                            new KeyValuePair<string, string>("strIssueDate", item.InvoiceIssued.ToString()),
                            new KeyValuePair<string, string>("reasonDelete","Khách hàng yêu cầu xóa"),
                        });

                            var response = await client.PostAsync(url, content);
                            var responseString = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                var apiResponse = JsonSerializer.Deserialize<CancelInvoiceResponse>(responseString, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                                if (apiResponse?.Description == "CANCEL TRANSACTION INVOICE SUCCESS")
                                {
                                    res.IsSuccess = true;
                                }
                                else
                                {
                                    res.IsSuccess = false;
                                    res.ErrorMessage = apiResponse?.Description ?? "Unknown error";
                                }
                            }
                            else
                            {
                                var errorResponse = JsonSerializer.Deserialize<CancelInvoiceErrorResponse>(responseString, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                                res.IsSuccess = false;
                                res.ErrorMessage = errorResponse != null
                                    ? $"{errorResponse.Message} - {errorResponse.Data}"
                                    : "API call failed";
                            }
                        }    
                    }
                }    
                    
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public void SavePrintAgain(long orderId, string userName,int quanti)
        {
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId",orderId),
                        new SqlParameter("@TicketCode",""),
                        new SqlParameter("@PrintType ",""),
                        new SqlParameter("@Quanti",quanti),
                        new SqlParameter("@UserName",userName),
                        new SqlParameter("@MaTraCuu",""),

                    };


                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_SavePrintAgain @OrderId,@TicketCode,@PrintType,@Quanti,@UserName,@MaTraCuu", param);

            }
            catch (Exception ex)
            {


            }
        }


        public ResultModel UpdateCustomerForTicketOrder(UpdateCustForOrderModel model)
        {

            var res = new ResultModel();
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter("@OrderId",model.OrderId),
                        new SqlParameter("@CustomerCode", model.CustomerCode),
                        new SqlParameter("@CustomerName", model.CustomerName),
                    };

                ValidNullValue(param);
                dtx.Database.ExecuteSqlCommand("EXEC sp_UpdateCustomerForOrder @OrderId,@CustomerCode,@CustomerName", param);

                res.ValueReturn = 1;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

            }
            return res;

        }



    }
}
