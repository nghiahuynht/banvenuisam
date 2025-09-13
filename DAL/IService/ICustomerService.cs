using DAL.Entities;
using DAL.Models;
using DAL.Models.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface ICustomerService
    {
        Task<SaveResultModel> CreateCustomer(Customer cus, string userName);
        Task<SaveResultModel> UpdateCustomer(Customer cus, string userName);
        Task<Customer> GetCustomerById(int id);
        /// <summary>
        /// lấy thông tin KH theo mã KH
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Customer> GetCustomerByCode(string code);
        Task<List<CustomerType>> LstAllCustomerType();
        Task<List<Area>> LstAllArea();
        Task<List<Province>> LstAllProvince();
        DataTableResultModel<CustomerGridModel> SearchCustomerPaging(CustomerFilterModel filter);
        Task<SaveResultModel> DeleteCustomer(int customerId, string userName);
        /// <summary>
        /// Lấy danh sách KH
        /// </summary>
        /// <returns></returns>
        Task<List<Customer>> GetAllCustomer();
        Task<List<Customer>> ListCustomerByType(string customerType);
        /// <summary>
        /// tạo mã code KH
        /// </summary>
        /// <returns></returns>
        (bool, string) GenerateCustomerCode();

        Task<Customer> GetCustomerByCodeAndTaxCode(string code, string taxCode);
    }
}
