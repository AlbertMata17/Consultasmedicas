using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;

namespace SpointLiteVersion.Controllers
{
    public class ciudadsController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: ciudads
        public ActionResult Index()
        {
            return View(db.ciudad.ToList());
        }

        // GET: ciudads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // GET: ciudads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ciudads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idciudad,Nombre")] ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                ciudad.Estatus = 1;
                if (ciudad.Nombre != null)
                {
                    ciudad.Nombre = ciudad.Nombre.ToUpper();
                }
                db.ciudad.Add(ciudad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ciudad);
        }

        // GET: ciudads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: ciudads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idciudad,Nombre")] ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ciudad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ciudad);
        }

        // GET: ciudads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: ciudads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ciudad ciudad = db.ciudad.Find(id);
            db.ciudad.Remove(ciudad);
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
