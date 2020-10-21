using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public interface ITransactionImportService : IBaseService<TransactionImport>
    {
        Task<List<TransactionImport>> GetAll(TransactionDatabaseContext context);
        Task<TransactionImport> GetById(TransactionDatabaseContext context, int? id);
        bool Exists(TransactionDatabaseContext context, int id);
    }
}
