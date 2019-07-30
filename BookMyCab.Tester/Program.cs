using BookMyCab.AZStorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyCab.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region Storage Table Test
                //Console.WriteLine("----Storage Account Methods ----BEGIN------");
                //BookMyCabRepository bRes = new BookMyCabRepository();
                //Console.WriteLine(bRes.CreateCustomerTable());
                //Console.WriteLine(bRes.CreateTransaction());

                //var bal = bRes.FetchBalance("001");
                //Console.WriteLine("Balance is {0}", bal);
                //var tId = bRes.GenerateNewTransactionId();
                //Console.WriteLine("Transaction Id:{0}", tId);
                //Random random = new Random();
                ////Uncomment to Insert Customers
                ////var cstId = "C" + random.Next(5000) * random.Next(99);
                ////Customer ct = new Customer(cstId) { CustomerName = "User" + random.Next(999), Address = "Test Address"+ random.Next(5000), Balance = random.Next(999), Offer = random.Next(9) };
                ////var re = bRes.InsertEntityInCustomerTable(ct);
                ////Console.WriteLine("Insertion Success?: {0}", re);
                //List<Customer> lstCust = bRes.GetAllCustomers();
                //if (lstCust != null)
                //{
                //    foreach (Customer cst in lstCust)
                //    {
                //        Console.WriteLine("Customer Id: {0}, Name: {1}, Balance: {2}, Offer: {3}",cst.Rowkey, cst.CustomerName, cst.Balance, cst.Offer);
                //    }
                //}

                //var returnValue = bRes.UpdateOfferForCustomer(lstCust[0].Rowkey, 10);
                //Console.WriteLine("Update Success?: {0}", returnValue);

                //returnValue = bRes.UpdateWallet(lstCust[0].Rowkey, 100, "Credit", "001");
                //Console.WriteLine("Wallet Credit Success?: {0}", returnValue);

                //returnValue = bRes.UpdateWallet(lstCust[0].Rowkey, 15, "Debit", "001");
                //Console.WriteLine("Wallet Debit Success?: {0}", returnValue);

                //var ret = bRes.CountTransactions(lstCust[0].Rowkey,"Debit");
                //Console.WriteLine("Count of Transactions {0}", ret);

                //Console.WriteLine("----Storage Account Methods -----END------");
                #endregion

                #region service bus Test
                Console.WriteLine("--------Service Bus Test------");
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            Console.ReadLine();

        }
    }
}
