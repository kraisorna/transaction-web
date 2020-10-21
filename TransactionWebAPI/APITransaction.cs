using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionWebAPI
{
    public class APITransaction
    {
        public string id { get; set; }
        public string payment { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }
}
