using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public class TransactionImportService : BaseService<TransactionImport>, ITransactionImportService
    {
        public async Task<List<TransactionImport>> GetAll(TransactionDatabaseContext context)
        {
            return await context.Set<TransactionImport>().ToListAsync();
        }

        public async Task<TransactionImport> GetById(TransactionDatabaseContext context, int? id)
        {
            return await context.Set<TransactionImport>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public bool Exists(TransactionDatabaseContext context, int id)
        {
            return context.Set<TransactionImport>().Any(e => e.Id == id);
        }
    }
}
