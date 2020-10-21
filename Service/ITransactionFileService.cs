using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public interface ITransactionFileService: IBaseService<TransactionFile>
    {
        Task<List<TransactionFile>> GetAll(TransactionDatabaseContext context);
        Task<TransactionFile> GetById(TransactionDatabaseContext context, int? id);
        bool Exists(TransactionDatabaseContext context, int id);
        FileStatus Upload(IFormFile file, List<TransactionImport> imports);
    }
}
