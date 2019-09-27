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
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: CitasAgendadas1
        public ActionResult Index()
        {
            var citasAgendadas = db.CitasAgendadas.Include(c => c.Empresa).Include(c => c.paciente).Include(c => c.Login);
            return View(citasAgendadas.ToList());
        }

        // GET: CitasAgendadas1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitasAgendadas citasAgendadas = db.CitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Create
        public ActionResult Create()
        {
            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre");
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre");
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username");
            return View();
        }

        // POST: CitasAgendadas1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCita,fecha,idpaciente,MotivoCita,Estatus,Empresaid,Usuarioid")] CitasAgendadas citasAgendadas)
        {
            if (ModelState.IsValid)
            {
                db.CitasAgendadas.Add(citasAgendadas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitasAgendadas citasAgendadas = db.CitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // POST: CitasAgendadas1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCita,fecha,idpaciente,MotivoCita,Estatus,Empresaid,Usuarioid")] CitasAgendadas citasAgendadas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citasAgendadas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", citasAgendadas.Empresaid);
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", citasAgendadas.Usuarioid);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitasAgendadas citasAgendadas = db.CitasAgendadas.Find(id);
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
            CitasAgendadas citasAgendadas = db.CitasAgendadas.Find(id);
            db.CitasAgendadas.Remove(citasAgendadas);
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
