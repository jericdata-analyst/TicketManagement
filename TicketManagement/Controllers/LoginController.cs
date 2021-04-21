using TicketManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(TicketManagement.Models.tblaccount account)
        {
            using (CS405Entities2 db = new CS405Entities2())
            {
                var userDetails = db.tblaccounts.Where(x => x.username == account.username && x.password == account.password).FirstOrDefault();
                var userType = db.tblaccounts.Where(x => x.usertype == account.usertype).FirstOrDefault();

                if (userDetails == null)
                {
                    account.LoginErrorMessage = "Wrong username or password";
                    return View("Index", account);
                }
                else
                {
                    Session["id"] = userDetails.id;
                    Session["username"] = userDetails.username;
                    Session["usertype"] = userDetails.usertype;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            int userId = (int)Session["id"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}