using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public class TransactionService : BaseService<Transaction>, ITransactionService
    {
        public async Task<List<Transaction>> GetAll(TransactionDatabaseContext context)
        {
            return await context.Set<Transaction>().ToListAsync();
        }

        public async Task<Transaction> GetById(TransactionDatabaseContext context, int? id)
        {
            return await context.Set<Transaction>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public bool Exists(TransactionDatabaseContext context, int id)
        {
            return context.Set<Transaction>().Any(e => e.Id == id);
        }
    }
}
