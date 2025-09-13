using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GM.DAL.Respository
{
    internal class RepositoryBase
    {
        protected IDbConnection connection = null;
        protected IDbTransaction transaction = null;

        public RepositoryBase(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string procedure, DynamicParameters dynamicParameters = null)
        {
            Type type = typeof(T);
            if (IsPrimitiveType(type))
            {
                return await QueryPrimitiveAsync<T>(procedure, dynamicParameters);
            }
            return await QueryDynamicAsync<T>(procedure, dynamicParameters);
        }

        protected async Task<T> QuerySingleOrDefaultAsync<T>(string procedure, DynamicParameters dynamicParameters = null)
        {
            Type type = typeof(T);
            if (IsPrimitiveType(type))
            {
                return await QuerySingleOrDefaultPrimitiveAsync<T>(procedure, dynamicParameters);
            }
            return await QuerySingleOrDefaultDynamicAsync<T>(procedure, dynamicParameters);
        }

        protected async Task<bool> ExecuteAsync(string procedure, DynamicParameters dynamicParameters = null)
        {
            await connection.ExecuteAsync(sql: procedure, param: dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            return true;
        }

        private async Task<IEnumerable<T>> QueryPrimitiveAsync<T>(string procedure, DynamicParameters dynamicParameters)
        {
            return await connection.QueryAsync<T>(sql: procedure, transaction: transaction, commandType: CommandType.StoredProcedure, param: dynamicParameters);
        }

        private async Task<IEnumerable<T>> QueryDynamicAsync<T>(string procedure, DynamicParameters dynamicParameters)
        {
            var result = await connection.QueryAsync<dynamic>(sql: procedure, transaction: transaction, commandType: CommandType.StoredProcedure, param: dynamicParameters);
            return result == null ? null : Slapper.AutoMapper.MapDynamic<T>(result, false) as IEnumerable<T>;
        }

        private async Task<T> QuerySingleOrDefaultPrimitiveAsync<T>(string procedure, DynamicParameters dynamicParameters)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(sql: procedure, transaction: transaction, commandType: CommandType.StoredProcedure, param: dynamicParameters);
        }

        private async Task<T> QuerySingleOrDefaultDynamicAsync<T>(string procedure, DynamicParameters dynamicParameters)
        {
            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql: procedure, transaction: transaction, commandType: CommandType.StoredProcedure, param: dynamicParameters);
            return result == null ? null : Slapper.AutoMapper.MapDynamic<T>(result, false);
        }

        /// <summary>
        /// Query one result of command satement sucess or fail
        /// </summary>
        /// <param name="storedName">name of stored proc</param>
        /// <param name="pars">paramater of stored proc</param>
        /// <returns>true or false</returns>
        protected async Task<bool> QueryFirstAsync(string storedName, DynamicParameters pars)
        {
            return await connection.QueryFirstAsync<bool>(storedName, pars, transaction, commandType: CommandType.StoredProcedure);
        }

        private bool IsPrimitiveType(Type type)
        {
            Type[] types = new Type[]
            {
                typeof (Enum), typeof (String), typeof (Char), typeof (Guid),
                typeof (Boolean), typeof (Byte), typeof (Int16), typeof (Int32),
                typeof (Int64), typeof (Single), typeof (Double), typeof (Decimal),
                typeof (SByte), typeof (UInt16), typeof (UInt32), typeof (UInt64),
                typeof (DateTime), typeof (DateTimeOffset), typeof (TimeSpan),
                typeof(int?), typeof(float?), typeof(double?), typeof(bool?)
            };
            return types.Any(x => x == type);
        }

        private async Task<IDictionary<string, object>> ReturnExecuteAsync(string sql, string[] outParamsName, DynamicParameters param = null, IDbTransaction dbTransaction = null, IDbConnection connection = null)
        {

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            param = param ?? new DynamicParameters();
            await connection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure, transaction: dbTransaction);

            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var item in outParamsName)
            {
                result.Add(item, param.Get<object>(item));
            }

            return result;

        }
    }
}