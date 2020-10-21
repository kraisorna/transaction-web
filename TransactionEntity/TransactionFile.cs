using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransactionEntity.Models
{
    public enum FileStatus
    {
        Success,
        Fail
    }
    public class TransactionFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset UploadDate { get; set; }

        public FileStatus FileStatus { get; set; }

        public ICollection<TransactionImport> Imports { get; set; }
    }
}