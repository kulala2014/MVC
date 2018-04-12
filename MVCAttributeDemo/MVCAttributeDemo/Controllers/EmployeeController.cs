using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCAttributeDemo.Models;

namespace MVCAttributeDemo.Controllers
{
    public class EmployeeController: Controller
    {
         [HttpGet]
        public ActionResult EmployeeRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeRegister(Employee emp)
        {
            ViewData["Message"] = "Test set value in ViewData.Get value from ViewBag";
            return View();
        }

        /******************Use ModelState Object to Validation***********************/
        //[HttpPost]
        //public ActionResult EmployeeRegister(Employee emp)
        //{
        //    if (string.IsNullOrEmpty(emp.Name))
        //    {
        //        ModelState.AddModelError("Name", "Name is requeired");
        //    }

        //    if (string.IsNullOrEmpty(emp.Email))
        //    {
        //        ModelState.AddModelError("Email", "Email is requeired");
        //    }
        //    if (string.IsNullOrEmpty(emp.Address))
        //    {
        //        ModelState.AddModelError("Address", "Address is requeired");
        //    }
        //    if (string.IsNullOrEmpty(emp.PhoneNo))
        //    {
        //        ModelState.AddModelError("PhoneNo", "PhoneNo is requeired");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Index","Home");
        //    }

        //    return View();
        //}

        public JsonResult CheckEmail(string Email)
        {
            if (!String.IsNullOrEmpty(Email) && Email.Equals("kulala@123.com"))
            {
                return Json("Email Id already exists", JsonRequestBehavior.AllowGet);
            }
            return Json("true", JsonRequestBehavior.AllowGet);                
        }

        [HandleError(ExceptionType =typeof(DivideByZeroException), View = "NullRefrenceErrorView")]
        public ActionResult CheckError()
        {
            int a = 10;
            int b = 0;
            return View();
        }
    }
}