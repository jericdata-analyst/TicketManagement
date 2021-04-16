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

        public ActionResult Index(string txtsearch)
        {
            ViewBag.type = 0;

            //create a variable for all the list users;
            var uTypeList = new List<string>();

            //create a query on selecting all the users with usertyoe
            var uTypequery = from d in db.tbltickets orderby d.TicketNumber select d.TicketNumber;

            //add the result of the query on the variable list
            uTypeList.AddRange(uTypequery.Distinct());

            //create the view bag of the resulet
            ViewBag.TicketNumber = new SelectList(uTypeList);

            //select all the users
            var accts = from m in db.tbltickets select m;

            //search all user
            if (!String.IsNullOrEmpty(txtsearch))
            {
                accts = accts.Where(s => s.Status.Contains(txtsearch));
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
        public ActionResult Edit([Bind(Include = "TicketId,TicketNumber,Problem,Details,Status,Date,Time,CreatedBy,AssignedTo,ApprovedBy")] tblticket editTicket, int id)
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
        public ActionResult Delete(int id)
        {
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
        public ActionResult Details(int Id)
        {
            //using (CS405Entities2 db = new CS405Entities2())
            //{
            //    return View(db.tbltickets.Where(x => x.TicketId == id).FirstOrDefault());

            //}

            tblticket ticketN = new tblticket();
            ticketN = db.tbltickets.Find(Id);
            return PartialView("_Details", ticketN);
        }
    }
}