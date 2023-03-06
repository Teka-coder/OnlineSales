using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HagbesOnlineSales.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        // GET: Test
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Foo()
        {
            return View();
        }
    }
}