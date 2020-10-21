using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace Service
{
    public interface IBaseService<T>
    {
        Task<int> Create(DbContext context, List<T> items);
        Task<int> Create(DbContext context, T item);
        Task<int> Edit(DbContext context, T item);
        Task<int> Delete(DbContext context, T item);
    }
}
