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
    public class CitasAgendadas1Controller : Controller
    {
        private hospointEntities db = new hospointEntities();

        // GET: CitasAgendadas1
        public ActionResult Index()
        {
            var citasAgendadas = db.HosCitasAgendadas.Include(c => c.HosEmpresa).Include(c => c.Hospaciente).Include(c => c.HosLogin);
            return View(citasAgendadas.ToList());
        }

        // GET: CitasAgendadas1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           HosCitasAgendadas citasAgendadas = db.HosCitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Create
        public ActionResult Create()
        {
            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre");
            ViewBag.idpaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre");
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username");
            return View();
        }

        // POST: CitasAgendadas1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCita,fecha,idpaciente,MotivoCita,Estatus,Empresaid,Usuarioid")] HosCitasAgendadas citasAgendadas)
        {
            if (ModelState.IsValid)
            {
                db.HosCitasAgendadas.Add(citasAgendadas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosCitasAgendadas citasAgendadas = db.HosCitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // POST: CitasAgendadas1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCita,fecha,idpaciente,MotivoCita,Estatus,Empresaid,Usuarioid")] HosCitasAgendadas citasAgendadas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citasAgendadas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosCitasAgendadas citasAgendadas = db.HosCitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            return View(citasAgendadas);
        }

        // POST: CitasAgendadas1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HosCitasAgendadas citasAgendadas = db.HosCitasAgendadas.Find(id);
            db.HosCitasAgendadas.Remove(citasAgendadas);
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
