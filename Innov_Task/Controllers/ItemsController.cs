using Innov_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Innov_Task.Controllers
{
    /// <summary>
    ///  This Controller made to handel the Items Data like View their data , add a new one , edit and delete them
    /// </summary>
    

    public class ItemsController : Controller
    {
        Innovation_TaskEntities db = new Innovation_TaskEntities();

        // GET: Items
        public ActionResult Index()
        {
           //var items= db.Items.Where(a => a.quantity != 0).ToList();
            var items = db.Items.Where(a => a.quantity != 0).ToList();
            return View(items);
        }

        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }

        // Add a New Item
        [HttpPost]
        public ActionResult Add([Bind(Exclude = "Code")]Item itm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Items.Add(itm);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


                return View("Index");
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public ActionResult Edit(int Code)
        {
            try
            {

                Item Itm = db.Items.FirstOrDefault(a => a.Code == Code);

                if (Itm == null)
                {
                    return HttpNotFound("Item Doesn't Exist");

                }

                return View(Itm);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Edit the Item
        [HttpPost]
        public ActionResult Edit(Item newItm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Item OldItm = db.Items.FirstOrDefault(a => a.Code == newItm.Code);
                    OldItm.name = newItm.name;
                    OldItm.quantity = newItm.quantity;
                    OldItm.Price = newItm.Price;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Items");
                }

                Item Itm = db.Items.FirstOrDefault(a => a.Code == newItm.Code);

                if (Itm == null)
                {
                    return HttpNotFound("Item Doesn't Exist");
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        [HttpGet]
        public ActionResult Delete(int Code)
        {
            try
            {
                Item itm = db.Items.Find(Code);

                ViewBag.name = db.Items.Find(Code).name;

                return View(itm);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Delete the Item
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                var id = int.Parse(TempData["ItemId"].ToString());

                var itm = db.item_Contract.Where(a => a.Code == id).ToList();
                if(itm!=null)
                {
                    db.Items.FirstOrDefault(a => a.Code == id).quantity = 0;
                    db.SaveChanges();
                }
                else
                {
                     Item Delitm = db.Items.FirstOrDefault(a => a.Code == id);
                     db.Items.Remove(Delitm);
                     db.SaveChanges();
                }
               
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // To Save TempData
        public ActionResult myendpoint(int dataToSave)
        {
            try
            {
                if (string.IsNullOrEmpty(dataToSave.ToString()))
                {
                    return Json(new { message = "Empty data to save" }, JsonRequestBehavior.AllowGet);
                }
                TempData["ItemId"] = dataToSave;
                TempData.Keep();
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }


    }
}