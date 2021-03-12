using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    public class HomeController : Controller
    {
        private CS405Entities1 db = new CS405Entities1();

        public ActionResult Index()
        {
            return View();
        }

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
    }
}