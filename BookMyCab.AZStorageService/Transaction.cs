using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyCab.AZStorageService
{
    public class Transaction : TableEntity
    {
        public Transaction() { }
        public Transaction(string tranId, string custId)
        {
            this.PartitionKey = custId;
            this.RowKey = tranId;
        }

        public DateTime TransactionDate { get; set; }

       public string Type { get; set; }

        public double Amount { get; set; }

        public string DriverId { get; set; }
    }
}
