using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService<T> : IBaseService<T>
    {
        public async Task<int> Create(DbContext context, List<T> items)
        {
            context.AddRange(items);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Create(DbContext context, T item)
        {
            context.Add(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Edit(DbContext context, T item)
        {
            context.Update(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(DbContext context, T item)
        {
            context.Remove(item);
            return await context.SaveChangesAsync();
        }
    }
}
