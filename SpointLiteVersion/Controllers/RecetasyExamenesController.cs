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
        private hospointEntities db = new hospointEntities();

        // GET: RecetasyExamenes
        public ActionResult Index()
        {
            var recetasyExamenes = db.HosRecetasyExamenes.Include(r => r.HosEmpresa).Include(r => r.HosLogin).Include(r => r.clientes);
            return View(recetasyExamenes.ToList());
        }

        // GET: RecetasyExamenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosRecetasyExamenes recetasyExamenes = db.HosRecetasyExamenes.Find(id);
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

                ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre");
                ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username");
                ViewBag.idPaciente = new SelectList(db.clientes.Where(m=>m.estado=="1" && m.Usuarioid==usuarioid1 || m.estado=="1" && m.Usuarioid==null), "idcliente", "nombre");
                var s = (from datosid in db.HosRecetasyExamenes select datosid.id).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.HosRecetasyExamenes select datosid.id).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idreceta = idmostrar.ToString("D2");

                return View();

            }
            HosRecetasyExamenes citas = db.HosRecetasyExamenes.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre");
                ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username");
                ViewBag.idPaciente = new SelectList(db.clientes.Where(m => m.estado == "1" && m.Usuarioid == usuarioid1 || m.estado=="1" && m.Usuarioid==null), "idcliente", "nombre",citas.idPaciente);
                ViewBag.tipo = (from s in db.HosRecetasyExamenes where s.id==id select s.Tipo).FirstOrDefault();
                ViewBag.Detalle = (from s in db.HosRecetasyExamenes where s.id == id select s.Detalle).FirstOrDefault();
                
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
        public ActionResult Create([Bind(Include = "id,Tipo,Detalle,idPaciente,Estatus,Empresaid,Usuarioid,fecha")] HosRecetasyExamenes recetasyExamenes)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var t = (from s in db.HosRecetasyExamenes where s.id == recetasyExamenes.id select s.id).Count();

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
                    return RedirectToAction("RecetasyExamen","Consultas");

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
                    db.HosRecetasyExamenes.Add(recetasyExamenes);
                    db.SaveChanges();
                    return RedirectToAction("RecetasyExamen","Consultas");
                }
            }

            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.clientes, "idcliente", "nombre", recetasyExamenes.idPaciente);
            return RedirectToAction("RecetasyExamen","Consultas");
        }

            // GET: RecetasyExamenes/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosRecetasyExamenes recetasyExamenes = db.HosRecetasyExamenes.Find(id);
            if (recetasyExamenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", recetasyExamenes.idPaciente);
            return View(recetasyExamenes);
        }

        // POST: RecetasyExamenes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Tipo,Detalle,idPaciente,Estatus,Empresaid,Usuarioid,fecha")] HosRecetasyExamenes recetasyExamenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recetasyExamenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Empresaid = new SelectList(db.HosEmpresa, "IdEmpresa", "Nombre", recetasyExamenes.Empresaid);
            ViewBag.Usuarioid = new SelectList(db.HosLogin, "LoginId", "Username", recetasyExamenes.Usuarioid);
            ViewBag.idPaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", recetasyExamenes.idPaciente);
            return View(recetasyExamenes);
        }

        // GET: RecetasyExamenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HosRecetasyExamenes recetasyExamenes = db.HosRecetasyExamenes.Find(id);
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
            HosRecetasyExamenes recetasyExamenes = db.HosRecetasyExamenes.Find(id);
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
