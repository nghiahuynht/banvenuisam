using DAL.Entities;
using DAL.Models;
using DAL.Models.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IService
{
    public interface IUserInfoService
    {
        UserInfo Login(string userName,string pass);
        string GetRoleByUser(string userName);
        SaveResultModel CreateNewUser(UserInfo entity, string userName);
        SaveResultModel UpdateUser(UserInfo entity, string userName);
        List<ComboBoxModel> GetAllRoles();
        UserInfo GetUserById(int id);
        DataTableResultModel<UserInfoGridModel> SearchUserInfo(UserInfoFilterModel filter);
        Task<List<MenuRole>> GetMenuByRole(string roleCode);
        Task<List<Menu>> LstMenu();
        Task<List<Menu>> LstMenuNavigationByRole(string roleCode);
        Task<UserInfo> GetUserByUserName(string userName);
        Task<List<UserInfo>> SearchUserAutocomplete(string keyword);
        Task SavePermissionMenu(string roleCode, int MenuId);
        Task<List<UserInfo>> ListAllUserInfo();
        SaveResultModel DeleteUser(int userId,string userNaem);
        ResComonGridModel ChangePass(ChangePassModel model);
    }
}
