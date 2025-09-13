using GM.DAL.Respository;
using System.Data;

namespace GM.DAL
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPaymentRepository PaymentRepository { get; }
    }

    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection connection = null;
        private readonly IDbTransaction transaction = null;

        public UnitOfWork(IDbConnection connection, IDbTransaction transaction)
        {
             this.connection = connection;
            this.transaction = transaction;
        }

        private IUserRepository _userRepository = null;
        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(connection, transaction);

        private IPaymentRepository _paymentsRepository = null;
        public IPaymentRepository PaymentRepository => _paymentsRepository = _paymentsRepository ?? new PaymentRepository(connection, transaction);



    }
}