using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TransactionEntity.Models;
using TransactionWeb.Models;
using Utility;

namespace TransactionWeb.Controllers
{
    public class TransactionFileController : Controller
    {
        private readonly TransactionDatabaseContext _context;
        private readonly ILogger<TransactionFileController> _logger;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".csv", ".xml" };
        private readonly ITransactionFileService _transactionFileService;
        private readonly ITransactionImportService _transactionImportService;
        private readonly ITransactionService _transactionService;

        public TransactionFileController(ILogger<TransactionFileController> logger, TransactionDatabaseContext context, IConfiguration config, ITransactionFileService transactionFileService,
                                        ITransactionImportService transactionImportService, ITransactionService transactionService)
        {
            _logger = logger;
            _context = context;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _transactionFileService = transactionFileService;
            _transactionImportService = transactionImportService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View(new UploadViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadViewModel viewModel)
        {
            
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return BadRequest(ModelState);
            }

            IFormFile file = Request.Form.Files[0];
            string fileName = file.FileName;
            byte[] streamedFileContent = new byte[0];
            streamedFileContent = await FileHelper.ProcessStreamedFile(file, ModelState, _permittedExtensions, _fileSizeLimit);
            if (streamedFileContent.Length == 0)
            {
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                List<TransactionImport> imports = new List<TransactionImport>();
                FileStatus fileStatus = _transactionFileService.Upload(file, imports);

                var transactionFile = new TransactionFile()
                {
                    Content = streamedFileContent,
                    FileName = fileName,
                    Note = viewModel.Note,
                    Size = streamedFileContent.Length,
                    UploadDate = DateTimeOffset.UtcNow,
                    FileStatus = fileStatus
                };

                await _transactionFileService.Create(_context, transactionFile);

                imports = imports.OrderBy(x => x.Id).Select(x => { x.TransactionFileId = transactionFile.Id; return x; }).ToList();

                try
                {
                    _context.TransactionImport.AddRange(imports);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                }

                if (fileStatus == FileStatus.Success)
                {
                    List<Transaction> transactions = new List<Transaction>();
                    transactions = imports.Where(x => x.ImportStatus == ImportStatus.Success).Select(x => new Transaction
                    {
                        TransactionIdentificator = x.TransactionId,
                        Amount = (decimal)x.Amount,
                        CurrencyCode = x.CurrencyCode,
                        TransactionDate = (DateTimeOffset)x.TransactionDate,
                        Status = x.Status,
                        TransactionImportId = x.Id,
                    }).ToList();

                    try
                    {
                        foreach (Transaction transaction in transactions)
                        {
                            await _transactionService.Create(_context, transaction);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return Created(nameof(TransactionFileController), null);
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader =
                MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        // GET: TransactionFile
        public async Task<IActionResult> Index()
        {
            return View(await _transactionFileService.GetAll(_context));
        }

        // GET: TransactionFile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionFile = await _transactionFileService.GetById(_context, id);
            if (transactionFile == null)
            {
                return NotFound();
            }

            return View(transactionFile);
        }

        // GET: TransactionFile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransactionFile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,FileName,Note,Size,UploadDate")] TransactionFile transactionFile)
        {
            if (ModelState.IsValid)
            {
                await _transactionFileService.Create(_context, transactionFile);
                return RedirectToAction(nameof(Index));
            }
            return View(transactionFile);
        }

        // GET: TransactionFile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionFile = await _transactionFileService.GetById(_context, id);
            if (transactionFile == null)
            {
                return NotFound();
            }
            return View(transactionFile);
        }

        // POST: TransactionFile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,FileName,Note,Size,UploadDate")] TransactionFile transactionFile)
        {
            if (id != transactionFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transactionFileService.Edit(_context, transactionFile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionFileExists(transactionFile.Id))
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
            return View(transactionFile);
        }

        // GET: TransactionFile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionFile = await _transactionFileService.GetById(_context, id);
            if (transactionFile == null)
            {
                return NotFound();
            }

            return View(transactionFile);
        }

        // POST: TransactionFile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionFile = await _transactionFileService.GetById(_context, id);
            await _transactionFileService.Delete(_context, transactionFile);
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionFileExists(int id)
        {
            return _transactionFileService.Exists(_context, id);
        }
    }
}
