using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBricks.Core;

namespace MVCBricks.Controllers
{
    [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BricksController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBoard()
        {
            return new JsonResult() { Data = GameManager.Instance.CurrentBoard, 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Tick()
        {
            GameManager.Instance.Presenter.Tick();
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MoveLeft()
        {
            GameManager.Instance.Presenter.MoveLeft();
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MoveUp()
        {
            GameManager.Instance.Presenter.Rotate90();
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MoveRight()
        {
            GameManager.Instance.Presenter.MoveRight();
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult MoveDown()
        {
            GameManager.Instance.Presenter.MoveDown();
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult InitializeBoard()
        {
            GameManager.Instance.InitializeBoard();
            
            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
