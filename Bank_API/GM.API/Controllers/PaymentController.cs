using GM.API.Provider;
using GM.BL.Service.Payment;
using GM.BL.Service.Users;
using GM.CORE;
using GM.MODEL.ViewModel;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Convert;


namespace GM.API.Controllers
{
    [ApiController]
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;



        private readonly TokenProviderOptions _options;
        protected readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public PaymentController(IPaymentService paymentService, TokenProviderOptions options
            )
        {
            _paymentService = paymentService;
            _options = options;
        }



        [AllowAnonymous]
        [HttpGet("GetOrderInforByCode")]
        public async Task<IActionResult> GetOrderInforByCode(string orderCode)
        {
            try
            {
                var result = await _paymentService.GetOrderInforByCode(orderCode);
                if (result != null  )
                {
                    return await CreateResponse<OrderInforViewModel>(result);
                }
                return await CreateResponseFail("Không tìm thấy thông tin thanh toán.", StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return await CreateResponseFail(ex.Message, StatusCodes.Status400BadRequest);
                throw;

            }
        }

       // [AllowAnonymous]
        [HttpPost("UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus(PaymentInforViewModel paymentInfor)
        {
            try
            {
                string userName = GetDataFromClaim(ApiClaimTypes.UserName);
                var result = await _paymentService.UpdatePaymentStatus(paymentInfor, userName);
                if (result.StatusResult) 
                {

                    if (paymentInfor.orderCode.Contains("v600023"))
                    {
                        string orderId = paymentInfor.orderCode.Replace("v600023", string.Empty);
                        SendZaloMess(Convert.ToInt64(orderId));
                    }

                    return await CreateResponse<int>(0, result.MessageResult);
                }

                return await CreateResponseFail(result.MessageResult, StatusCodes.Status302Found);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return await CreateResponseFail(ex.Message, StatusCodes.Status400BadRequest);
                throw;
            }
        }


        //[AllowAnonymous]
        //[HttpGet("SendZaloMess")]
        private async void SendZaloMess(long orderId)
        {
            try
            {
                string apiUrl = "https://ganhdadia.gamanjsc.com/condao/SendNoti?id=" + orderId;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);



                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));




                    //string tokenSet = _antiforgery.GetTokens(HttpContext).ToString();
                    //client.DefaultRequestHeaders.Add("RequestVerificationToken", tokenSet);


                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var data = await response.Content.ReadAsStringAsync();
                    //   // var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);

                    //}


                }
            }
            catch
            {

            }
            


        }


    }


}