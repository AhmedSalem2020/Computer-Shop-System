using Innov_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Innov_Task.Controllers
{
    /// <summary>
    ///  This Controller made to handel the Customers Data like View their data , add a new one , edit and delete them..
    ///  
    ///  It's Also contain operations for itemContract like add item to contract , delete item from the contract , edit quantity 
    ///  and finally save the contract 
    /// </summary>
    
    public class CustomersController : Controller
    {
        
        Innovation_TaskEntities db = new Innovation_TaskEntities();
       
        // GET: Customers
        public ActionResult Index()
        {
            //TempData["id"] =1;
            var cust = db.Customers.ToList();
            return View(cust);
        }


        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }

        // Add a New Customer
        [HttpPost]
        public ActionResult Add([Bind(Exclude = "Id")]Customer cust)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Customers.Add(cust);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //ViewBag.btnTitle = "Create";

                return View(cust);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage","Home");
            }
            
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {


                Customer Cust = db.Customers.FirstOrDefault(a => a.Id == id);

                if (Cust == null)
                {
                    return HttpNotFound("Customer Doesn't Exist");

                }

                return View(Cust);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Edit the Customer
        [HttpPost]
        public ActionResult Edit(Customer newCust)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Customer OldCust = db.Customers.FirstOrDefault(a => a.Id == newCust.Id);

                    OldCust.Name = newCust.Name;
                    OldCust.Address = newCust.Address;
                    OldCust.Email = newCust.Email;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Customers");
                    //return View("Index");
                    //return Json(new { success = true, message = "Customer data Updated Successfuly" }, JsonRequestBehavior.AllowGet);
                }

                Customer cust = db.Customers.FirstOrDefault(a => a.Id == newCust.Id);

                if (cust == null)
                {
                    return HttpNotFound("Customer Doesn't Exist");
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
        public ActionResult Delete(int Id)
        {
            try
            {
                Customer Cust = db.Customers.Find(Id);

                ViewBag.name = db.Customers.Find(Id).Name;

                return View(Cust);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Delete the Selected Customer
        [HttpPost]
        public ActionResult Delete(/*[Bind(Include = "Id")] Customer Cust*/)
        {
            try
            {
                var id = int.Parse(TempData["customerId"].ToString());
                Customer DelCust = db.Customers.FirstOrDefault(a => a.Id == id);
                db.Customers.Remove(DelCust);
                db.SaveChanges();
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
                TempData["customerId"] = dataToSave;
                TempData.Keep();

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Show the Contracts related to specific Customer
        [HttpGet]
        public ActionResult viewContracts(int Cust_Id)
        {

            var conts = db.Contracts.Where(a => a.Cust_Id == Cust_Id).ToList();
            return View(conts);
        }

        [HttpGet]
        public ActionResult DeleteContract(int id)
        {
            try
            {
                Contract cont = db.Contracts.FirstOrDefault(a => a.Cont_No == id);

                //Contract Cont = db.Contracts.Find(id);

                return View(cont);
                //return View(Cont);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
           
        }

        // Delete a specific Contract
        [HttpPost]
        public ActionResult DeleteContract(Contract cont)
        {
            try
            {
                var itm = db.item_Contract.Where(a => a.Cont_No == cont.Cont_No).ToList();
                if(itm!=null)
                {
                    foreach(var item in itm)
                    {
                        db.item_Contract.Remove(item);
                        db.SaveChanges();
                    }
                }
                Contract DelCon = db.Contracts.FirstOrDefault(a => a.Cont_No == cont.Cont_No);

                db.Contracts.Remove(DelCon);

                db.SaveChanges();

                return RedirectToAction("viewContracts", "Customers", new { Cust_Id = cont.Cust_Id });
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Show the Items related to Specific Contract 
        public ActionResult viewCont(int Cont_No , int Cust_Id)
        {
            try
            {
                var cont = db.item_Contract.Where(a => a.Cont_No == Cont_No).ToList();
                ViewBag.resetId = Cust_Id;
                ViewBag.resetDate = db.Contracts.FirstOrDefault(a => a.Cont_No==Cont_No).Date;
                TempData["Cont_No"] = Cont_No;
                TempData["Cust_Id"] = Cust_Id;
                TempData.Keep();

                return View(cont);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }


        [HttpGet]
        public ActionResult DeleteItemContract(int Cont_No, int Code,int Cust_Id)
        {
            //ViewBag.Cust_Id = Cust_Id;
            TempData["Cust_Id"] = Cust_Id;
            return View();
        }

        // Delete an item from specific Contract
        [HttpPost]
        public ActionResult DeleteItemContract(item_Contract itmCont)
        {
            try
            {
                item_Contract DelItmCont = db.item_Contract.FirstOrDefault(a => a.Code == itmCont.Code && a.Cont_No==itmCont.Cont_No);

                    db.item_Contract.Remove(DelItmCont);

                    db.SaveChanges();

                    int Cust_Id = int.Parse(TempData["Cust_Id"].ToString());

                    return RedirectToAction("viewCont", "Customers", new { Cont_No = itmCont.Cont_No, Cust_Id });

            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }

        }

        [HttpGet]
        public ActionResult AddItemContract(int Cont_No , int Cust_Id)
        {
            try
            {
                TempData["Cust_Id"] = Cust_Id;
                List<Item> itms = db.Items.Where(a=>a.quantity!=0).ToList<Item>();
                SelectList itemsl = new SelectList(itms, "Code", "name");
                ViewBag.Code = itemsl;

                return View();
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Add an item to specific Contract
        [HttpPost]
        public ActionResult AddItemContract(item_Contract itmCont)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var ff = db.item_Contract.FirstOrDefault(a => a.Cont_No == itmCont.Cont_No && a.Code == itmCont.Code);
                    if (ff != null)
                    {
                        ff.Quantity += 1;
                        db.SaveChanges();
                        int Cust_Id = int.Parse(TempData["Cust_Id"].ToString());
                        return RedirectToAction("viewCont", "Customers", new { Cont_No = itmCont.Cont_No, Cust_Id });
                    }

                    else if (ff == null)
                    {
                        var gg = db.item_Contract.FirstOrDefault(a => a.Cont_No == itmCont.Cont_No);
                        itmCont.Quantity = 1;
                        db.item_Contract.Add(itmCont);
                        db.SaveChanges();
                        int Cust_Id = int.Parse(TempData["Cust_Id"].ToString());
                        return RedirectToAction("viewCont", "Customers", new { Cont_No = itmCont.Cont_No, Cust_Id });

                    }
                }

                List<Item> itms = db.Items.ToList<Item>();
                SelectList itemsl = new SelectList(itms, "Code", "name");
                ViewBag.Code = itemsl;
                return View(itmCont);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }
        }

        // Save the contract changes 
        [HttpPost]
        public ActionResult SaveContract(Dictionary<int,int>itcount,int contNo)
        {
            try
            {
                int Cust_Id = int.Parse(TempData["Cust_Id"].ToString());
                int Cont_No = int.Parse(TempData["Cont_No"].ToString());
                if (ModelState.IsValid)
                {
                    foreach (KeyValuePair<int, int> item in itcount)
                    {
                        var cont = db.item_Contract.SingleOrDefault(c => c.Cont_No == contNo && c.Item.Code == item.Key);
                        cont.Quantity = item.Value;
                        db.SaveChanges();

                    }
                    return RedirectToAction("viewCont", "Customers", new { Cont_No, Cust_Id });
                }
                return View(itcount);
            }
            catch (Exception ex)
            {
                LogError.Error(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return RedirectToAction("ErrorPage", "Home");
            }

        }

    }
}