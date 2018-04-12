using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using WebGridSampleApplication.Models;

namespace WebGridSampleApplication.Controllers
{
    public class InventoryController : Controller
    {
        public ActionResult WebgridSample()
        {
            ObservableCollection<Product> inventoryList = DataBaseHelper.GetProductList();
            return View(inventoryList);
        }

        public ActionResult UpdateProduct(string product)
        {
            //if (DataBaseHelper.UpdateProduct(product))
            //{
            //    return View();
            //}
            return View();
        }
    }
}
