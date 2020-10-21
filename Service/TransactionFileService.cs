using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using TransactionEntity.Models;
using Utility;

namespace Service
{
    public class TransactionFileService : BaseService<TransactionFile>, ITransactionFileService
    {
        public async Task<List<TransactionFile>> GetAll(TransactionDatabaseContext context)
        {
            return await context.Set<TransactionFile>().ToListAsync();
        }

        public async Task<TransactionFile> GetById(TransactionDatabaseContext context, int? id)
        {
            return await context.Set<TransactionFile>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public bool Exists(TransactionDatabaseContext context, int id)
        {
            return context.Set<TransactionFile>().Any(e => e.Id == id);
        }

        public FileStatus Upload(IFormFile file, List<TransactionImport> imports) {

            string ext = Path.GetExtension(file.FileName);
            if (ext.ToLower().Equals(".csv"))
                return CSVHelper.Parse(file, imports);
            else if (ext.ToLower().Equals(".xml"))
                return XMLHelper.Parse(file, imports);
            else
                return FileStatus.Fail;
        }
    }
}
