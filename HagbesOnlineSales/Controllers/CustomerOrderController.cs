using HagbesOnlineSales.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HagbesOnlineSales.Controllers
{
    public class CustomerOrderController : Controller
    {
        // GET: CustomerOrder
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomersList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCustomers()
        {
            using (HagbesSalesContext _context = new HagbesSalesContext())
            {
                var customerList = _context.CustomerSet.Select(c => new
                {
                    c.ID,
                    c.CustomerName,
                    c.CustomerEmail,
                    c.CustomerPhone,
                    c.CustomerCountry,
                    c.CustomerImage
                }).ToList();

                return Json(new { data = customerList }, JsonRequestBehavior.AllowGet);
            }

           
        }



        

    }
}