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
    public class Logins1Controller : Controller
    {
        private hospointEntities db = new hospointEntities();

        // GET: Logins1
        public ActionResult Index()
        {
            var login = db.HosLogin.Include(l => l.HosEmpresa);
            return View(login.ToList());
        }

        // GET: Logins1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosLogin login = db.HosLogin.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: Logins1/Create
        public ActionResult Create()
        {
            ViewBag.empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre");
            return View();
        }

        // POST: Logins1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginId,Username,Password,Privilegio,Nombre,Apellido,empresaid")] HosLogin login)
        {
            if (ModelState.IsValid)
            {
                db.HosLogin.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", login.empresaid);
            return View(login);
        }

        // GET: Logins1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosLogin login = db.HosLogin.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            ViewBag.empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", login.empresaid);
            return View(login);
        }

        // POST: Logins1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginId,Username,Password,Privilegio,Nombre,Apellido,empresaid")] HosLogin login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", login.empresaid);
            return View(login);
        }

        // GET: Logins1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosLogin login = db.HosLogin.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HosLogin login = db.HosLogin.Find(id);
            db.HosLogin.Remove(login);
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
