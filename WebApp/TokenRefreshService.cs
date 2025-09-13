using DAL.Models.TokenMisa;
using DAL.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp
{
    public class TokenRefreshService : BackgroundService
    {
        private Timer _timer;
        private readonly IConfiguration _configuration;
        public TokenRefreshService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Thực hiện công việc gọi API lấy token mỗi ngày một lần
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                // Lấy connection string từ cấu hình
                var connectionString =  _configuration["ConnectionStrings:DefaultConnection"];
                MisaConfigModel configModel = new MisaConfigModel();
                // Tạo đối tượng SqlConnection get info 
                using (var connection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo câu truy vấn SQL
                    var sql = "SELECT TOP 1 APIAddress as 'apiAddress', appId, UserName as 'user', Password as 'pass', TaxCode as 'taxCode', AddressToken, AddressBienLai, HSM_User, HSM_Pass FROM MisaConfig WHERE Mode=@Mode";

                    // Tạo đối tượng SqlCommand
                    using (var command = new SqlCommand(sql, connection))
                    {
                        // Thêm tham số cho truy vấn
                        command.Parameters.AddWithValue("@Mode", "TEST");

                        // Thực hiện truy vấn và đọc kết quả
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Đọc dữ liệu từ cơ sở dữ liệu
                                configModel.apiAddress = reader["apiAddress"].ToString();
                                configModel.appId = reader["appId"].ToString();
                                configModel.user = reader["user"].ToString();
                                configModel.pass = reader["pass"].ToString();
                                configModel.taxCode = reader["taxCode"].ToString();
                                configModel.addressToken = reader["AddressToken"].ToString();
                                configModel.addressBienLai = reader["AddressBienLai"].ToString();
                                configModel.HSMUser = reader["HSM_User"].ToString();
                                configModel.HSMPass = reader["HSM_Pass"].ToString();
                            }
                        }
                    }
                }

                // con api
                // Gọi API
                var tokenReturn = new APIResponseResultModel();
                try
                {
                    string fullPathAPI = string.Format(@"{0}/api2/user/token?appid={1}&taxcode={2}&username={3}&password={4}", configModel.apiAddress, configModel.appId, configModel.taxCode, configModel.user, configModel.pass);

                    var client = new RestClient(fullPathAPI);
                    var request = new RestRequest();
                    var  response = client.Execute(request);
                    var content = response.Content;
                    tokenReturn = JsonConvert.DeserializeObject<APIResponseResultModel>(content);
                    if (tokenReturn.Success)
                    {
                        // Lưu token vào cơ sở dữ liệu
                        //userService.SaveMisaToken("TokenBienLai", tokenReturn.Data);
                        UpdateTokenValue("TokenBienLai", tokenReturn.Data);
                    }
                }
                catch (Exception ex)
                {
                    tokenReturn.Success = false;
                }

            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
            }
        }

        private void UpdateTokenValue(string key, string value)
        {
            try
            {
                // Lấy connection string từ cấu hình
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

                // Tạo đối tượng SqlConnection
                using (var connection = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    connection.Open();

                    // Tạo câu lệnh SQL cập nhật dữ liệu
                    var sql = string.Format("UPDATE MisaToken SET TokenValue='{0}', CreateDate=getdate() WHERE TokenKey='{1}'", value, key);

                    // Tạo đối tượng SqlCommand
                    using (var command = new SqlCommand(sql, connection))
                    {
                        // Thực hiện câu lệnh SQL
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Cập nhật thành công
                            Console.WriteLine("Cập nhật thành công.");
                        }
                        else
                        {
                            // Không có bản ghi nào được cập nhật
                            Console.WriteLine("Không tìm thấy bản ghi để cập nhật.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Dừng Timer khi dịch vụ được ngừng
            _timer?.Change(Timeout.Infinite, 0);
            await base.StopAsync(cancellationToken);
        }
    }
}
