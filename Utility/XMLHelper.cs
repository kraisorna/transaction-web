using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TransactionEntity.Models;

namespace Utility
{
    public static class XMLHelper
    {
        public static FileStatus Parse(IFormFile file, List<TransactionImport> imports)
        {
            XmlReader xr = XmlReader.Create(file.OpenReadStream());

            string[] row = new string[5];

            while (xr.Read())
            {
                ImportStatus importStatus = ImportStatus.Success;

                switch (xr.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xr.Name.ToLower().Equals("transaction"))
                        {
                            row = new string[5];
                            row[0] = xr.GetAttribute("id");
                        }
                        else if (xr.Name.ToLower().Equals("transactiondate"))
                        {
                            row[3] = xr.ReadInnerXml().Replace('T', ' ');
                        }
                        else if (xr.Name.ToLower().Equals("amount"))
                        {
                            row[1] = xr.ReadInnerXml();
                        }
                        else if (xr.Name.ToLower().Equals("currencycode"))
                        {
                            row[2] = xr.ReadInnerXml();
                        }
                        else if (xr.Name.ToLower().Equals("status"))
                        {
                            row[4] = xr.ReadInnerXml();
                        }
                        continue;
                    case XmlNodeType.EndElement:
                        if (xr.Name.ToLower().Equals("transaction"))
                        {
                            goto NextStatements;
                        }
                        continue;
                    default:
                        continue;
                }

            NextStatements:
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
                else if (!DateTimeOffset.TryParse(row[3], out transactionDate))
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
                        case "rejected":
                            status = 'R';
                            break;
                        case "done":
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