using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookMyCab.AZStorageService
{
    public class BookMyCabRepository
    {
        #region Varibles & Constructor
        /// <summary>
        /// Variables
        /// </summary>
        public CloudTable customerTable;
        public CloudStorageAccount storageAccount;
        public CloudTableClient tableClient;
        public CloudTable transactionTable;


        public BookMyCabRepository()
        {
            string conStr = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            storageAccount = CloudStorageAccount.Parse(conStr);
            tableClient = storageAccount.CreateCloudTableClient();
            customerTable = tableClient.GetTableReference("Customer");
            transactionTable = tableClient.GetTableReference("Transaction");

        }

        #endregion



        #region Business operations
        /// <summary>
        /// Create the Customer Id.
        /// </summary>
        /// <returns></returns>
        public bool CreateCustomerTable()
        {
            bool status;
            try
            {
                customerTable.CreateIfNotExists();

                status = (customerTable != null) ? true : false;
            }
            catch
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// Create the Transaction Table.
        /// </summary>
        /// <returns></returns>
        public bool CreateTransaction()
        {
            bool status;
            try
            {
                transactionTable.CreateIfNotExists();
                status = (transactionTable != null) ? true : false;
            }
            catch
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// Retrieve the Number of Transactoins for CustomerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public int CountTransactions(string CustomerId, string type)
        {
            try
            {
                var cntTran = 0;
                var query = "PartitionKey eq '" + CustomerId + "' and Type eq '" + type + "'";
                List<Transaction> tListObj = RetrieveEntity<Transaction>(transactionTable, query);
                cntTran = tListObj.Count();
                return cntTran;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -99;
            }


        }

        /// <summary>
        /// Fetch the balance for customer.
        /// </summary>
        /// <param name="cusotmerId"></param>
        /// <returns></returns>
        public double FetchBalance(string cusotmerId)
        {
            double balance = 0;
            try
            {
                var query = "RowKey eq '" + cusotmerId + "'";
                List<Customer> lstCust = RetrieveEntity<Customer>(customerTable, query);
                balance = lstCust.FirstOrDefault().Balance;
            }
            catch
            {
                balance = 0;
            }
            return balance;
        }

        /// <summary>
        /// Generate the Transaction Id
        /// </summary>
        /// <returns></returns>
        public string GenerateNewTransactionId()
        {
            string tranID;
            try
            {
                tranID = RandomString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception {0}", ex.Message);
                tranID = "NA";
            }
            return tranID;

        }

        /// <summary>
        /// Returns the Cusotmer Details.
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetAllCustomers()
        {
            List<Customer> lstCustomer = new List<Customer>();
            try
            {
                lstCustomer = RetrieveEntity<Customer>(customerTable);
            }
            catch
            {
                return null;
            }
            return lstCustomer;
        }

        /// <summary>
        /// Inserts entry in customer table.
        /// </summary>
        /// <param name="cst"></param>
        /// <returns>false in case of exception.</returns>
        public bool InsertEntityInCustomerTable(Customer cst)
        {
            try
            {
                InsertEntity<Customer>(customerTable, cst, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the Offer for Customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="offerPercentage"></param>
        /// <returns></returns>
        public bool UpdateOfferForCustomer(string customerId, int offerPercentage)
        {
            try
            {
                string query = "RowKey eq '" + customerId + "'";
                Customer cst = RetrieveEntity<Customer>(customerTable, query).FirstOrDefault<Customer>();
                cst.Offer = offerPercentage;
                InsertEntity<Customer>(customerTable, cst, false);
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the Wallet amount for customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public bool UpdateWallet(string customerId, double amount, string type, string driverId = "0")
        {
            try
            {
                string tranId = GenerateNewTransactionId();
                string query = "RowKey eq '" + customerId + "'";
                bool status = false;
                Customer custObj = RetrieveEntity<Customer>(customerTable, query).FirstOrDefault<Customer>();
                double balance = 0;
                if (custObj != null)
                {
                    Transaction tranObj = new Transaction(tranId, custObj.Rowkey);
                    balance = custObj.Balance;

                    if (type.Equals("Debit"))
                    {
                        if (amount <= balance)
                        {
                            double offerAmount = 0;
                            offerAmount = (custObj.Offer * (amount / 10));
                            custObj.Balance -= (amount - offerAmount);
                            InsertEntity<Customer>(customerTable, custObj, false);
                            tranObj.TransactionDate = DateTime.Now;
                            tranObj.Type = "Debit";
                            tranObj.Amount = offerAmount;
                            tranObj.DriverId = driverId;
                            InsertEntity<Transaction>(transactionTable, tranObj, true);
                            status = true;
                        }
                    }
                    else if (type.Equals("Credit") && (amount >= 100 && amount <= 5000))
                    {
                        custObj.Balance += amount;
                        InsertEntity<Customer>(customerTable, custObj, false);
                        tranObj.TransactionDate = DateTime.Now;
                        tranObj.Type = "Credit";
                        tranObj.Amount = amount;
                        tranObj.DriverId = "0";
                        InsertEntity<Transaction>(transactionTable, tranObj, true);
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                return status;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DAL Operations

        /// <summary>
        /// Logic to Retrieve data from Storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> RetrieveEntity<T>(CloudTable table, string query = null) where T : TableEntity, new()
        {
            try
            {
                // Create the Table Query Object for Azure Table Storage  
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!String.IsNullOrEmpty(query))
                {
                    DataTableQuery = new TableQuery<T>().Where(query);
                }
                IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> DataList = new List<T>();
                foreach (var singleData in IDataList)
                    DataList.Add(singleData);
                return DataList;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        /// <summary>
        /// Insert Operation on storage table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <param name="forInsert"></param>
        public void InsertEntity<T>(CloudTable table, T entity, bool forInsert = true) where T : TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var insertOperation = TableOperation.Insert(entity);
                    table.Execute(insertOperation);
                }
                else
                {
                    var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                    table.Execute(insertOrMergeOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        /// <summary>
        /// Delete opetation on Storage table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity<T>(CloudTable table, T entity) where T : TableEntity, new()
        {
            try
            {
                var DeleteOperation = TableOperation.Delete(entity);
                table.Execute(DeleteOperation);
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        #endregion

        #region Misc
        /// <summary>
        ///Generate a random string for Transaction Id  
        /// </summary>
        /// <returns></returns>
        public string RandomString()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            builder.Append(DateTime.UtcNow.Millisecond);
            builder.Append(DateTime.UtcNow.Minute);
            builder.Append(DateTime.UtcNow.Hour);
            for (int i = 0; i < 9; i++)
            {
                //Append timestmap in Middle.
                if (i == 5)
                {
                    builder.Append(DateTime.UtcNow.Day);
                    builder.Append(DateTime.UtcNow.Month);
                    builder.Append(DateTime.UtcNow.Year);
                }
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        #endregion
    }
}

