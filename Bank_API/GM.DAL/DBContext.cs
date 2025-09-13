using System;
using System.Data;
using System.Data.SqlClient;

namespace GM.DAL
{
    public interface IDBContext : IDisposable
    {
        void Commit();

        void Rollback();

        IUnitOfWork UnitOfWork { get; }
    }

    internal sealed class DBContext : IDBContext
    {
        private const string multipeResult = "MultipleActiveResultSets=True";
        private const string readOnly = "ApplicationIntent=ReadOnly";

        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        private IUnitOfWork unitOfWork;

        public IUnitOfWork UnitOfWork => unitOfWork;

        public DBContext(string connectionString, bool isTransaction, bool isReadOnly = false, bool isMultipleResult = false)
        {
            try
            {
                if (isReadOnly)
                {
                    connectionString += ";" + readOnly;
                }
                if (isMultipleResult)
                {
                    connectionString += ";" + multipeResult;
                }
                _connection = new SqlConnection(connectionString);
                _connection.Open();

                if (isTransaction)
                {
                    _transaction = _connection.BeginTransaction();
                }

                unitOfWork = new UnitOfWork(_connection, _transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}