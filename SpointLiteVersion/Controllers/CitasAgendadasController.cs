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
    public class CitasAgendadasController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: CitasAgendadas
        public ActionResult Index()

        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var citasAgendadas = db.CitasAgendadas.Include(c => c.paciente);
            return View(citasAgendadas.ToList());
        }

        // GET: CitasAgendadas/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
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

        // GET: CitasAgendadas/Create
        public ActionResult Create(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            if (id == null)
            {

                ViewBag.idpaciente1 = new SelectList(db.paciente, "idPaciente", "nombre");
                var s = (from datosid in db.CitasAgendadas select datosid.idCita).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.CitasAgendadas select datosid.idCita).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idpaciente = idmostrar.ToString("D2");
                return View();

            }
            CitasAgendadas citas = db.CitasAgendadas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.idpaciente1 = new SelectList(db.paciente, "idPaciente", "nombre",citas.idCita);
                ViewBag.id = "algo";
             
                return View(citas);
            }
            return View();
        }

        // POST: CitasAgendadas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCita,fecha,idpaciente,MotivoCita")] CitasAgendadas citasAgendadas)
        {
            var t = (from s in db.CitasAgendadas where s.idCita == citasAgendadas.idCita select s.idCita).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (citasAgendadas.MotivoCita != "")
                    {
                        citasAgendadas.MotivoCita = citasAgendadas.MotivoCita.ToUpper();
                    }
                    citasAgendadas.Estatus = 1;
                    db.Entry(citasAgendadas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            else if (citasAgendadas.idCita <= 0)
            {

                if (ModelState.IsValid)
                {
                    if (citasAgendadas.MotivoCita != "")
                    {
                        citasAgendadas.MotivoCita = citasAgendadas.MotivoCita.ToUpper();
                    }
                    citasAgendadas.Estatus = 1;
                    db.CitasAgendadas.Add(citasAgendadas);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitasAgendadas citasAgendadas = db.CitasAgendadas.Find(id);
            if (citasAgendadas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            return View(citasAgendadas);
        }

        // POST: CitasAgendadas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCita,fecha,idpaciente,MotivoCita")] CitasAgendadas citasAgendadas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citasAgendadas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", citasAgendadas.idpaciente);
            return View(citasAgendadas);
        }

        // GET: CitasAgendadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
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

        // POST: CitasAgendadas/Delete/5
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
