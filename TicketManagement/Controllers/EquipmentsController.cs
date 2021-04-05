using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicketManagement.Models;

namespace TicketManagement.Controllers
{
    public class EquipmentsController : Controller
    {
        // GET: Equipments
        private CS405Entities1 db = new CS405Entities1();

        public ActionResult Index(string txtsearch)
        {
            //create a variable for all the list users;
            var uTypeList = new List<string>();
            //create a query on selecting all the users with usertyoe
            var uTypequery = from d in db.tblequipments orderby d.AssetNumber select d.AssetNumber;

            //add the result of the query on the variable list
            uTypeList.AddRange(uTypequery.Distinct());
            //create the view bag of the resulet
            ViewBag.SerialNumber = new SelectList(uTypeList);
            //select all the users
            var eqpmnt = from m in db.tblequipments select m;

            //search all user
            if (!String.IsNullOrEmpty(txtsearch))
            {
                eqpmnt = eqpmnt.Where(s => s.AssetNumber.Contains(txtsearch));
            }

            //SWEETALERT

            //return all the user
            return View(eqpmnt.ToList());
        }

        //form get
        public ActionResult Create()
        {
            return View();
        }

        //form post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblequipment newEquipment)
        {
            if (!ModelState.IsValid)
                return View(newEquipment);
            if (db.tblequipments.Any(k => k.AssetNumber == newEquipment.AssetNumber))
            {
                ModelState.AddModelError("username", "Username already exist");
                return View(newEquipment);
            }
            TempData["Msg"] = "Account Successfully Added";
            db.tblequipments.Add(newEquipment);
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
            tblequipment editEquipment = db.tblequipments.Find(ID);
            if (editEquipment == null)
            {
                return HttpNotFound();
            }
            return View(editEquipment);
        }

        //POST EDIT ACCOUNT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetNumber,SerialNumber,Type,Manufacturer,YearModel,Description,Branch,Department,Status")] tblequipment editEquipment)
        {
            if (ModelState.IsValid)
            {
                TempData["MsgEdit"] = "Account Successfully Updated";
                db.Entry(editEquipment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editEquipment);
        }

        //Get: /customer/delete
        public ActionResult Delete(int id)
        {
            using (CS405Entities1 db = new CS405Entities1())
            {
                return View(db.tblequipments.Where(x => x.equipmentsId == id).FirstOrDefault());
            }
        }

        //POST: /USER/DELETE/
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                //TODO: // CODE HERE
                using (CS405Entities1 db = new CS405Entities1())
                {
                    tblequipment item = db.tblequipments.Where(x => x.equipmentsId == id).FirstOrDefault();
                    TempData["MsgDelete"] = "Account Successfully Deleted";
                    db.tblequipments.Remove(item);
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
            using (CS405Entities1 db = new CS405Entities1())
            {
                return View(db.tblequipments.Where(x => x.equipmentsId == id).FirstOrDefault());
            }
        }
    }
}