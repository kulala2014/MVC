using System.Web.Mvc;
using WebApplication2.Models.ViewModels;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "This Message is coming from ViewData";

            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "This Message is coming from ViewBag";

            return View();
        }

        public ActionResult SampleBook()
        {
            Book book = new Book
            {
                ID = 1,
                BookName = "Sample Book",
                Author = "Sample Author",
                ISBN = "Not available"
            };

            return View(book);
        }

        public ActionResult SampleBook2()
        {
            Book book = new Book
            {
                ID = 1,
                BookName = "Sample Book",
                Author = "Sample Author",
                ISBN = "Not available"
            };

            Message msg = new Message
            {
                MessageText = "This is a Sample Message",
                MessageFrom = "Test user"
            };

            ShowBookAndMessageViewModel viewModel = new ShowBookAndMessageViewModel
            {
                Message = msg,
                Book = book
            };

            return View(viewModel);
        }

        public ActionResult SampleBook3()
        {
            Book book = new Book
            {
                ID = 1,
                BookName = "Sample Book",
                Author = "Sample Author",
                ISBN = "Not available"
            };

            TempData["BookData"] = book;
            return RedirectToAction("SampleBook4");
        }

        public ActionResult SampleBook4()
        {
            Book book = TempData["BookData"] as Book;

            return View(book);
        }

        public ActionResult SampleBook5()
        {
            Book book = new Book
            {
                ID = 1,
                BookName = "Sample Book",
                Author = "Sample Author",
                ISBN = "Not available"
            };

            Session["BookData"] = book;
            return RedirectToAction("SampleBook6");
        }

        public ActionResult SampleBook6()
        {
            Book book = Session["BookData"] as Book;

            return View(book);
        }
    }
}