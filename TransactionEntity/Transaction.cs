using System;
using System.Collections.Generic;

namespace TransactionEntity.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public string TransactionIdentificator { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        public DateTimeOffset TransactionDate { get; set; }

        public int TransactionImportId { get; set; }
        public TransactionImport TransactionImport { get; set; }
    }
}
