using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Search.Models;

namespace Search.Controllers
{
    public class CaseController : Controller
    {
        private CasesEntities db = new CasesEntities();

        // GET: /Case/
        public ActionResult Index(string caseProduct, string caseType, string caseCategory, string searchString)
        {
            var CategoryList = new List<string>();
            var CategoryQry = from a in db.Cases
                              orderby a.Category
                              select a.Category;

            CategoryList.AddRange(CategoryQry.Distinct());
            ViewBag.caseCategory = new SelectList(CategoryList);


            var TypeList = new List<string>();

            var TypeQry = from t in db.Cases
                          orderby t.CaseType
                          select t.CaseType;

            TypeList.AddRange(TypeQry.Distinct());
            ViewBag.caseType = new SelectList(TypeList);

            var ProductList = new List<string>();

            var ProductQry = from d in db.Cases
                             orderby d.Product
                             select d.Product;

            ProductList.AddRange(ProductQry.Distinct());
            ViewBag.caseProduct = new SelectList(ProductList);


            var cases = from c in db.Cases
                        select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cases = cases.Where(s => s.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(caseProduct))
            {
                cases = cases.Where(x => x.Product == caseProduct);
            }

            if (!string.IsNullOrEmpty(caseType))
            {
                cases = cases.Where(y => y.CaseType == caseType);
            }

            if (!string.IsNullOrEmpty(caseCategory))
            {
                cases = cases.Where(b => b.Category == caseCategory);
            }



            return View(cases);
        }

        // GET: /Case/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // GET: /Case/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Case/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CaseNumber,Product,CaseType,Category,Subject,Description,ResMessage")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Cases.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@case);
        }

        // GET: /Case/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: /Case/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CaseNumber,Product,CaseType,Category,Subject,Description,ResMessage")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@case);
        }

        // GET: /Case/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: /Case/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            db.Cases.Remove(@case);
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
    }
}
