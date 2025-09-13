using Dapper;
using GM.CORE;
using GM.CORE.Helpers;
using GM.MODEL.Model;
using GM.MODEL.ViewModel;
using GM.MODEL.ViewModel.Common;
using GM.MODEL.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GM.DAL.Respository
{
    public interface IUserRepository
    {
        Task<UserLogin> UserLogin(string userName, string password);

    }

    internal class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction)
        {
        }

        public async Task<UserLogin> UserLogin(string userName, string password)
        {
            DynamicParameters pars = new DynamicParameters();
            pars.Add("@UserName", userName);
            //pars.Add("@Password", password.EncrypMD5());
            pars.Add("@Pass", password);
            return await QuerySingleOrDefaultAsync<UserLogin>(StoreProcedure.LOGIN, pars);
        }

    }
}