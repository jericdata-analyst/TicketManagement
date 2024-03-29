﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        private CS405Entities2 db = new CS405Entities2();

        public ActionResult Index(string txtsearch)
        {
            //create a variable for all the list users;
            var uTypeList = new List<string>();
            //create a query on selecting all the users with usertyoe
            var uTypequery = from d in db.tblaccounts orderby d.usertype select d.usertype;

            //add the result of the query on the variable list
            uTypeList.AddRange(uTypequery.Distinct());
            //create the view bag of the resulet
            ViewBag.username = new SelectList(uTypeList);
            //select all the users
            var accts = from m in db.tblaccounts select m;

            //search all user
            if (!String.IsNullOrEmpty(txtsearch))
            {
                accts = accts.Where(s => s.username.Contains(txtsearch));
            }

            //SWEETALERT

            //return all the user
            return View(accts.ToList());
        }

        //form get
        public ActionResult Create()
        {
            return View();
        }

        //form post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblaccount newAccount)
        {
            if (!ModelState.IsValid)
                return View(newAccount);
            if (db.tblaccounts.Any(k => k.username == newAccount.username))
            {
                ModelState.AddModelError("username", "Username already exist");
                return View(newAccount);
            }
            TempData["Msg"] = "Account Successfully Added";
            db.tblaccounts.Add(newAccount);
            db.SaveChanges();
            return RedirectToAction("Index");

            //else
            //{
            //    return View(newAccount);
            //}
        }

        //form get
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblaccount editAccount = db.tblaccounts.Find(ID);
            if (editAccount == null)
            {
                return HttpNotFound();
            }
            return View(editAccount);
        }

        //POST EDIT ACCOUNT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LastName,FirstName,username,MiddleName,password,department,branch,usertype,status")] tblaccount editAccount)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgEdit"] = "Account Successfully Updated";
                db.Entry(editAccount).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editAccount);
        }

        //Get: /customer/delete
        public ActionResult Delete(int id)
        {
            using (CS405Entities2 db = new CS405Entities2())
            {
                return View(db.tblaccounts.Where(x => x.id == id).FirstOrDefault());
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
                    tblaccount acc = db.tblaccounts.Where(x => x.id == id).FirstOrDefault();
                    TempData["MsgDelete"] = "Account Successfully Deleted";
                    db.tblaccounts.Remove(acc);
                    db.SaveChanges();
                }
            }
            catch
            {
            }
            return RedirectToAction("Index");
        }

        //GET: /Accounts/
        public ActionResult Details(int id)
        {
            using (CS405Entities2 db = new CS405Entities2())
            {
                return View(db.tblaccounts.Where(x => x.id == id).FirstOrDefault());
            }
        }
    }
}