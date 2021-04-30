using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private CS405Entities2 db = new CS405Entities2();

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