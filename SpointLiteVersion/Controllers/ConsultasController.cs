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
    public class ConsultasController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: Consultas
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var consultas = db.Consultas.Include(c => c.paciente);
            return View(consultas.Where(m=>m.Estatus==1).ToList());
        }
        public JsonResult GetPaciente(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);


            //var numeros =Convert.ToInt32(s).ToString("D10");
            var numero = 0;
           
            
           
            db.Configuration.ProxyCreationEnabled = false;
            List<paciente> productos = db.paciente.Where(m => m.idPaciente == idpaciente && m.Estatus==1).ToList();
          
            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConsulta(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);



            var datosconsulta = db.Consultas.Where(m => m.idpaciente == idpaciente && m.Estatus==1).FirstOrDefault();
            Consultas consultas = new Consultas();
            consultas.Compania = datosconsulta.Compania.ToString();
            consultas.Examenes = datosconsulta.Examenes.ToString();
            consultas.Diagnostico = datosconsulta.Diagnostico.ToString();
            consultas.Receta = datosconsulta.Receta.ToString();

            db.Configuration.ProxyCreationEnabled = false;
            List<Consultas> productos = new List<Consultas>();
            productos.Add(consultas);
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
      
        // GET: Consultas/Details/5
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
            Consultas consultas = db.Consultas.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            return View(consultas);
        }
        public ActionResult VistaImpresion(int? id)
        {
            id = 20;
            return View(db.paciente.Where(m=>m.idPaciente==id).ToList());
        }
        public JsonResult Obtenerdatos(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);



            var datosconsulta = db.HistoriaClinica.Where(m => m.idpaciente == idpaciente && m.Estatus==1).FirstOrDefault();
            HistoriaClinica consultas = new HistoriaClinica();
            consultas.antsociales = datosconsulta.antsociales.ToString();
            consultas.antecedentesfamiliares = datosconsulta.antecedentesfamiliares.ToString();
            consultas.historia = datosconsulta.historia.ToString();
            consultas.motivoconsulta = datosconsulta.motivoconsulta.ToString();


            db.Configuration.ProxyCreationEnabled = false;
            List<HistoriaClinica> productos = new List<HistoriaClinica>();
            productos.Add(consultas);
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult reporteActual(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id != null)
            {
                string idconsulta = id.ToString();
                return Redirect("~/RTPFactura/WebForm1.aspx?idconsulta=" + idconsulta);
            }
            else if (Session["idConsulta"].ToString() != null || Session["idConsulta"].ToString()!="")
            {
                string idconsulta = Session["idConsulta"].ToString();
                Session["idConsulta"] = "";
                
                return Redirect("~/RTPFactura/WebForm1.aspx?idconsulta=" + idconsulta);

            }

                return View("Index");
            
            //return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+001);




        }

        public ActionResult reporteExamenes(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id != null)
            {
                string idPaciente = id.ToString();
                return Redirect("~/RTPFactura/Examen.aspx?id=" + id);
            }
            else if (Session["id"].ToString() != null || Session["id"].ToString() != "")
            {
                string idp = Session["id"].ToString();
                Session["id"] = "";

                return Redirect("~/RTPFactura/Examen.aspx?id=" + idp);

            }
            else
            {
                return View("Index");
            }
            //return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+001);





        }

        public ActionResult reporteReceta(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id != null)
            {
                string idp = id.ToString();
                return Redirect("~/RTPFactura/Receta.aspx?id=" + idp);
            }
            else if (Session["id"].ToString() != null || Session["id"].ToString() != "")
            {
                string idp = Session["id"].ToString();
                Session["id"] = "";

                return Redirect("~/RTPFactura/Receta.aspx?id=" + idp);

            }
            else
            {
                return View("Index");
            }
            //return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+001);





        }
        // GET: Consultas/Create
        public ActionResult Create(int? id,int? idpaciente)
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
                ViewBag.idmostrarseleccion = idpaciente;
                ViewBag.idpaciente = new SelectList(db.paciente.Where(m=>m.Estatus==1), "idPaciente", "nombre");
                var s = (from datosid in db.Consultas where datosid.Estatus==1 select datosid.idConsulta).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.Consultas where datosid.Estatus==1 select datosid.idConsulta).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idconsulta = idmostrar.ToString("D2");
                return View();

            }
            Consultas paciente = db.Consultas.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.idpaciente = new SelectList(db.paciente.Where(m=>m.Estatus==1), "idPaciente", "nombre",paciente.idpaciente);
                ViewBag.id = "algo";

                var historia = (from his in db.Consultas where his.idConsulta == id && his.Estatus==1 select his).FirstOrDefault();
                if (historia != null)
                {
                    ViewBag.Observaciones = historia.Observaciones;
                    ViewBag.Diagnostico = historia.Diagnostico;
                    ViewBag.Receta = historia.Receta;
                    ViewBag.Examenes = historia.Examenes;
                    ViewBag.SeguroMedico = historia.SeguroMedico;
                    ViewBag.Compania = historia.Compania;

                }

                return View(paciente);

            }

            ViewBag.idpaciente = new SelectList(db.paciente.Where(m=>m.Estatus==1), "idPaciente", "nombre");
            return View();
        }

        // POST: Consultas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idConsulta,TipoConsulta,fecha,idpaciente,edad,telefono,SeguroMedico,Compania,Poliza,Observaciones,Diagnostico,Receta,Examenes")] Consultas consultas)
        {
            var t = (from s in db.Consultas where s.idConsulta == consultas.idConsulta && s.Estatus==1 select s.idConsulta).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (consultas.TipoConsulta != null)
                    {
                        consultas.TipoConsulta = consultas.TipoConsulta.ToUpper();
                    }
                    if (consultas.Compania != null)
                    {
                        consultas.Compania = consultas.Compania.ToUpper();
                    }
                    if (consultas.Observaciones != null)
                    {
                        consultas.Observaciones = consultas.Observaciones.ToUpper();
                    }
                    if (consultas.Diagnostico != null)
                    {
                        consultas.Diagnostico = consultas.Diagnostico.ToUpper();
                    }
                    if (consultas.Receta != null)
                    {
                        consultas.Receta = consultas.Receta.ToUpper();
                    }
                    if (consultas.Examenes != null)
                    {
                        consultas.Examenes = consultas.Examenes.ToUpper();
                    }
                    consultas.Estatus = 1;
                    db.Entry(consultas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (consultas.idConsulta <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (consultas.TipoConsulta != null)
                    {
                        consultas.TipoConsulta = consultas.TipoConsulta.ToUpper();
                    }
                    if (consultas.Compania != null)
                    {
                        consultas.Compania = consultas.Compania.ToUpper();
                    }
                    if (consultas.Observaciones != null)
                    {
                        consultas.Observaciones = consultas.Observaciones.ToUpper();
                    }
                    if (consultas.Diagnostico != null)
                    {
                        consultas.Diagnostico = consultas.Diagnostico.ToUpper();
                    }
                    if (consultas.Receta != null)
                    {
                        consultas.Receta = consultas.Receta.ToUpper();
                    }
                    if (consultas.Examenes != null)
                    {
                        consultas.Examenes = consultas.Examenes.ToUpper();
                    }
                    consultas.Estatus = 1;
                    db.Consultas.Add(consultas);
                    db.SaveChanges();
                    var id = consultas.idpaciente;

                    //return RedirectToAction("Index", new { target = "_blank" })
                    if (id != null)
                    {
                        string idPaciente = id.ToString();
                    }
                    return RedirectToAction("Index");

                }
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", consultas.idpaciente);
            return View(consultas);
        }

        // GET: Consultas/Edit/5
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
            Consultas consultas = db.Consultas.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", consultas.idpaciente);
            return View(consultas);
        }

        // POST: Consultas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idConsulta,TipoConsulta,fecha,idpaciente,edad,telefono,SeguroMedico,Compania,Poliza,Observaciones,Diagnostico,Receta,Examenes")] Consultas consultas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idpaciente = new SelectList(db.paciente, "idPaciente", "nombre", consultas.idpaciente);
            return View(consultas);
        }
        public ActionResult GuardarConsultar(string TipoConsulta,string idConsulta1,string fecha,string idpaciente,string edad,string telefono,string SeguroMedico,string Compania,string Poliza,string Observaciones,string Diagnostico,string Receta,string Examenes)
        {
            string mensaje = "";
            Consultas consult = new Consultas();
            var consultas = 0;
            if (idConsulta1 != "undefined")
            {
                 consultas = Convert.ToInt32(idConsulta1);
            }
            var t = (from s in db.Consultas where s.idConsulta == consultas && s.Estatus == 1 select s.idConsulta).Count();
            if (t != 0)
            {
                var consultaval = (from s in db.Consultas where s.idConsulta == consultas && s.Estatus == 1 select s.idConsulta).FirstOrDefault();
                System.Diagnostics.Debug.Write("El Seguro es:" + SeguroMedico.ToUpper());

                consult.idConsulta = consultaval;
                if (TipoConsulta != "undefined")
                {
                    consult.TipoConsulta = TipoConsulta.ToUpper();
                }
                consult.fecha = Convert.ToDateTime(fecha);
                if (idpaciente != "undefined")
                {
                    consult.idpaciente = Convert.ToInt32(idpaciente);
                }
                if (telefono != "undefined")
                {
                    consult.telefono = telefono;
                }
                if (SeguroMedico != "undefined")
                {
                    consult.SeguroMedico = SeguroMedico.ToUpper();
                }
                if (Compania != "undefined")
                {
                    consult.Compania = Compania.ToUpper();
                }
                if (Poliza != "undefined")
                {
                    consult.Poliza = Poliza.ToUpper();
                }
                if (Observaciones != "undefined")
                {
                    consult.Observaciones = Observaciones.ToUpper();
                }
                if (Diagnostico != "undefined")
                {
                    consult.Diagnostico = Diagnostico.ToUpper();
                }
                if (Receta != "undefined")
                {
                    consult.Receta = Receta.ToUpper();
                }
                if (Examenes != "undefined")
                {
                    consult.Examenes = Examenes.ToUpper();
                }
                if (edad != "undefined")
                {
                    consult.edad = edad;
                }
                consult.Estatus = 1;

                db.Entry(consult).State = EntityState.Modified;
                db.SaveChanges();
                var idConsulta = consult.idConsulta;
                mensaje = "CONSULTA GUARDADA CON EXITO...";
                ViewBag.idPacientes = idConsulta.ToString();
                Session["idConsulta"] = idConsulta;
            }
            else
            {
                if (TipoConsulta != "undefined")
                {
                    consult.TipoConsulta = TipoConsulta.ToUpper();
                }
                consult.fecha = Convert.ToDateTime(fecha);
                if (idpaciente != "undefined")
                {
                    consult.idpaciente = Convert.ToInt32(idpaciente);
                }
                if (telefono != "undefined")
                {
                    consult.telefono = telefono;
                }
                if (SeguroMedico != "undefined")
                {
                    consult.SeguroMedico = SeguroMedico.ToUpper();
                }
                if (Compania != "undefined")
                {
                    consult.Compania = Compania.ToUpper();
                }
                if (Poliza != "undefined")
                {
                    consult.Poliza = Poliza.ToUpper();
                }
                if (Observaciones != "undefined")
                {
                    consult.Observaciones = Observaciones.ToUpper();
                }
                if (Diagnostico != "undefined")
                {
                    consult.Diagnostico = Diagnostico.ToUpper();
                }
                if (Receta != "undefined")
                {
                    consult.Receta = Receta.ToUpper();
                }
                if (Examenes != "undefined")
                {
                    consult.Examenes = Examenes.ToUpper();
                }
                if (edad != "undefined")
                {
                    consult.edad = edad;
                }
                consult.Estatus = 1;
                db.Consultas.Add(consult);
                db.SaveChanges();

                var idConsulta = consult.idConsulta;
                mensaje = "CONSULTA GUARDADA CON EXITO...";
                ViewBag.idPacientes = idConsulta.ToString();
                Session["idConsulta"] = idConsulta;
            }
            return Json(mensaje);
        }

        public ActionResult GuardarReceta(string idpaciente,string Receta)
        {
            string mensaje = "";
            RecetasyExamenes consulta = new RecetasyExamenes();
            if (Receta != "")
            {
                consulta.Detalle = Receta.ToUpper();
            }
            if (idpaciente != null)
            {
                consulta.idPaciente = Convert.ToInt32(idpaciente);
            }

            consulta.Tipo = "RECETA";
            consulta.Estatus = 1;

            db.RecetasyExamenes.Add(consulta);
            db.SaveChanges();
            var id = consulta.id;
            mensaje = "RECETA GUARDADA CON EXITO...";
            ViewBag.idPacientes = id.ToString();
            Session["id"] = id;
            return Json(mensaje);
        }

        public ActionResult RecetasyExamen() {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(db.RecetasyExamenes.ToList());
        }
        public ActionResult GuardarExamen(string idpaciente, string Examenes)
        {
            string mensaje = "";
            RecetasyExamenes consulta = new RecetasyExamenes();
            if (Examenes != "")
            {
                consulta.Detalle = Examenes.ToUpper();
            }
            if (idpaciente != null)
            {
                consulta.idPaciente = Convert.ToInt32(idpaciente);
            }
            consulta.Estatus = 1;
            consulta.Tipo = "EXAMEN";


            db.RecetasyExamenes.Add(consulta);
            db.SaveChanges();
            var id = consulta.id;
            mensaje = "EXAMEN GUARDADO CON EXITO...";
            ViewBag.idPacientes = id.ToString();
            Session["id"] = id;
            return Json(mensaje);
        }
        // GET: Consultas/Delete/5
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
            Consultas consultas = db.Consultas.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            return View(consultas);
        }

        // POST: Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consultas consultas = db.Consultas.Find(id);
            consultas.Estatus = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Borrar(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RecetasyExamenes consultas = db.RecetasyExamenes.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            return View(consultas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult BorrarConfirmed(int id)
        {
            RecetasyExamenes consultas = db.RecetasyExamenes.Find(id);
            consultas.Estatus = 0;
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
