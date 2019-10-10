using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;

namespace SpointLiteVersion.Controllers
{
    public class ConsultasController : Controller
    {
        private hospointEntities db = new hospointEntities();
        HosRecetasyExamenes receta1 = new HosRecetasyExamenes();

        // GET: Consultas
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
            var busca = (from s in db.HosDetalleTemporales select s.Id).Count();
            if (busca > 0)
            {
                db.VaciarTablaTemporal();
            }
            var consultas = db.HosConsultas.Include(c => c.clientes);
            return View(consultas.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1).ToList());
        }
        public JsonResult GetPaciente(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);


            //var numeros =Convert.ToInt32(s).ToString("D10");
           
            
           
            db.Configuration.ProxyCreationEnabled = false;
            List<clientes> productos = db.clientes.Where(m => m.idcliente == idpaciente && m.estado=="1" && m.Usuarioid==usuarioid1 || m.idcliente == idpaciente && m.estado == "1" && m.Usuarioid == null).ToList();
          
            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConsulta(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);



            var datosconsulta = db.HosConsultas.Where(m => m.idpaciente == idpaciente && m.Estatus==1 && m.Usuarioid==usuarioid1).FirstOrDefault();
            HosConsultas consultas = new HosConsultas();
            consultas.Compania = datosconsulta.Compania.ToString();
            consultas.Examenes = datosconsulta.Examenes.ToString();
            consultas.Diagnostico = datosconsulta.Diagnostico.ToString();
            consultas.Receta = datosconsulta.Receta.ToString();

            db.Configuration.ProxyCreationEnabled = false;
            List<HosConsultas> productos = new List<HosConsultas>();
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
            HosConsultas consultas = db.HosConsultas.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            return View(consultas);
        }
        public ActionResult VistaImpresion(int? id)
        {
            id = 20;
            return View(db.clientes.Where(m=>m.idcliente==id).ToList());
        }
        public JsonResult Obtenerdatos(int idpaciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);



            var datosconsulta = db.HosHistoriaClinica.Where(m => m.idpaciente == idpaciente && m.Estatus==1 && m.Usuarioid==usuarioid1).FirstOrDefault();
            HosHistoriaClinica consultas = new HosHistoriaClinica();
            consultas.antsociales = datosconsulta.antsociales.ToString();
            consultas.antecedentesfamiliares = datosconsulta.antecedentesfamiliares.ToString();
            consultas.historia = datosconsulta.historia.ToString();
            consultas.motivoconsulta = datosconsulta.motivoconsulta.ToString();


            db.Configuration.ProxyCreationEnabled = false;
            List<HosHistoriaClinica> productos = new List<HosHistoriaClinica>();
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
                ViewBag.idpaciente = new SelectList(db.clientes.Where(m=>m.estado=="1" && m.Usuarioid==usuarioid1 || m.estado == "1" && m.Usuarioid == null), "idcliente", "nombre");
                var s = (from datosid in db.HosConsultas where datosid.Estatus==1 select datosid.idConsulta).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.HosConsultas where datosid.Estatus==1 select datosid.idConsulta).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idconsulta = idmostrar.ToString("D2");
                return View();

            }
            HosConsultas paciente = db.HosConsultas.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.idpaciente = new SelectList(db.clientes.Where(m=>m.estado=="1" && m.Usuarioid==usuarioid1 || m.estado=="1" && m.Usuarioid==null), "idcliente", "nombre",paciente.idpaciente);
                ViewBag.id = "algo";
                if ((from s in db.HosConsultas where s.idConsulta == id && s.Usuarioid == usuarioid1 && s.Estatus == 1 select s.Hora).FirstOrDefault() != null)
                {
                    DateTime Horaprueba = Convert.ToDateTime((from s in db.HosConsultas where s.idConsulta == id && s.Usuarioid == usuarioid1 && s.Estatus == 1 select s.Hora).FirstOrDefault());
                    ViewBag.hora = Horaprueba.ToString("hh:mm tt");
                }
                DateTime fechaprueba=Convert.ToDateTime((from s in db.HosConsultas where s.idConsulta == id && s.Estatus == 1 && s.Usuarioid == usuarioid1 select s.fecha).FirstOrDefault());
                ViewBag.fecha = fechaprueba.ToString("yyyy-MM-dd");
                var historia = (from his in db.HosConsultas where his.idConsulta == id && his.Estatus==1 && his.Usuarioid==usuarioid1 select his).FirstOrDefault();
                if (historia != null)
                {
                    ViewBag.Observaciones = historia.Observaciones;
                    ViewBag.Diagnostico = historia.Diagnostico;
                    ViewBag.Receta = historia.Receta;
                    ViewBag.Examenes = historia.Examenes;
                    ViewBag.SeguroMedico = historia.SeguroMedico;
                    ViewBag.Compania = historia.Compania;
                    ViewBag.idreceta = (from recet in db.HosRecetasyExamenes where recet.idPaciente==paciente.idpaciente && recet.Tipo=="RECETA" && recet.Detalle.ToString().ToUpper()==historia.Receta.ToString().ToUpper() select recet.id).FirstOrDefault();
                    TempData["idejemplo"] = ViewBag.idreceta.ToString();
                    ViewBag.idreceta1 = (from recet in db.HosRecetasyExamenes where recet.idPaciente == paciente.idpaciente && recet.Tipo=="EXAMENES" && recet.Detalle.ToString().ToUpper() == historia.Receta.ToString().ToUpper() select recet.id).FirstOrDefault();
                    TempData["idejemplo1"] = ViewBag.idreceta1.ToString();
                }
                return View(paciente);

            }

            ViewBag.idpaciente = new SelectList(db.clientes.Where(m=>m.estado=="1" && m.Usuarioid==usuarioid1 || m.estado == "1" && m.Usuarioid == null), "idcliente", "nombre");
            return View();
        }

        // POST: Consultas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idConsulta,TipoConsulta,fecha,idpaciente,edad,telefono,SeguroMedico,Compania,Poliza,Observaciones,Diagnostico,Receta,Examenes,Hora")] HosConsultas consultas)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var t = (from s in db.HosConsultas where s.idConsulta == consultas.idConsulta && s.Estatus==1 select s.idConsulta).Count();
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
                        receta1.id = Convert.ToInt32(TempData["idejemplo"].ToString());
                        receta1.Detalle = consultas.Receta.ToUpper();
                        receta1.Estatus = 1;
                        receta1.Tipo = "RECETA";
                        receta1.idPaciente = consultas.idpaciente;
                        receta1.Usuarioid = usuarioid1;
                        receta1.Empresaid = empresaid1;
                        if (consultas.fecha != null)
                        {
                            receta1.fecha = consultas.fecha;
                        }
                      
                        db.Entry(receta1).State = EntityState.Modified;
                        db.SaveChanges();
                        consultas.Receta = consultas.Receta.ToUpper();
                    }
                    if (consultas.Examenes != null)
                    {

                        receta1.id = Convert.ToInt32(TempData["idejemplo1"].ToString());
                        receta1.Detalle = consultas.Examenes.ToUpper();
                        receta1.Estatus = 1;
                        receta1.Tipo = "EXAMENES";
                        receta1.idPaciente = consultas.idpaciente;
                        receta1.Empresaid = empresaid1;
                        receta1.Usuarioid = usuarioid1;
                        if (consultas.fecha != null)
                        {
                            receta1.fecha = consultas.fecha;
                        }
                        db.Entry(receta1).State = EntityState.Modified;
                        db.SaveChanges();

                        consultas.Examenes = consultas.Examenes.ToUpper();
                    }
                    consultas.Estatus = 1;
                    consultas.Empresaid = empresaid1;
                    consultas.Usuarioid = usuarioid1;
                    var buscar = (from s in db.HosDetalleTemporales select s.Id).Count();
                    if (buscar > 0)
                    {
                        db.TemptoEspecial();
                    }
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
                        receta1.Tipo = "RECETA";
                        receta1.Detalle = consultas.Receta.ToUpper();
                        receta1.idPaciente = consultas.idpaciente;
                        receta1.Estatus = 1;
                        receta1.Empresaid = empresaid1;
                        receta1.Usuarioid = usuarioid1;
                        if (consultas.fecha != null)
                        {
                            receta1.fecha = consultas.fecha;
                        }
                        db.HosRecetasyExamenes.Add(receta1);
                        db.SaveChanges();
                        consultas.Receta = consultas.Receta.ToUpper();
                    }
                    if (consultas.Examenes != null)
                    {
                        receta1.Tipo = "EXAMENES";
                        receta1.Detalle = consultas.Examenes.ToUpper();
                        receta1.idPaciente = consultas.idpaciente;
                        receta1.Estatus = 1;
                        receta1.Empresaid = empresaid1;
                        receta1.Usuarioid = usuarioid1;
                        if (consultas.fecha != null)
                        {
                            receta1.fecha = consultas.fecha;
                        }
                        db.HosRecetasyExamenes.Add(receta1);
                        db.SaveChanges();
                        consultas.Examenes = consultas.Examenes.ToUpper();
                    }
                    consultas.Estatus = 1;
                    consultas.Empresaid = empresaid1;
                    consultas.Usuarioid = usuarioid1;
                    var buscar = (from s in db.HosDetalleTemporales select s.Id).Count();
                    if (buscar > 0)
                    {
                        db.TemptoEspecial();
                    }
                    db.HosConsultas.Add(consultas);
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
            ViewBag.idpaciente = new SelectList(db.clientes, "idcliente", "nombre", consultas.idpaciente);
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
            HosConsultas consultas = db.HosConsultas.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpaciente = new SelectList(db.clientes, "idPaciente", "nombre", consultas.idpaciente);
            return View(consultas);
        }

        // POST: Consultas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idConsulta,TipoConsulta,fecha,idpaciente,edad,telefono,SeguroMedico,Compania,Poliza,Observaciones,Diagnostico,Receta,Examenes")] HosConsultas consultas)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(consultas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idpaciente = new SelectList(db.Hospaciente, "idPaciente", "nombre", consultas.idpaciente);
            return View(consultas);
        }
        public ActionResult GuardarConsultar(string TipoConsulta,string idConsulta1,string fecha,string idpaciente,string edad,string telefono,string SeguroMedico,string Compania,string Poliza,string Observaciones,string Diagnostico,string Hora,string Receta,string Examenes)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            string mensaje = "";
            HosConsultas consult = new HosConsultas();
            if (idpaciente == "undefined" || idpaciente=="")
            {
                mensaje = "Seleccione Un Paciente";
            }
            else
            {
                var consultas = 0;
                if (idConsulta1 != "undefined" || idConsulta1!="")
                {
                    consultas = Convert.ToInt32(idConsulta1);
                }
                var t = (from s in db.HosConsultas where s.idConsulta == consultas && s.Estatus == 1 select s.idConsulta).Count();
                if (t != 0)
                {
                    var consultaval = (from s in db.HosConsultas where s.idConsulta == consultas && s.Estatus == 1 select s.idConsulta).FirstOrDefault();

                    consult.idConsulta = consultaval;
                    if (TipoConsulta != "undefined" || TipoConsulta!="")
                    {
                        consult.TipoConsulta = TipoConsulta.ToUpper();
                    }
                    consult.fecha = Convert.ToDateTime(fecha);
                    if (idpaciente != "undefined" || idpaciente!="")
                    {
                        consult.idpaciente = Convert.ToInt32(idpaciente);
                    }
                    if (telefono != "undefined" || telefono!="")
                    {
                        consult.telefono = telefono;
                    }
                    if (SeguroMedico != "undefined" || SeguroMedico!="")
                    {
                        consult.SeguroMedico = SeguroMedico.ToUpper();
                    }
                    if (Compania != "undefined" || Compania!="")
                    {
                        consult.Compania = Compania.ToUpper();
                    }
                    if (Poliza != "undefined" || Poliza!="")
                    {
                        consult.Poliza = Poliza.ToUpper();
                    }
                    if (Observaciones != "undefined" || Observaciones!="")
                    {
                        consult.Observaciones = Observaciones.ToUpper();
                    }
                    if (Diagnostico != "undefined" || Diagnostico!="")
                    {
                        consult.Diagnostico = Diagnostico.ToUpper();
                    }
                    if (Receta != "undefined" || Receta!="")
                    {
                        receta1.idPaciente = Convert.ToInt32(idpaciente);
                        receta1.Detalle = Receta.ToUpper();
                        receta1.Estatus = 1;
                        receta1.Tipo = "RECETA";
                        receta1.fecha = Convert.ToDateTime(fecha);
                        consult.Receta = Receta.ToUpper();
                        
                        receta1.Empresaid = empresaid1;
                        receta1.Usuarioid = usuarioid1;
                        if(fecha!="undefined" || fecha != "")
                        {
                            receta1.fecha = Convert.ToDateTime(fecha);
                        }
                        db.HosRecetasyExamenes.Add(receta1);
                        db.SaveChanges();
                    }
                    if (Examenes != "undefined" || Examenes!="")
                    {
                        receta1.idPaciente = Convert.ToInt32(idpaciente);
                        receta1.Detalle = Examenes.ToUpper();
                        receta1.Estatus = 1;
                        receta1.Tipo = "EXAMEN";
                        receta1.Empresaid = empresaid1;
                        receta1.Usuarioid = usuarioid1;
                        if (fecha != "undefined" || fecha != "")
                        {
                            receta1.fecha = Convert.ToDateTime(fecha);
                        }
                        
                        db.HosRecetasyExamenes.Add(receta1);
                        db.SaveChanges();
                        consult.Examenes = Examenes.ToUpper();
                    }
                    if (edad != "undefined" || edad!="")
                    {
                        consult.edad = edad;
                    }
                    consult.Estatus = 1;
                    consult.Empresaid = empresaid1;
                    consult.Usuarioid = usuarioid1;
                    var buscar = (from s in db.HosDetalleTemporales select s.Id).Count();
                    if (buscar > 0)
                    {
                        db.TemptoEspecial();
                    }
                    consult.Hora = Hora;
                    db.Entry(consult).State = EntityState.Modified;
                    db.SaveChanges();
                    var idConsulta = consult.idConsulta;
                    mensaje = "CONSULTA GUARDADA CON EXITO...";
                    ViewBag.idPacientes = idConsulta.ToString();
                    Session["idConsulta"] = idConsulta;
                }
                else
                {
                    if (TipoConsulta != "undefined" || TipoConsulta!="")
                    {
                        consult.TipoConsulta = TipoConsulta.ToUpper();
                    }
                    consult.fecha = Convert.ToDateTime(fecha);
                    if (idpaciente != "undefined" || idpaciente!="")
                    {
                        consult.idpaciente = Convert.ToInt32(idpaciente);
                    }
                    if (telefono != "undefined" || telefono!="")
                    {
                        consult.telefono = telefono;
                    }
                    if (SeguroMedico != "undefined" || SeguroMedico!="")
                    {
                        consult.SeguroMedico = SeguroMedico.ToUpper();
                    }
                    if (Compania != "undefined" || Compania!="")
                    {
                        consult.Compania = Compania.ToUpper();
                    }
                    if (Poliza != "undefined" || Poliza!="")
                    {
                        consult.Poliza = Poliza.ToUpper();
                    }
                    if (Observaciones != "undefined" || Observaciones!="")
                    {
                        consult.Observaciones = Observaciones.ToUpper();
                    }
                    if (Diagnostico != "undefined" || Diagnostico!="")
                    {
                        consult.Diagnostico = Diagnostico.ToUpper();
                    }
                    if (Receta != "undefined" || Receta!="")
                    {
                        consult.Receta = Receta.ToUpper();
                    }
                    if (Examenes != "undefined" || Examenes!="")
                    {
                        consult.Examenes = Examenes.ToUpper();
                    }
                    if (edad != "undefined" || edad!="")
                    {
                        consult.edad = edad;
                    }
                    consult.Estatus = 1;
                    consult.Empresaid = empresaid1;
                    consult.Usuarioid = usuarioid1;
                    var buscar = (from s in db.HosDetalleTemporales select s.Id).Count();
                    if (buscar > 0)
                    {
                        db.TemptoEspecial();
                        
                    }
                    consult.Hora = Hora;
                    db.HosConsultas.Add(consult);
                    db.SaveChanges();

                    var idConsulta = consult.idConsulta;
                    mensaje = "CONSULTA GUARDADA CON EXITO...";
                    ViewBag.idPacientes = idConsulta.ToString();
                    Session["idConsulta"] = idConsulta;
                }
            }
            return Json(mensaje);
        }

        public ActionResult GuardarReceta(string idpaciente,string Receta,string fecha,string idrecet,string tipo)
        {
            string mensaje = "";
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            HosRecetasyExamenes consulta = new HosRecetasyExamenes();
            if (idrecet != null && idrecet != "" && idrecet != "undefined")
            {
                if (idpaciente == "undefined" || idpaciente == "" || Receta == "undefined" || Receta == "")
                {
                    if (idpaciente == "undefined" || idpaciente == "") mensaje = "Seleccione Un Paciente";
                    if (Receta == "undefined" || Receta == "") mensaje = "Ingrese las indicaciones de la Receta";
                }
                else
                {
                    consulta.id = Convert.ToInt32(idrecet);
                    if (Receta != "undefined" || Receta == "")
                    {
                        consulta.Detalle = Receta.ToUpper();
                    }
                    if (idpaciente != "undefined" || idpaciente == "")
                    {
                        consulta.idPaciente = Convert.ToInt32(idpaciente);
                    }
                    if (fecha != "undefined" || fecha == "")
                    {
                        consulta.fecha = Convert.ToDateTime(fecha);
                    }
                    if (tipo != "" && tipo != "undefined" && tipo != null)
                    {
                        consulta.Tipo = tipo.ToString().ToUpper();

                    }
                    //else
                    //{
                    //    consulta.Tipo = "RECETA";
                    //}
                    consulta.Estatus = 1;
                    consulta.Empresaid = empresaid1;
                    consulta.Usuarioid = usuarioid1;
                    db.Entry(consulta).State = EntityState.Modified;
                    db.SaveChanges();
                    mensaje = "DETALLE GUARDADOS CON EXITO...";
                }

            }
            
            else
            {
                System.Diagnostics.Debug.Write("Id del Paciente" + idpaciente);
                System.Diagnostics.Debug.Write("Receta" + Receta);
                if (idpaciente == "undefined" || idpaciente == "" || Receta == "undefined" || Receta == "")
                {
                    if (idpaciente == "undefined" || idpaciente == "") mensaje = "Seleccione Un Paciente";
                    if (Receta == "undefined" || Receta == "") mensaje = "Ingrese las indicaciones de la Receta";
                }
                else
                {
                    if (Receta != "undefined" || Receta == "")
                    {
                        consulta.Detalle = Receta.ToUpper();
                    }
                    if (idpaciente != "undefined" || idpaciente == "")
                    {
                        consulta.idPaciente = Convert.ToInt32(idpaciente);
                    }
                    if (fecha != "undefined" || fecha == "")
                    {
                        consulta.fecha = Convert.ToDateTime(fecha);
                    }
                    consulta.Tipo = "RECETA";
                    consulta.Estatus = 1;
                    consulta.Empresaid = empresaid1;
                    consulta.Usuarioid = usuarioid1;
                    if (tipo != "" && tipo != "undefined" && tipo != null)
                    {
                        consulta.Tipo = tipo.ToString().ToUpper();

                    }
                    db.HosRecetasyExamenes.Add(consulta);
                    db.SaveChanges();
                    var id = consulta.id;
                    mensaje = "RECETA GUARDADA CON EXITO...";
                    ViewBag.idPacientes = id.ToString();
                    Session["id"] = id;
                }
            }
            
            return Json(mensaje);
        }

        public ActionResult RecetasyExamen() {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            return View(db.HosRecetasyExamenes.Where(m=>m.Usuarioid==usuarioid1 && m.Estatus==1).ToList());
        }
        public ActionResult GuardarExamen(string idpaciente, string Examenes,string fecha)
        {
            string mensaje = "";
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            HosRecetasyExamenes consulta = new HosRecetasyExamenes();
            if (idpaciente == "undefined" || idpaciente == "" || Examenes == "undefined" || Examenes=="undefined")
            {
                if (idpaciente == "undefined" || idpaciente=="") mensaje = "Seleccione Un Paciente";
                if (Examenes == "undefined" || Examenes=="") mensaje = "Ingrese las indicaciones del Examen";
            }
            else
            {
                if (Examenes != "")
                {
                    consulta.Detalle = Examenes.ToUpper();
                }
                if (idpaciente != null && idpaciente!="")
                {
                    consulta.idPaciente = Convert.ToInt32(idpaciente);
                }
                if(fecha!="undefined" && fecha != "")
                {
                    consulta.fecha = Convert.ToDateTime(fecha);
                }
                consulta.Estatus = 1;
                consulta.Tipo = "EXAMENES";

                consulta.Empresaid = empresaid1;
                consulta.Usuarioid = usuarioid1;
                db.HosRecetasyExamenes.Add(consulta);
                db.SaveChanges();
                var id = consulta.id;
                mensaje = "EXAMEN GUARDADO CON EXITO...";
                ViewBag.idPacientes = id.ToString();
                Session["id"] = id;
            }
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
            HosConsultas consultas = db.HosConsultas.Find(id);
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
            HosConsultas consultas = db.HosConsultas.Find(id);
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

            HosRecetasyExamenes consultas = db.HosRecetasyExamenes.Find(id);
            if (consultas == null)
            {
                return HttpNotFound();
            }
            return View(consultas);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult BorrarConfirmed(int id)
        {
            HosRecetasyExamenes consultas = db.HosRecetasyExamenes.Find(id);
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
