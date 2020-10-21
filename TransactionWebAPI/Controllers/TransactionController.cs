using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionEntity.Models;

namespace TransactionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionDatabaseContext _context;

        public TransactionController(TransactionDatabaseContext context)
        {
            _context = context;
        }

        // GET: Transaction/currency/USD
        [HttpGet("currency/{currencyCode}")]
        public async Task<ActionResult<IEnumerable<APITransaction>>> GetTransactionByCurrency(string currencyCode)
        {
            List<APITransaction> transactions = await _context.Transaction.Where(x => x.CurrencyCode.ToLower() == currencyCode.ToLower()).Select(x => new APITransaction
            {
                id = x.TransactionIdentificator,
                payment = String.Format("{0:0.00} ", x.Amount) + x.CurrencyCode.ToUpper(),
                Status = x.Status,
            }).ToListAsync();
            return transactions;
        }

        // GET: Transaction/date/2019-10-10?endDate=2020-10-10
        [HttpGet("date/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<APITransaction>>> GetTransactionByDateRange(DateTime startDate, DateTime endDate)
        {
            List<APITransaction> transactions = await _context.Transaction.Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate).Select(x => new APITransaction
            {
                id = x.TransactionIdentificator,
                payment = String.Format("{0:0.00} ", x.Amount) + x.CurrencyCode.ToUpper(),
                Status = x.Status,
            }).ToListAsync();
            return transactions;
        }

        // GET: Transaction/status/USD
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<APITransaction>>> GetTransactionByStatus(string status)
        {
            List<APITransaction> transactions = await _context.Transaction.Where(x => x.Status.ToLower() == status.ToLower()).Select(x => new APITransaction
            {
                id = x.TransactionIdentificator,
                payment = String.Format("{0:0.00} ", x.Amount) + x.CurrencyCode.ToUpper(),
                Status = x.Status,
            }).ToListAsync();
            return transactions;
        }

        // GET: Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
            return await _context.Transaction.ToListAsync();
        }

        // GET: Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: Transaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Transaction
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: Transaction/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }
    }
}
