using DAL.Entities;
using DAL.IService;
using DAL.Models;
using DAL.Models.Customer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class CustomerService:BaseService, ICustomerService
    {
        private EntityDataContext dtx;
        public CustomerService(EntityDataContext dtx)
        {
            this.dtx = dtx;
        }
        public async Task<SaveResultModel> CreateCustomer(Customer cus, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                cus.CreatedBy = userName;
                cus.CreatedDate = DateTime.Now;
                await dtx.Customer.AddAsync(cus);
                await dtx.SaveChangesAsync();
                res.ValueReturn = cus.Id;

            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
           
            return res;
        }

        public async Task<SaveResultModel> UpdateCustomer(Customer cus, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                cus.UpdatedBy = userName;
                cus.UpdatedDate = DateTime.Now;
                dtx.Customer.Update(cus);
                await dtx.SaveChangesAsync();
                res.IsSuccess = true;
                res.ValueReturn = cus.Id;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;
        }


        public async Task<Customer> GetCustomerById(int id)
        {
            var cus =await dtx.Customer.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return cus;
        }
        /// <summary>
        /// Lấy thông tin KH theo SĐT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByPhone(string phone)
        {
            var cus = await dtx.Customer.FirstOrDefaultAsync(x => x.Phone == phone && !x.IsDeleted);
            return cus;
        }

        public async Task<Customer> GetCustomerByCode(string code)
        {
            var cus = await dtx.Customer.FirstOrDefaultAsync(x => x.CustomerCode == code && !x.IsDeleted);
            return cus;
        }
        /// <summary>
        /// lấy danh sách KH
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetAllCustomer()
        {
            try
            {
                var cus = await dtx.Customer.Where(x => !x.IsDeleted).OrderBy(x => x.Priority).ToListAsync();
                return cus;
            }
            catch(Exception ex)
            {
                return new List<Customer>();
            }
            
        }

        public async Task<List<Customer>> ListCustomerByType(string customerType)
        {
            try
            {
                var param = new SqlParameter[] {
                    new SqlParameter("@CustomerType", customerType),
                };
                ValidNullValue(param);
                var lstData =await dtx.Customer.FromSql("EXEC sp_ListCustomerByType @CustomerType", param).ToListAsync();
                return lstData;
            }
            catch (Exception ex)
            {
                return new List<Customer>();
            }

        }



        public async Task<List<CustomerType>> LstAllCustomerType()
        {
            var res = await dtx.CustomerType.OrderBy(x => x.Priority).ToListAsync();
            return res;
        }


        public async Task<List<Area>> LstAllArea()
        {
            var res = await dtx.Area.ToListAsync();
            return res;
        }

        public async Task<List<Province>> LstAllProvince()
        {
            var res = await dtx.Province.ToListAsync();
            return res;
        }
        public DataTableResultModel<CustomerGridModel> SearchCustomerPaging(CustomerFilterModel filter)
        {
            var res = new DataTableResultModel<CustomerGridModel>();
            try
            {
                var param = new SqlParameter[] {
                    //new SqlParameter("@CustomerType", filter.CustomerTypeId),
                    //new SqlParameter("@ProvinceCode", filter.ProvinceCode),
                    new SqlParameter("@Keyword", filter.Keyword),
                    new SqlParameter("@Start", filter.start),
                    new SqlParameter("@Length", filter.length),
                    new SqlParameter { ParameterName = "@TotalRow", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.Output }
                };
                ValidNullValue(param);
                var lstData = dtx.CustomerGridModel.FromSql("EXEC sp_SearchCustomerForWeb @Keyword,@Start,@Length,@TotalRow OUT", param).ToList();
                res.recordsTotal = Convert.ToInt32(param[param.Length-1].Value);
                res.recordsFiltered = res.recordsTotal;
                res.data = lstData.ToList();
            }
            catch (Exception ex)
            {
                res.error = ex.Message;
                res.recordsTotal = 0;
                res.recordsFiltered = 0;
                res.data = new List<CustomerGridModel>();
            }

            return res;
        }


        public async Task<SaveResultModel> DeleteCustomer(int customerId, string userName)
        {
            var res = new SaveResultModel();
            try
            {
                var customer = await dtx.Customer.FirstOrDefaultAsync(x => x.Id == customerId);
                customer.IsDeleted = true;
                customer.UpdatedBy = userName;
                customer.UpdatedDate = DateTime.Now;
                dtx.Customer.Update(customer);
                await dtx.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
            }
            return res;




        }
        /// <summary>
        /// tạo mã code KH
        /// </summary>
        /// <returns></returns>
        public (bool, string) GenerateCustomerCode()
        {
            var values = string.Empty;
            int keyMax = CusCodeDataSet.DataSet.Count;   //chiều dài chuỗi ký tự mẫu
            string valueMin = CusCodeDataSet.DataSet.FirstOrDefault(x => x.Key == 0).Value;
            int len = CusCodeDataSet.len;
            long num = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Trường hợp quá "len" ký tự quy định
            if (num >= (long)Math.Pow(keyMax, len))
            {
                return (false, string.Format("Lỗi chiều dài cơ sở vượt quá giới hạn. Đã vượt mức tối đa {0} ký tự phần mã hóa!", len));
            }
            long quotient = num;    //phần nguyên
            long mod = 0;           //phần dư

            while (quotient > 0)
            {
                //tính phần dư
                mod = quotient % keyMax;
                //tính phần nguyên
                quotient = quotient / keyMax;
                //ghép chuỗi từ phải qua trái, ký tự mới nằm bên trái
                values = string.Concat(CusCodeDataSet.DataSet.FirstOrDefault(x => x.Key == mod).Value, values);
            }

            //chưa đủ "len" ký tự ghép thêm ký tự giá trị nhỏ nhất vào bên trái cho đủ
            for (int i = values.Length; i < len; i++)
                values = string.Concat(valueMin, values);

            //Có thể đảo ngược chuỗi hoặc đảo ngược quy tắc cặp 3... để tăng độ khó

            return (true, string.Concat("ONL", values.ToUpper()));
        }

        public async Task<Customer> GetCustomerByCodeAndTaxCode(string code, string taxCode)
        {
            var cus = await dtx.Customer.FirstOrDefaultAsync(x => (x.CustomerCode==code || x.TaxCode== taxCode) && !x.IsDeleted);
            return cus;
        }
    }
}
