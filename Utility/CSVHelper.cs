using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TransactionEntity.Models;

namespace Utility
{
    public static class CSVHelper
    {
        public static FileStatus Parse(IFormFile file, List<TransactionImport> imports) {

            string[] dateTimeformats = { @"dd/MM/yyyy hh:mm:ss", @"yyyy-MM-dd hh:mm:ss" };

            //csv process
            StreamReader sr = new StreamReader(file.OpenReadStream());
            string line;
            string[] row = new string[5];
            char[] quotes = { '\"', ' ' };
            while ((line = sr.ReadLine()) != null)
            {
                ImportStatus importStatus = ImportStatus.Success;

                //Define pattern
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                //Separating columns to array
                row = CSVParser.Split(line).Select(s => s.Trim(quotes).Replace("\\\"", "\"")).ToArray();

                string note = string.Empty;
                if (string.IsNullOrEmpty(row[0]))
                {
                    note += "TransactionIdentificator is missing. ";
                    importStatus = ImportStatus.Fail;
                }
                decimal amount = decimal.Zero;
                if (string.IsNullOrEmpty(row[1]))
                {
                    note += "Amount is missing. ";
                    importStatus = ImportStatus.Fail;
                }
                else if (!decimal.TryParse(row[1], out amount))
                {
                    note += "Amount cannot be parsed. ";
                    importStatus = ImportStatus.Fail;
                }
                if (string.IsNullOrEmpty(row[2]))
                {
                    note += "CurrencyCode is missing. ";
                    importStatus = ImportStatus.Fail;
                }
                DateTimeOffset transactionDate = DateTimeOffset.MinValue;
                if (string.IsNullOrEmpty(row[3]))
                {
                    note += "TransactionDate is missing. ";
                    importStatus = ImportStatus.Fail;
                }
                else if (!DateTimeOffset.TryParseExact(row[3], dateTimeformats, CultureInfo.InvariantCulture, DateTimeStyles.None, out transactionDate))
                {
                    note += "TransactionDate cannot be parsed. ";
                    importStatus = ImportStatus.Fail;
                }
                char status = char.MinValue;
                if (string.IsNullOrEmpty(row[4]))
                {
                    note += "Status is missing. ";
                    importStatus = ImportStatus.Fail;
                }
                else
                {
                    switch (row[4].ToLower())
                    {
                        case "approved":
                            status = 'A';
                            break;
                        case "failed":
                            status = 'R';
                            break;
                        case "finished":
                            status = 'D';
                            break;
                        default:
                            break;
                    }
                }

                imports.Add(new TransactionImport
                {
                    TransactionId = row[0],
                    Amount = amount,
                    CurrencyCode = row[2],
                    TransactionDate = transactionDate,
                    Status = status.ToString(),
                    ImportDate = DateTimeOffset.UtcNow,
                    ImportStatus = importStatus,
                    Note = note,
                });

                if (importStatus == ImportStatus.Fail) return FileStatus.Fail;
            }

            return FileStatus.Success;
        }
    }
}