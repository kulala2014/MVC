using System.Data;
using System.Linq;
using System.Web.Mvc;
using MVCAttributeDemo.Models;
using System;
using MVCAttributeDemo.DAL;

namespace MVCAttributeDemo.Controllers
{
    public class PersonController : Controller
    {
       public ActionResult Index()
        {
            //try {
            //int x = 0;
            //int y = 1 / x;
            var people = DBManager.GetAction<IPersonRepository>().GetPersonList();
            return View(people);
            //    }
            //catch (Exception e)
            //{
            //    return View("Error");
            //}
        }

        public ViewResult Details(int id)
        {
            var person = DBManager.GetAction<IPersonRepository>().GetPersonById(id);
            return View(person);
        }

        public ActionResult Create()
        {
            return View(new Person());
        }

        [HttpPost]
        public ActionResult Create(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!DBManager.GetAction<IPersonRepository>().InsertNewPerson(person))
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(person);
        }

        public ActionResult Edit(int id)
        {
            var person = DBManager.GetAction<IPersonRepository>().GetPersonById(id);
            return View(person);
        }


        [HttpPost]
        public ActionResult Edit(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (DBManager.GetAction<IPersonRepository>().UpdatePersonInfo(person))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(person);
        }

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            var person = DBManager.GetAction<IPersonRepository>().GetPersonById(id);
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var person = DBManager.GetAction<IPersonRepository>().GetPersonById(id);
                if (!DBManager.GetAction<IPersonRepository>().DeletePerson(id))
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            catch (DataException)
            {
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary {
                 { "id", id },
                 { "saveChangesError", true } });
            }
            return RedirectToAction("Index");
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception e = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;
        //    filterContext.Result = new ViewResult()
        //    {
        //        ViewName = "Error"
        //    };
        //    //base.OnException(filterContext);
        //}

    }
}
