using Microsoft.Ajax.Utilities;
using spLettings.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace spLettings.Controllers
{
    public class HomeController : Controller
    {
        PropertiesDBEntities2 _db = new PropertiesDBEntities2();

        // GET: Home
        public ActionResult Index(String Bedrooms, String City, String MaxPrice)
        {



            ViewBag.Bedrooms = new SelectList(_db.Properties.DistinctBy(c => c.Bedrooms), "Bedrooms", "Bedrooms");    // preselect item in selectlist by CompanyID param


            ViewBag.City = new SelectList(_db.Properties.DistinctBy(c => c.City), "City", "City");




            var query = _db.Properties.AsQueryable();


           if(Bedrooms!= null && int.TryParse(Bedrooms, out int bdr))
            {
                query = query.Where(x => x.Bedrooms == bdr);
                //return View(_db.Properties.Where(x => x.Bedrooms == bdr).Take(50));
            }

            if (City != null && City !="")
            {
                query = query.Where(x => x.City == City);
                //return View(_db.Properties.Where(x => x.Bedrooms == bdr).Take(50));
            }


            if (MaxPrice != null && int.TryParse(MaxPrice, out int max))
            {
                query = query.Where(x => x.Price <= max);
                //return View(_db.Properties.Where(x => x.Bedrooms == bdr).Take(50));
            }


            string markers = "[";
           
            foreach (Property p in query)
            {
                markers += "{";
                markers += string.Format("'id': '{0}',",p.Id);
                markers += string.Format("'title': '{0}',", p.Address);
                markers += string.Format("'lat': '{0}',", p.Lat);
                markers += string.Format("'lng': '{0}',", p.Long);
                markers += string.Format("'description': '{0}'", p.Address);
                markers += "},";
            }


            markers += "];";
            ViewBag.Markers = markers;

            return View(query.ToList());




        }

        // GET: Home/GetMapMarker
        public JsonResult GetMapMarker()
        {
            var ListOfAddress = _db.Properties.ToList();

            return Json(ListOfAddress, JsonRequestBehavior.AllowGet);
        }


        // GET: Home/Details/5
        public ActionResult Details(int Id)
        {
            var property = (from p in _db.Properties
                            where p.Id == Id
                            select p).First();


            string markers = "[";
                markers += "{";
                markers += string.Format("'id': '{0}',", property.Id);
                markers += string.Format("'title': '{0}',", property.Address);
                markers += string.Format("'lat': '{0}',", property.Lat);
                markers += string.Format("'lng': '{0}',", property.Long);
                markers += string.Format("'description': '{0}'", property.Address);
                markers += "},";
            markers += "];";
            ViewBag.Markers = markers;

            return View(property);
        }


        // POST: Home/Inquire/5
        [Authorize]
        public ActionResult Inquire(int Id)
        {
            var property = (from p in _db.Properties
                            where p.Id == Id
                            select p).First();
            string user = User.Identity.Name;
            Interest interest = new Interest();
            interest.User = user;
            interest.PropertyId = Id;
            Console.WriteLine(interest.Id);
            _db.Interests.Add(interest);
            _db.SaveChanges();


            string markers = "[";
            markers += "{";
            markers += string.Format("'id': '{0}',", property.Id);
            markers += string.Format("'title': '{0}',", property.Address);
            markers += string.Format("'lat': '{0}',", property.Lat);
            markers += string.Format("'lng': '{0}',", property.Long);
            markers += string.Format("'description': '{0}'", property.Address);
            markers += "},";
            markers += "];";
            ViewBag.Markers = markers;

            return View(property);
        }


        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
