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
        especiales especial = new especiales();

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
            return View(db.especiales.Where(m=>m.estatus==1).ToList());
        }
        public ActionResult DetallesTemporales(int? id)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            return View(db.DetalleTemporales.Where(m => m.estatus == 1 && m.Usuarioid == usuarioid1 && m.idConsulta == id).ToList());
        }
        public ActionResult DatosEspeciales(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var buscar = (from s in db.DetalleTemporales select s.Id).Count();
            if (buscar > 0)
            {
                return RedirectToAction("DetallesTemporales",new { id=id});
            }
            else
            {
                return View(db.DatosEspeciales.Where(m => m.estatus == 1 && m.Usuarioid == usuarioid1 && m.idConsulta == id).ToList());

            }
        }
        public ActionResult especiales(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            System.Diagnostics.Debug.Write("El id que quiero es:" + id);
            var buscar = (from s in db.DetalleTemporales where s.idConsulta==id select s.Id).Count();
            var buscar1 = (from s in db.DatosEspeciales where s.idConsulta==id select s.Id).Count();
            if (buscar>0 || buscar1>0)
            {
                return RedirectToAction("DatosEspeciales",new {id=id});
            }
            else
            {
                var usuarioid = Session["userid"].ToString();
                var empresaid = Session["empresaid"].ToString();
                var usuarioid1 = Convert.ToInt32(usuarioid);
                var empresaid1 = Convert.ToInt32(empresaid);
                ViewBag.Valor = (from s in db.DatosEspeciales where s.idpaciente == id select s.Valor).FirstOrDefault();
                return View(db.especiales.Where(m => m.estatus == 1 && m.usuarioid == usuarioid1).ToList());
            }
        }
        [HttpPost]
        public ActionResult especiales(string valor,string nombre)
        {
            string mensaje = "";

            return Json(mensaje);
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
                ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username");

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
                ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username",especial.usuarioid);
                ViewBag.tipo = (from s in db.especiales where s.Id == id select s.Tipo).FirstOrDefault();

                return View(especial);
            }
            return View();
        }

        public ActionResult GuardarEspeciales(string idConsulta,string paciente,List<DetalleTemporales> ListadoDetalle)
        {
            string Mensaje = "";
            var valor="";
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var paciente2 = 0;
           
            if (paciente != "undefined" && paciente != "" && paciente != null)
            {
                paciente2 = Convert.ToInt32(paciente);
            }
            foreach (var data in ListadoDetalle)
            {
                if (data.Valor != "undefined" && data.Valor != null && data.Valor != "")
                {
                   
                   
                        var idespecial = Convert.ToInt32(data.IdDatosEspeciales.ToString());
                    var idconsult = Convert.ToInt32(idConsulta);

                        valor = data.Valor.ToString();
                        System.Diagnostics.Debug.Write("id consulta es: " + paciente2);
                        DetalleTemporales especial = new DetalleTemporales(idespecial, valor, usuarioid1, paciente2, empresaid1,1,idconsult);
                        db.DetalleTemporales.Add(especial);
                        db.SaveChanges();
                        Mensaje = "Guardado Con Exito";
                    

                }
                else
                {
                    Mensaje = "Debes Llenar Algun Valor";
                }
              
            }
            return Json(Mensaje);
        }
        // POST: especiales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Tipo,estatus,empresaid,usuarioid")] especiales especiales)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
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
                    especiales.empresaid = empresaid1;
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
                    especiales.empresaid = empresaid1;
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
            especiales.estatus = 0;
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
