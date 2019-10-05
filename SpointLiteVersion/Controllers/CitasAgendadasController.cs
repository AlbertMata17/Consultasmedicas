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
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var citasAgendadas = db.CitasAgendadas.Include(c => c.paciente);
            return View(citasAgendadas.Where(m=>m.Usuarioid==usuarioid1 && m.Estatus==1).ToList());
        }
        public ActionResult GuardarCita(string fecha, string idpaciente1, string MotivoCita, string idCita)
        {
            string mensaje = "";
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            CitasAgendadas citas = new CitasAgendadas();
            if (idCita != "" && idCita != "undefined" && idCita != null)
            {
                citas.idCita = Convert.ToInt32(idCita);
                if (fecha != "undefined")
                {
                    citas.fecha = Convert.ToDateTime(fecha);
                }
                if (idpaciente1 != "undefined")
                {
                    citas.idpaciente = Convert.ToInt32(idpaciente1);
                }
                if (MotivoCita != "undefined")
                {
                    citas.MotivoCita = MotivoCita.ToUpper();
                }
                citas.Empresaid = empresaid1;
                citas.Usuarioid = usuarioid1;
                citas.Estatus = 1;
                db.Entry(citas).State = EntityState.Modified;
                db.SaveChanges();
                mensaje = "CITA GUARDADA CON EXITO...";
            }
            else
            {
                if (fecha != "undefined")
                {
                    citas.fecha = Convert.ToDateTime(fecha);
                }
                if (idpaciente1 != "undefined")
                {
                    citas.idpaciente = Convert.ToInt32(idpaciente1);
                }
                if (MotivoCita != "undefined")
                {
                    citas.MotivoCita = MotivoCita.ToUpper();
                }
                citas.Empresaid = empresaid1;
                citas.Usuarioid = usuarioid1;
                citas.Estatus = 1;
                db.CitasAgendadas.Add(citas);
                db.SaveChanges();

                mensaje = "CITA GUARDADA CON EXITO...";
            }
            
           
            
            return Json(mensaje);
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
        public ActionResult Create(int? id,int? idconsulta,int? pacientes)
        {
            
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            ViewBag.idpacientemost = idconsulta.ToString();
            System.Diagnostics.Debug.Write("El id de paciente es:"+pacientes);
            System.Diagnostics.Debug.Write("El id de consulta es:"+idconsulta);
            System.Diagnostics.Debug.Write("El id de es:"+id);
            if (id == null)
            {

                ViewBag.idpaciente1 = new SelectList(db.paciente.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1), "idPaciente", "nombre");
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
                TempData["idconsulta"] = idconsulta;
                ViewBag.idpacientemost = idconsulta.ToString();
                return View();

            }
            CitasAgendadas citas = db.CitasAgendadas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.idpaciente1 = new SelectList(db.paciente.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1), "idPaciente", "nombre",citas.idpaciente);
                ViewBag.id = "algo";
                ViewBag.idpacientemost = idconsulta.ToString();
                ViewBag.ejemplo = (from s in db.CitasAgendadas where s.Estatus==1 && s.Usuarioid==usuarioid1 select s.idpaciente).FirstOrDefault();

                return View(citas);
            }
            ViewBag.idpaciente1 = new SelectList(db.paciente.Where(m => m.Estatus == 1 && m.Usuarioid == usuarioid1), "idPaciente", "nombre");

            return View(citas);
        }

        // POST: CitasAgendadas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCita,fecha,idpaciente,MotivoCita")] CitasAgendadas citasAgendadas)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
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
                    citasAgendadas.Empresaid = empresaid1;
                    citasAgendadas.Usuarioid = usuarioid1;
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
                    citasAgendadas.Empresaid = empresaid1;
                    citasAgendadas.Usuarioid = usuarioid1;
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
            citasAgendadas.Estatus = 0;
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
