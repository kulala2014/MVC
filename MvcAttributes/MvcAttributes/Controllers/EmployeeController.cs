using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAttributes.Models;

namespace MvcAttributes.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EmployeeRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeRegister([Bind(Exclude="Address")] Employee emp)
        {
            return View();
        }

        public JsonResult CheckEmail(string Email)
        {
            //Check here in database if it exist in database return true else false.
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HandleError(ExceptionType=typeof(DivideByZeroException),View="DivideByZeroErrorView")]
        [HandleError(ExceptionType = typeof(NullReferenceException), View = "NullRefrenceErrorView")]
        public ActionResult CheckError()
        {
            int a = 10;
            int b = 0;
            int k = a / b;
            return View();
        }

    }
}
