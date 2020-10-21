using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service;
using TransactionEntity.Models;

namespace TransactionWeb.Views.Home
{
    public class TransactionImportController : Controller
    {
        private readonly TransactionDatabaseContext _context;
        private readonly ITransactionImportService _transactionImportService;

        public TransactionImportController(TransactionDatabaseContext context, ITransactionImportService transactionImportService)
        {
            _context = context;
            _transactionImportService = transactionImportService;
        }

        // GET: TransactionImport
        public async Task<IActionResult> Index()
        {
            return View(await _transactionImportService.GetAll(_context));
        }

        // GET: TransactionImport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionImport = await _transactionImportService.GetById(_context, id);
            if (transactionImport == null)
            {
                return NotFound();
            }

            return View(transactionImport);
        }

        // GET: TransactionImport/Create
        public IActionResult Create()
        {
            ViewData["TransactionFileId"] = new SelectList(_context.TransactionFile, "Id", "Id");
            return View();
        }

        // POST: TransactionImport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TransactionId,Amount,CurrencyCode,Status,TransactionDate,ImportStatus,Note,ImportDate,TransactionFileId")] TransactionImport transactionImport)
        {
            if (ModelState.IsValid)
            {
                await _transactionImportService.Create(_context, transactionImport);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransactionFileId"] = new SelectList(_context.TransactionFile, "Id", "Id", transactionImport.TransactionFileId);
            return View(transactionImport);
        }

        // GET: TransactionImport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionImport = await _transactionImportService.GetById(_context, id);
            if (transactionImport == null)
            {
                return NotFound();
            }
            ViewData["TransactionFileId"] = new SelectList(_context.TransactionFile, "Id", "Id", transactionImport.TransactionFileId);
            return View(transactionImport);
        }

        // POST: TransactionImport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransactionId,Amount,CurrencyCode,Status,TransactionDate,ImportStatus,Note,ImportDate,TransactionFileId")] TransactionImport transactionImport)
        {
            if (id != transactionImport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transactionImportService.Edit(_context, transactionImport);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionImportExists(transactionImport.Id))
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
            ViewData["TransactionFileId"] = new SelectList(_context.TransactionFile, "Id", "Id", transactionImport.TransactionFileId);
            return View(transactionImport);
        }

        // GET: TransactionImport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionImport = await _transactionImportService.GetById(_context, id);
            if (transactionImport == null)
            {
                return NotFound();
            }

            return View(transactionImport);
        }

        // POST: TransactionImport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionImport = await _transactionImportService.GetById(_context, id);
            await _transactionImportService.Delete(_context, transactionImport);
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionImportExists(int id)
        {
            return _transactionImportService.Exists(_context, id);
        }
    }
}
