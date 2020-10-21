using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service;
using System.Threading.Tasks;
using TransactionEntity.Models;

namespace TransactionWeb.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionDatabaseContext _context;
        private readonly ITransactionService _transactionService;

        public TransactionController(TransactionDatabaseContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _transactionService.GetAll(_context));
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetById(_context, id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            ViewData["TransactionImportId"] = new SelectList(_context.TransactionImport, "Id", "Id");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TransactionId,Amount,CurrencyCode,Status,TransactionDate,TransactionImportId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.Create(_context, transaction);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactionImportId"] = new SelectList(_context.TransactionImport, "Id", "Id", transaction.TransactionImportId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetById(_context, id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["TransactionImportId"] = new SelectList(_context.TransactionImport, "Id", "Id", transaction.TransactionImportId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransactionId,Amount,CurrencyCode,Status,TransactionDate,TransactionImportId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transactionService.Edit(_context, transaction);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactionImportId"] = new SelectList(_context.TransactionImport, "Id", "Id", transaction.TransactionImportId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetById(_context, id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _transactionService.GetById(_context, id);
            await _transactionService.Delete(_context, transaction);
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _transactionService.Exists(_context, id);
        }
    }
}
