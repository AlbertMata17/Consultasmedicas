﻿using System;
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
    public class vendedoresController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: vendedores
        public ActionResult Index()
        {
            return View(db.vendedores.ToList().Where(m=>m.Status=="1"));
        }

        // GET: vendedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // GET: vendedores/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();

            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.id = "algo";

                return View(vendedores);
            }

            return View();
        }

        // POST: vendedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idvendedor,nombre,direccion,telefono,cedula,cumpleaños,correo")] vendedores vendedores)
        {
            var t = (from s in db.vendedores where s.idvendedor == vendedores.idvendedor select s.idvendedor).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    vendedores.nombre = vendedores.nombre.ToUpper();
                    vendedores.correo = vendedores.correo.ToUpper();
                    vendedores.direccion = vendedores.direccion.ToUpper();
                    db.Entry(vendedores).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (vendedores.idvendedor <= 0)
            
                {
                    if (ModelState.IsValid)
                    {
                    vendedores.nombre = vendedores.nombre.ToUpper();
                    vendedores.correo = vendedores.correo.ToUpper();
                    vendedores.direccion = vendedores.direccion.ToUpper();
                    
                        db.vendedores.Add(vendedores);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            return View(vendedores);
        }

        // GET: vendedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // POST: vendedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idvendedor,nombre,direccion,telefono,cedula,cumpleaños,correo")] vendedores vendedores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendedores);
        }

        // GET: vendedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // POST: vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vendedores vendedores = db.vendedores.Find(id);
            vendedores.Status = "0";

            db.vendedores.Remove(vendedores);
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
