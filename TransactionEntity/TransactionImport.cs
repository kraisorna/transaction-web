using System;

namespace TransactionEntity.Models
{
    public enum ImportStatus
    {
        Success,
        Fail
    }
    public class TransactionImport
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public ImportStatus ImportStatus { get; set; }
        public string Note { get; set; }
        public DateTimeOffset ImportDate { get; set; }

        public int TransactionFileId { get; set; }
        public TransactionFile TransactionFile { get; set; }
        public Transaction Transaction { get; set; }
    }
}
