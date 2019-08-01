using BookMyCab.WebAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMyCab.WebAPI.Models.Database
{
    public class CabDriversRepository
    {
        CabDriversDBContext context;
        public CabDriversRepository()
        {
            context = new CabDriversDBContext();
        }

        public List<DriverDetail> GetDrivers()
        {
            List<DriverDetail> lst = new List<DriverDetail>();
            try
            {
                lst = context.DriverDetails.ToList();
            }
            catch (Exception)
            {
                lst = null;
            }

            return lst;
        }
    }
}