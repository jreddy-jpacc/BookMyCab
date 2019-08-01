using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using BookMyCab.WebAPI.Models.StorageService;

namespace BookMyCab.WebJobAuto
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            try
            {
                BookMyCabRepository bookMyCab = new BookMyCabRepository();
                List<Customer> customers = bookMyCab.GetAllCustomers();
                List<Customer> customersToUpdate = bookMyCab.GetAllCustomers();
                foreach (var item in customers)
                {
                    var count = bookMyCab.CountTransactions(item.RowKey, "Debit");
                    if (count > 4 && item.Offer != 10)
                    {
                        item.Offer = 10;
                        bookMyCab.UpdateOfferForCustomer(item.RowKey, item.Offer);
                        Console.WriteLine("Update Offer for Customer Id: {0} to {1}", item.RowKey, item.Offer);

                    }
                    else if (count > 2 && count <= 4 && item.Offer != 5)
                    {
                        item.Offer = 5;
                        customersToUpdate.Add(item);
                        bookMyCab.UpdateOfferForCustomer(item.RowKey, item.Offer);
                        Console.WriteLine("Update Offer for Customer Id: {0} to {1}", item.RowKey, item.Offer);
                    }
                    else if (count < 2 && item.Offer != 7)
                    {
                        item.Offer = 7;
                        customersToUpdate.Add(item);
                        bookMyCab.UpdateOfferForCustomer(item.RowKey, item.Offer);
                        Console.WriteLine("Update Offer for Customer Id: {0} to {1}", item.RowKey, item.Offer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }
    }
}
