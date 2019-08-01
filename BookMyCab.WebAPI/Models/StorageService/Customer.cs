using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyCab.WebAPI.Models.StorageService
{
    public class Customer : TableEntity
    {
        public Customer() { }
        public Customer(string CustId)
        {
            this.PartitionKey = "Customer";
            this.RowKey = CustId;
        }

        public string CustomerName { get; set; }
        public double Balance { get; set; }
        public string Address { get; set; }
        public int Offer { get; set; }
        public string Rowkey
        {
            get
            {
                return this.RowKey;
            }
        }
    }
}
