using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public interface ITransactionService: IBaseService<Transaction>
    {
        Task<List<Transaction>> GetAll(TransactionDatabaseContext context);
        Task<Transaction> GetById(TransactionDatabaseContext context, int? id);
        bool Exists(TransactionDatabaseContext context, int id);
    }
}
