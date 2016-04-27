using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace cis237Assignment6.Models
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageCCunninghamEntities db = new BeverageCCunninghamEntities();

        // GET: /Beverages/
        public ActionResult Index()
        {
            // >Setup a variable to hold the beverage data.
            DbSet<Beverage> beveragesToFilter = db.Beverages;

            // >Setup some default strings to hold the session data if there is any.
            string filterName = string.Empty;
            string filterMinPrice = string.Empty;
            string filterMaxPrice = string.Empty;

            // >These hold the parsed strings.
            decimal minPriceDec = 0.00m;
            decimal maxPriceDec = 2000.00m;

            // >Check to see if there is are values in the session.
            if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }
            if (Session["minPrice"] != null && !String.IsNullOrWhiteSpace((string)Session["minPrice"]))
            {
                filterMinPrice = (string)Session["minPrice"];
                minPriceDec = decimal.Parse(filterMinPrice);
            }
            if (Session["maxPrice"] != null && !String.IsNullOrWhiteSpace((string)Session["maxPrice"]))
            {
                filterMaxPrice = (string)Session["maxPrice"];
                maxPriceDec = decimal.Parse(filterMaxPrice);
            }

            // >Filter the database.
            IEnumerable<Beverage> filtered = beveragesToFilter.Where(bev => bev.price >= minPriceDec &&
                                                                  bev.price <= maxPriceDec &&
                                                                  bev.name.Contains(filterName));

            // >Convert the database set to a list now that the query work is done on it.
            IEnumerable<Beverage> finalFiltered = filtered.ToList();

            // >Place the values that are in the session into the viewbag.
            ViewBag.filterName = filterName;
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

            // >Return the view with a filtered selection.
            return View(finalFiltered);
        }

        // GET: /Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: /Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: /Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: /Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: /Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: /Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        [HttpPost, ActionName("Filter")]
        public ActionResult Filter()
        {
            // >Get the data from the textboxes on the form.
            string name = Request.Form.Get("name");
            string minPrice = Request.Form.Get("minPrice");
            string maxPrice = Request.Form.Get("maxPrice");

            // >Store the data into the session.
            Session["name"] = name;
            Session["minPrice"] = minPrice;
            Session["maxPrice"] = maxPrice;

            // >Redirect the user to the index page.
            return RedirectToAction("Index");
        }










    }
}
