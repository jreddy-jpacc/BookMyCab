using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyCab.WebAPI.Models.StorageService
{
    public class Wallet
    {
        public string customerId { get; set; }
        public double amount { get; set; }
        public string type { get; set; }
        public string driverId { get; set; }
    }
}