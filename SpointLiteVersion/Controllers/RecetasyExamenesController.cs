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
    public class RecetasyExamenesController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: RecetasyExamenes
        public ActionResult Index()
        {
            var recetasyExamenes = db.RecetasyExamenes.Include(r => r.Empresa).Include(r => r.Login).Include(r => r.paciente);
            return View(recetasyExamenes.ToList());
        }

        // GET: RecetasyExamenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetasyExamenes recetasyExamenes = db.RecetasyExamenes.Find(id);
            if (recetasyExamenes == null)
            {
                return HttpNotFound();
            }
            return View(recetasyExamenes);
        }

        // GET: RecetasyExamenes/Create
        public ActionResult Create(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            if (id == null)
            {

                ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre");
                ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username");
                ViewBag.idPaciente = new SelectList(db.paciente.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1), "idPaciente", "nombre");
                var s = (from datosid in db.RecetasyExamenes select datosid.id).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.RecetasyExamenes select datosid.id).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idreceta = idmostrar.ToString("D2");

                return View();

            }
            RecetasyExamenes citas = db.RecetasyExamenes.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre");
                ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username");
                ViewBag.idPaciente = new SelectList(db.paciente.Where(m => m.Estatus == 1 && m.Usuarioid == usuarioid1), "idPaciente", "nombre",citas.idPaciente);
                ViewBag.tipo = (from s in db.RecetasyExamenes where s.id==id select s.Tipo).FirstOrDefault();
                ViewBag.Detalle = (from s in db.RecetasyExamenes where s.id == id select s.Detalle).FirstOrDefault();
                
                ViewBag.id = "algo";

                return View(citas);
            }
           
            return View();
        }

        // POST: RecetasyExamenes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Tipo,Detalle,idPaciente,Estatus,Empresaid,Usuarioid,fecha")] RecetasyExamenes recetasyExamenes)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var t = (from s in db.RecetasyExamenes where s.id == recetasyExamenes.id select s.id).Count();

            if (ModelState.IsValid)
            {
                if (t != 0)
                {
                    if (recetasyExamenes.Tipo != null)
                    {
                        recetasyExamenes.Tipo = recetasyExamenes.Tipo.ToUpper();
                    }
                    if (recetasyExamenes.Detalle != null)
                    {
                        recetasyExamenes.Detalle = recetasyExamenes.Detalle.ToUpper();
                    }
                    recetasyExamenes.Estatus = 1;
                    db.Entry(recetasyExamenes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }


            }
            else if (recetasyExamenes.id <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (recetasyExamenes.Tipo != null)
                    {
                        recetasyExamenes.Tipo = recetasyExamenes.Tipo.ToUpper();
                    }
                    if (recetasyExamenes.Detalle != null)
                    {
                        recetasyExamenes.Detalle = recetasyExamenes.Detalle.ToUpper();
                    }
                    recetasyExamenes.Estatus = 1;
                    db.RecetasyExamenes.Add(recetasyExamenes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.paciente, "idPaciente", "nombre", recetasyExamenes.idPaciente);
            return View(recetasyExamenes);
        }

            // GET: RecetasyExamenes/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetasyExamenes recetasyExamenes = db.RecetasyExamenes.Find(id);
            if (recetasyExamenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.paciente, "idPaciente", "nombre", recetasyExamenes.idPaciente);
            return View(recetasyExamenes);
        }

        // POST: RecetasyExamenes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Tipo,Detalle,idPaciente,Estatus,Empresaid,Usuarioid,fecha")] RecetasyExamenes recetasyExamenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recetasyExamenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.Login, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.paciente, "idPaciente", "nombre", recetasyExamenes.idPaciente);
            return View(recetasyExamenes);
        }

        // GET: RecetasyExamenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecetasyExamenes recetasyExamenes = db.RecetasyExamenes.Find(id);
            if (recetasyExamenes == null)
            {
                return HttpNotFound();
            }
            return View(recetasyExamenes);
        }

        // POST: RecetasyExamenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecetasyExamenes recetasyExamenes = db.RecetasyExamenes.Find(id);
            recetasyExamenes.Estatus = 0;
            db.SaveChanges();
            return RedirectToAction("RecetasyExamen","Consultas");
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
