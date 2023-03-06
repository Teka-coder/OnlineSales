using HagbesOnlineSales.DataAccess;
using HagbesOnlineSales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HagbesOnlineSales.Controllers
{
    [Authorize(Roles =("Admin"))]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {

            using (HagbesSalesContext _context = new HagbesSalesContext())
            {
                ViewBag.CountCustomers = _context.CustomerSet.Count();
                ViewBag.CountOrders = _context.OrderSet.Count();
                ViewBag.CountProducts = _context.ProductSet.Count();
            }

           return View();
        }

        public ActionResult GetDetails(string type)
        {
            List<ProductOrCustomerViewModel> result = GetProductOrCustomer(type);

            return PartialView("~/Views/Dashboard/GetDetails.cshtml", result);

        }


        public ActionResult TopCustomers()
        {
            List<TopCustomerViewModel> topFiveCustomers = null;
            using (HagbesSalesContext _context = new HagbesSalesContext())
            {
                var OrderByCustomer = (from o in _context.OrderSet
                                      group o by o.Customer.ID into g
                                      orderby g.Count() descending
                                      select new
                                      {
                                         CustomerID = g.Key,
                                         Count = g.Count()
                                      }).Take(5);

                 topFiveCustomers = (from c in _context.CustomerSet
                                  join o in OrderByCustomer
                                  on c.ID equals o.CustomerID
                                  select new TopCustomerViewModel
                                  {
                                      CustomerName = c.CustomerName,
                                      CustomerImage = c.CustomerImage,
                                      CustomerCountry = c.CustomerCountry,
                                      CountOrder = o.Count
                                  }).ToList();
            }

            return PartialView("~/Views/Dashboard/TopCustomers.cshtml", topFiveCustomers);
        }

        public ActionResult OrdersByCountry()
        {
            HagbesSalesContext _context = new HagbesSalesContext();
            
             var ordersByCountry = (from o in _context.OrderSet
                                       group o by o.Customer.CustomerCountry into g
                                       orderby g.Count() descending
                                       select new
                                       {
                                           Country = g.Key,
                                           CountOrders = g.Count()
                                       }).ToList();
            
            return Json(new { result = ordersByCountry }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomersByCountry()
        {
            HagbesSalesContext _context = new HagbesSalesContext();

            var customerByCountry = (from c in _context.CustomerSet
                                   group c by c.CustomerCountry into g
                                   orderby g.Count() descending
                                   select new
                                   {
                                       Country = g.Key,
                                       CountCustomer = g.Count()
                                   }).ToList();

            return Json(new { result = customerByCountry }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrdersByCustomer()
        {
            HagbesSalesContext _context = new HagbesSalesContext();
            var ordersByCustomer = (from o in _context.OrderSet
                                    group o by o.Customer.ID into g
                                    select new
                                    {
                                        Name = from c in _context.CustomerSet
                                               where c.ID == g.Key
                                               select c.CustomerName,

                                        CountOrders = g.Count()

                                    }).ToList();


            return Json(new { result = ordersByCustomer }, JsonRequestBehavior.AllowGet);
        }


        public List<ProductOrCustomerViewModel> GetProductOrCustomer(string type)
        {
            List<ProductOrCustomerViewModel> result = null;

            using (HagbesSalesContext _context = new DataAccess.HagbesSalesContext())
            {
                if (!string.IsNullOrEmpty(type))
                {
                    if (type == "customers")
                    {
                        result = _context.CustomerSet.Select(c => new ProductOrCustomerViewModel
                        {
                            Name = c.CustomerName,
                            Image = c.CustomerImage,
                            TypeOrCountry = c.CustomerCountry,
                            Type = "Customers"

                        }).ToList();

                    }
                    else if (type == "products")
                    {
                        result = _context.ProductSet.Select(p => new ProductOrCustomerViewModel
                        {
                            Name = p.ProductName,
                            Image = p.ProductImage,
                            TypeOrCountry = p.ProductType,
                            Type = p.ProductType

                        }).ToList();

                    }
                }

                return result;
            }

        }
    }
}