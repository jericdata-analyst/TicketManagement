using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    public class TicketsController : Controller
    {
        // GET: Tickets
        private CS405Entities2 db = new CS405Entities2();

        [HandleError]
        public ActionResult Index(string txtsearch)
        {
            ViewBag.type = 0;

            ////create a variable for all the list users;
            //var uTypeList = new List<string>();

            ////create a query on selecting all the users with usertyoe
            //var uTypequery = from d in db.tbltickets orderby d.TicketNumber select d.TicketNumber;

            ////add the result of the query on the variable list
            //uTypeList.AddRange(uTypequery.Distinct());

            ////create the view bag of the resulet
            //ViewBag.TicketNumber = new SelectList(uTypeList);

            var user = Session["username"].ToString();

            //select all the users
            var accts = from m in db.tbltickets select m;

            if (!String.IsNullOrEmpty(txtsearch))
            {
                accts = accts.Where(s => s.Status.Contains(txtsearch));
                accts = accts.Where(s => s.TicketNumber.Contains(txtsearch));
            }

            if (Session["usertype"].ToString() == "User")
            {
                accts = accts.Where(s => s.CreatedBy.Contains(user));
            }

            if (Session["usertype"].ToString() == "Technical")
            {
                accts = accts.Where(s => s.AssignedTo.Contains(user));
            }

            //return all the user
            return View(accts.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //form post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblticket newTicket)
        {
            if (!ModelState.IsValid)
                return View(newTicket);
            if (db.tbltickets.Any(k => k.TicketNumber == newTicket.TicketNumber))
            {
                ModelState.AddModelError("TicketNumber", "Username already exist");
                return View(newTicket);
            }
            TempData["Msg"] = "Ticket Successfully Added";
            db.tbltickets.Add(newTicket);
            db.SaveChanges();
            return RedirectToAction("Index");

            //else
            //{
            //    return View(newAccount);
            //}
        }

        //formget
        public ActionResult Edit(int? ID)

        {
            if (ID == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }

            tblticket editTicket = db.tbltickets.Find(ID);
            if (editTicket == null)
            {
                return HttpNotFound();
            }
            return View(editTicket);
        }

        //POST EDIT ACCOUNT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketId,TicketNumber,Problem,Details,Status,Date,Time,CreatedBy,AssignedTo,ApprovedBy")] tblticket editTicket, int? id)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgEdit"] = "Ticket Successfully Updated";
                db.Entry(editTicket).State = System.Data.Entity.EntityState.Added;

                tblticket acc = db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault();
                db.tbltickets.Remove(acc);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editTicket);
        }

        //Get: /customer/delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }
            using (CS405Entities2 db = new CS405Entities2())
            {
                return View(db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault());
            }
        }

        //POST: /USER/DELETE/
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                //TODO: // CODE HERE
                using (CS405Entities2 db = new CS405Entities2())
                {
                    tblticket acc = db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault();
                    TempData["MsgDelete"] = "Ticket Successfully Deleted";
                    db.tbltickets.Remove(acc);
                    db.SaveChanges();
                }
            }
            catch
            {
            }
            return RedirectToAction("Index");
        }

        //GET: /Accounts/
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }

            tblticket ticketN = new tblticket();
            ticketN = db.tbltickets.Find(Id);
            return PartialView("_Details", ticketN);
        }

        [HttpGet]
        public ActionResult Assigned(int? Id)
        {
            CS405Entities2 db = new CS405Entities2();

            if (Id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }

            //ViewBag.tblaccounts = new SelectList(db.tblaccounts, "id", "username");
            ViewBag.tblaccounts = new SelectList(db.tblaccounts.Where(k => k.username == "Technical"));

            var item = from d in db.tblaccounts.Where(k => k.usertype.Contains("Technical")) orderby d.username select d.username;
            item.ToList();

            if (item != null)
            {
                ViewBag.data = item;
            }

            tblticket ticketN = new tblticket();
            ticketN = db.tbltickets.Find(Id);
            return PartialView("_Assigned", ticketN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assigned([Bind(Include = "TicketId,TicketNumber,Problem,Details,Status,Date,Time,CreatedBy,AssignedTo,ApprovedBy")] tblticket editTicket, int id)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgAssign"] = "Ticket Assigned";
                db.Entry(editTicket).State = System.Data.Entity.EntityState.Added;

                tblticket acc = db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault();
                db.tbltickets.Remove(acc);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editTicket);
        }

        [HttpGet]
        public ActionResult Approved(int? Id)
        {
            if (Id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }

            tblticket ticketN = new tblticket();
            ticketN = db.tbltickets.Find(Id);
            return PartialView("_Approved", ticketN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approved([Bind(Include = "TicketId,TicketNumber,Problem,Details,Status,Date,Time,CreatedBy,AssignedTo,ApprovedBy")] tblticket editTicket, int id)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgAssign"] = "Ticket Assigned";
                db.Entry(editTicket).State = System.Data.Entity.EntityState.Added;

                tblticket acc = db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault();
                db.tbltickets.Remove(acc);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editTicket);
        }

        [HttpGet]
        public ActionResult Complete(int? Id)
        {
            if (Id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                return RedirectToAction("Error/PageNotFound");
            }

            tblticket ticketN = new tblticket();
            ticketN = db.tbltickets.Find(Id);
            return PartialView("_Complete", ticketN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete([Bind(Include = "TicketId,TicketNumber,Problem,Details,Status,Date,Time,CreatedBy,AssignedTo,ApprovedBy")] tblticket editTicket, int id)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgAssign"] = "Ticket Saved";
                db.Entry(editTicket).State = System.Data.Entity.EntityState.Added;

                tblticket acc = db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault();
                db.tbltickets.Remove(acc);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editTicket);
        }
    }
}