using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookMyCab.WebAPI.Models.Database;
using BookMyCab.WebAPI.Models;
using BookMyCab.WebAPI.Models.StorageService;

namespace BookMyCab.WebAPI.Controllers
{
    public class CoreController : ApiController
    {
        static CabDriversRepository dbRepo = new CabDriversRepository();
        static BookMyCabRepository stRepo = new BookMyCabRepository();

        [Route("Customers")]
        [HttpGet]
        public List<Customer> FetchAllCustomers()
        {
            try
            {
                List<Customer> customers;
                customers = stRepo.GetAllCustomers();
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: {0}", ex.Message);
                return null;
            }
        }

        [Route("Balance/{cusotmerId}")]
        [HttpGet]
        public double FetchBalance(string cusotmerId)
        {
            double balance = 0;
            try
            {
                balance = stRepo.FetchBalance(cusotmerId);
                return balance;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: {0}", ex.Message);
                return 0;
            }
        }

        [Route("Customer/{customer}")]
        [HttpPost]
        public string InsertCustomer([FromBody]Customer customer)
        {
            try
            {
                var stat = (stRepo.InsertEntityInCustomerTable(customer)) ? "Cusotmer Inserted into Storage." : "Unable to Insert";
                return stat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: {0}", ex.Message);
                return ex.Message;
            }
        }

        [Route("Drivers")]
        [HttpGet]
        public List<DriverDetail> GetDrivers()
        {
            List<DriverDetail> lst = new List<DriverDetail>();
            try
            {
                lst = dbRepo.GetDrivers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured: {0}", ex.Message);
                lst = null;
            }
            return lst;
        }

        [Route("UpdateWallet/{wallet}")]
        [HttpPost]
        public string UpdateWallet([FromBody]Wallet wallet)
        {
            try
            {
                var stat = (stRepo.UpdateWallet(wallet)) ? "Transaction Inserted into Storage." : "Unable to Insert";
                return stat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured: {0}", ex.Message);
                return ex.Message;
            }
            
        }
    }
}