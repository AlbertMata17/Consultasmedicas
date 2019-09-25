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
    public class especialesController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: especiales
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            return View(db.especiales.ToList());
        }

        // GET: especiales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            especiales especiales = db.especiales.Find(id);
            if (especiales == null)
            {
                return HttpNotFound();
            }
            return View(especiales);
        }

        // GET: especiales/Create
        public ActionResult Create(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return View();

            }
            especiales especial = db.especiales.Find(id);
            if (especial == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.id = "algo";

                return View(especial);
            }
            return View();
        }

        // POST: especiales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Tipo")] especiales especiales)
        {
            var t = (from s in db.especiales where s.Id == especiales.Id select s.Id).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (especiales.Nombre != null)
                    {
                        especiales.Nombre = especiales.Nombre.ToUpper();
                    }
                    if (especiales.Tipo != null)
                    {
                        especiales.Tipo = especiales.Tipo.ToUpper();
                    }
                    especiales.estatus = 1;
                    db.Entry(especiales).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (especiales.Id <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (especiales.Nombre != null)
                    {
                        especiales.Nombre = especiales.Nombre.ToUpper();
                    }
                    if (especiales.Tipo != null)
                    {
                        especiales.Tipo = especiales.Tipo.ToUpper();
                    }
                    especiales.estatus = 1;
                    db.especiales.Add(especiales);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }


                return View(especiales);
        }

        // GET: especiales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            especiales especiales = db.especiales.Find(id);
            if (especiales == null)
            {
                return HttpNotFound();
            }
            return View(especiales);
        }

        // POST: especiales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Tipo")] especiales especiales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(especiales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(especiales);
        }

        // GET: especiales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            especiales especiales = db.especiales.Find(id);
            if (especiales == null)
            {
                return HttpNotFound();
            }
            return View(especiales);
        }

        // POST: especiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            especiales especiales = db.especiales.Find(id);
            db.especiales.Remove(especiales);
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
