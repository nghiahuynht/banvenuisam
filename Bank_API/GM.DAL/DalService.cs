using MailKit;
using System;

namespace GM.DAL
{
    public interface IDalService
    {
        IDBContext Connection(bool isTransaction = false, bool isReadOnly = false, bool isMultipleResult = false);
    }

    public sealed class DalService : IDalService
    {
        private string connectionString = "";
        private DBContext dbContext = null;

        public DalService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDBContext Connection(bool isTransaction = false, bool isReadOnly = false, bool isMultipleResult = false)
        {
            dbContext = new DBContext(connectionString, isTransaction, isReadOnly, isMultipleResult);
            return dbContext;
        }
    }

}


