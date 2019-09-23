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
    public class pacientesController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: pacientes
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
            var paciente = db.paciente.Include(p => p.ciudad);
            return View(paciente.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1).ToList());
        }

        // GET: pacientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // GET: pacientes/Create
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

                ViewBag.idciuddad = new SelectList(db.ciudad.Where(m=>m.Usuarioid==usuarioid1), "idciudad", "Nombre");
                var s = (from datosid in db.paciente where datosid.Estatus==1 select datosid.idPaciente).FirstOrDefault();
                var idmostrar = 0;
                if (s > 0)
                {
                    idmostrar = (from datosid in db.paciente where datosid.Estatus==1  select datosid.idPaciente).Max() + 1;
                }
                else
                {
                    idmostrar = 1;
                }
                ViewBag.idpaciente = idmostrar.ToString("D2");
                return View();

            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }

            if (id != null)
            {
                ViewBag.idciuddad = new SelectList(db.ciudad.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1), "idciudad", "Nombre", paciente.idciuddad);
                ViewBag.id = "algo";
                ViewBag.foto = paciente.Foto;
              var nacimiento = (from s in db.paciente where s.idPaciente == id && s.Usuarioid==usuarioid1 select s.fechanacimiento).FirstOrDefault();
                var edad1 = (from s in db.paciente where s.idPaciente == id && s.Usuarioid==usuarioid1 select s.edad).FirstOrDefault();
                if (nacimiento != null)
                {
                    ViewBag.nacimiento = nacimiento.Value.Date.ToString("yyyy-MM-dd");
                }
                if (edad1 != null)
                {
                    ViewBag.edad1 = edad1.ToString();
                }
                var historia = (from his in db.HistoriaClinica where his.idpaciente == id && his.Estatus==1 && his.Usuarioid==usuarioid1 select his).FirstOrDefault();
                if (historia != null)
                {
                    ViewBag.consulta = historia.motivoconsulta;
                    ViewBag.medico = historia.AntecedentesMedicos;
                    ViewBag.ginecologico = historia.antecedentesGinecologico;
                    ViewBag.alergia = historia.alergia;
                    ViewBag.medicamento = historia.medicamentos;
                    ViewBag.revision = historia.Revisionporsistema;
                    ViewBag.enfermedad = historia.historia;
                    ViewBag.personales = historia.antsociales;
                    ViewBag.familiares = historia.antecedentesfamiliares;
                    ViewBag.vacuna = historia.vacunas;
                    ViewBag.habito = historia.habitos;
                    ViewBag.estatura = historia.estaturas;
                    ViewBag.peso = historia.peso;
                    ViewBag.temperatura = historia.temperatura;
                    ViewBag.gruposanguineo = historia.gruposanguineo;
                }
                return View(paciente);
            }
            return View();

        }

        // POST: pacientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPaciente,nombre,telefono,telefono2,idciuddad,cedula,direccion,email,fechanacimiento,Estatus,sexo,EstadoCivil,Foto")] paciente paciente)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            var t = (from s in db.paciente where s.idPaciente == paciente.idPaciente &&s.Estatus==1 && s.Usuarioid==usuarioid1 select s.idPaciente).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (paciente.direccion != null)
                    {
                        paciente.direccion = paciente.direccion.ToUpper();
                    }
                    if (paciente.nombre != null)
                    {
                        paciente.nombre = paciente.nombre.ToUpper();
                    }
                    if (paciente.email != null)
                    {
                        paciente.email = paciente.email.ToUpper();
                    }
                    if (paciente.sexo != "")
                    {
                        paciente.sexo = paciente.sexo.ToUpper();
                    }
                    if (paciente.EstadoCivil != "")
                    {
                        paciente.EstadoCivil = paciente.EstadoCivil.ToUpper();
                    }
                    if (paciente.fechanacimiento != null)
                    {
                        DateTime now = DateTime.Today;
                        var ano = paciente.fechanacimiento.Value.Year;
                        int edad = DateTime.Today.Year - ano;

                        if (DateTime.Today < paciente.fechanacimiento.Value.AddYears(edad))
                        {
                            --edad;
                        }
                        paciente.edad = edad;
                    }
                    paciente.Estatus = 1;
                    paciente.Empresaid = empresaid1;
                    paciente.Usuarioid = usuarioid1;
                    db.Entry(paciente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (paciente.idPaciente <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (paciente.direccion != null)
                    {
                        paciente.direccion = paciente.direccion.ToUpper();
                    }
                    if (paciente.nombre != null)
                    {
                        paciente.nombre = paciente.nombre.ToUpper();
                    }
                    if (paciente.email != null)
                    {
                        paciente.email = paciente.email.ToUpper();
                    }
                    if (paciente.fechanacimiento != null)
                    {
                        DateTime now = DateTime.Today;
                        var ano = paciente.fechanacimiento.Value.Year;
                        int edad = DateTime.Today.Year - ano;

                        if (DateTime.Today < paciente.fechanacimiento.Value.AddYears(edad))
                        {
                            --edad;
                        }
                        paciente.edad = edad;
                    }
                    paciente.Estatus = 1;
                    paciente.Empresaid = empresaid1;
                    paciente.Usuarioid = usuarioid1;
                    db.paciente.Add(paciente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.idciuddad = new SelectList(db.ciudad.Where(m=>m.Estatus==1 ), "idciudad", "Nombre", paciente.idciuddad);
            return View(paciente);
        }
        public JsonResult Getciudad()
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            db.Configuration.ProxyCreationEnabled = false;
            List<ciudad> ciudad = db.ciudad.Where(m=>m.Estatus==1 && m.Usuarioid==usuarioid1).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(ciudad, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getedad(string fechanacimiento)
        {
            db.Configuration.ProxyCreationEnabled = false;
            paciente paciente1 = new paciente();
            List<paciente> paciente = new List<paciente>();
            if (fechanacimiento != null)
            {
                DateTime now = DateTime.Today;
                var fecha = Convert.ToDateTime(fechanacimiento);
                var ano = fecha.Year;
                int edad = DateTime.Today.Year - ano;

                if (DateTime.Today < fecha.AddYears(edad))
                {
                    --edad;
                }
                paciente1.edad = edad;
                paciente.Add(paciente1);
            }
            return Json(paciente, JsonRequestBehavior.AllowGet);
        }

        // GET: pacientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            ViewBag.idciuddad = new SelectList(db.ciudad.Where(m=>m.Estatus==1), "idciudad", "Nombre", paciente.idciuddad);
            return View(paciente);
        }

        // POST: pacientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPaciente,nombre,telefono,telefono2,idciuddad,cedula,direccion,email,fechanacimiento,NCF,Estatus")] paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idciuddad = new SelectList(db.ciudad.Where(m=>m.Estatus==1), "idciudad", "Nombre", paciente.idciuddad);
            return View(paciente);
        }
        public ActionResult GuardarConsultar(string nombre, string fechanacimiento, string idpaciente, string idPacient, string direccion, string telefono, string telefono2, string cedula, string idciuddad, string email, string sexo, string EstadoCivil, string Foto)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            string mensaje = "";
            int idpacientebuscar = 0;
            int ciudad = 0;
            if (idpaciente!= "undefined")
            {
                idpacientebuscar = Convert.ToInt32(idpaciente);
            } else if (idPacient!= "undefined")
            {
                idpacientebuscar = Convert.ToInt32(idPacient);
            }
            if (idciuddad != "undefined")
            {
                ciudad =Convert.ToInt32(idciuddad);
            }
            paciente paciente = new paciente();
            var t = (from s in db.paciente where s.idPaciente == idpacientebuscar && s.Estatus == 1 && s.Usuarioid==usuarioid1 select s.idPaciente).Count();
            if (t != 0)
            {
                var si = (from db in db.paciente where db.idPaciente == idpacientebuscar && db.Usuarioid==usuarioid1 select db.idPaciente).FirstOrDefault();
                paciente.idPaciente = si;
                if (direccion != null)
                {
                    paciente.direccion = direccion.ToUpper();
                }
                if (nombre != null)
                {
                    paciente.nombre = nombre.ToUpper();
                }
                if (email != null)
                {
                    paciente.email = email.ToUpper();
                }
                if (sexo !=null)
                {
                    paciente.sexo = sexo.ToUpper();
                }
                if (EstadoCivil !=null)
                {
                    paciente.EstadoCivil = EstadoCivil.ToUpper();
                }
                if (fechanacimiento != null)
                {
                    var fech1 = Convert.ToDateTime(fechanacimiento);

                    DateTime now1 = DateTime.Today;
                    var ano1 = fech1.Year;
                    int edad1 = DateTime.Today.Year - ano1;

                    if (DateTime.Today < fech1.AddYears(edad1))
                    {
                        --edad1;
                    }
                    paciente.edad = edad1;
                    paciente.fechanacimiento = Convert.ToDateTime(fechanacimiento);

                }
                paciente.telefono = telefono;
                paciente.telefono2 = telefono2;
                paciente.cedula = cedula;
                paciente.Estatus = 1;
                paciente.idciuddad = ciudad;
                paciente.Empresaid = empresaid1;
                paciente.Usuarioid = usuarioid1;
                paciente.Foto = Foto;
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                var idPaciente = paciente.idPaciente;
                mensaje = idPaciente.ToString();
                TempData["id"] = 5;
            }
            else
            {
                if (nombre != "")
                {
                    paciente.nombre = nombre.ToUpper();
                }
                paciente.fechanacimiento = Convert.ToDateTime(fechanacimiento);
                if (direccion != "")
                {
                    paciente.direccion = direccion.ToUpper();
                }
                if (sexo != null)
                {
                    paciente.sexo = sexo.ToUpper();
                }
                if (EstadoCivil !=null)
                {
                    paciente.EstadoCivil = EstadoCivil.ToUpper();
                }
                paciente.telefono = telefono;
                paciente.telefono2 = telefono2;
                paciente.cedula = cedula;
                if (fechanacimiento != null)
                {
                    var fech1 = Convert.ToDateTime(fechanacimiento);

                    DateTime now1 = DateTime.Today;
                    var ano1 = fech1.Year;
                    int edad1 = DateTime.Today.Year - ano1;

                    if (DateTime.Today < fech1.AddYears(edad1))
                    {
                        --edad1;
                    }
                    paciente.edad = edad1;
                    paciente.fechanacimiento = Convert.ToDateTime(fechanacimiento);
                }
                if (email != "")
                {
                    paciente.email = email.ToUpper();
                }
                paciente.Estatus = 1;
                paciente.idciuddad = ciudad;
                paciente.Empresaid = empresaid1;
                paciente.Usuarioid = usuarioid1;
                paciente.Foto = Foto;
                db.paciente.Add(paciente);
                db.SaveChanges();
                var idPaciente = paciente.idPaciente;
                mensaje = idPaciente.ToString();
                TempData["id"] = 5;
            }
            return Json(mensaje);
        }
        public ActionResult Historia(string consulta, string idpaciente,string idPacient, string antecedentesmedicos,string estatura,string centimetro,string peso,string peso1,string temperatura, string temperatura1, string nombre, string fechanacimiento,string gruposanguineo, string direccion, string telefono, string telefono2, string cedula, string idciuddad, string email,string sexo, string EstadoCivil, string antecedentesginecologico, string alergias, string Medicamentos, string Revision, string Enfermedad, string personales, string familiares, string Vacunas, string Habitos, string Foto) {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            paciente paciente = new paciente();
            HistoriaClinica HistoriaClinica = new HistoriaClinica();

            string mensaje = "";
            int idpacientebuscar = 0;
            int ciudad=0;
            if (idpaciente != "undefined")
            {
                idpacientebuscar = Convert.ToInt32(idpaciente);
            }
            else if (idPacient != "undefined")
            {
                idpacientebuscar = Convert.ToInt32(idPacient);
            }
            if (idciuddad != "undefined")
            {
                ciudad = Convert.ToInt32(idciuddad);
            }
            if (nombre == "" || fechanacimiento == "")
            {
                if (nombre == "") mensaje = "ERROR EN EL NOMBRE, INGRESE LOS DATOS DEL FORMULARIO DEL PACIENTE QUE SE ENCUENTRA MAS ARRIBA";
                if (fechanacimiento == "") mensaje = "ERROR EN LA FECHANACIMIENTO, INGRESE LOS DATOS DEL FORMULARIO DEL PACIENTE QUE SE ENCUENTRA MAS ARRIBA";



            }
            else
            {
                var t = (from s in db.paciente where s.idPaciente == idpacientebuscar && s.Estatus == 1 && s.Usuarioid==usuarioid1 select s.idPaciente).Count();
                if (t != 0)
                {
                    paciente paciente1 = new paciente();

                    var si = (from db in db.paciente where db.idPaciente == idpacientebuscar && db.Usuarioid==usuarioid1 select db.idPaciente).FirstOrDefault();
                    paciente.idPaciente = si;
                    if (direccion != "undefined")
                    {
                        paciente.direccion = direccion.ToUpper();
                    }
                    if (nombre != "undefined")
                    {
                        paciente.nombre = nombre.ToUpper();
                    }
                    if (email != "undefined")
                    {
                        paciente.email = email.ToUpper();
                    }
                    if (sexo != "undefined")
                    {
                        paciente.sexo = sexo.ToUpper();
                    }
                    if (EstadoCivil != "undefined")
                    {
                        paciente.EstadoCivil = EstadoCivil.ToUpper();
                    }
                    if (fechanacimiento != "undefined" || fechanacimiento != "")
                    {
                        var fech1 = Convert.ToDateTime(fechanacimiento);

                        DateTime now1 = DateTime.Today;
                        var ano1 = fech1.Year;
                        int edad1 = DateTime.Today.Year - ano1;

                        if (DateTime.Today < fech1.AddYears(edad1))
                        {
                            --edad1;
                        }
                        paciente.edad = edad1;
                        paciente.fechanacimiento = Convert.ToDateTime(fechanacimiento);
                    }
                    paciente.telefono = telefono;
                    paciente.telefono2 = telefono2;
                    paciente.cedula = cedula;
                    paciente.Estatus = 1;
                    paciente.idciuddad = ciudad;
                    paciente.Empresaid = empresaid1;
                    paciente.Usuarioid = usuarioid1;
                    paciente.Foto = Foto;
                    db.Entry(paciente).State = EntityState.Modified;
                    db.SaveChanges();
                    var comprobar = (from s in db.HistoriaClinica where s.idpaciente == idpacientebuscar && s.Estatus == 1 && s.Usuarioid==usuarioid1 select s.idHistorio).Count();
                    if (comprobar != 0) {
                        var comprobar1 = (from s in db.HistoriaClinica where s.idpaciente == idpacientebuscar && s.Estatus == 1 && s.Usuarioid==usuarioid1 select s.idHistorio).FirstOrDefault();

                        HistoriaClinica histroiaclicnica = new HistoriaClinica();


                        HistoriaClinica.idHistorio = comprobar1;
                        HistoriaClinica.AntecedentesMedicos = antecedentesmedicos;
                        HistoriaClinica.antecedentesGinecologico = antecedentesginecologico;
                        HistoriaClinica.idpaciente = idpacientebuscar;
                        HistoriaClinica.alergia = alergias;
                        HistoriaClinica.medicamentos = Medicamentos;
                        HistoriaClinica.Revisionporsistema = Revision;
                        HistoriaClinica.motivoconsulta = consulta;
                        HistoriaClinica.historia = Enfermedad;
                        HistoriaClinica.antsociales = personales;
                        HistoriaClinica.antecedentesfamiliares = familiares;
                        HistoriaClinica.habitos = Habitos;
                        HistoriaClinica.estaturas = estatura;
                        HistoriaClinica.unidad = centimetro;
                        HistoriaClinica.peso = peso;
                        HistoriaClinica.cantidadpeso = peso1;
                        HistoriaClinica.medidatemperatura = temperatura1;
                        HistoriaClinica.temperatura = temperatura;
                        HistoriaClinica.Estatus = 1;
                        HistoriaClinica.vacunas = Vacunas;
                        HistoriaClinica.gruposanguineo = gruposanguineo;

                        if (fechanacimiento != null || fechanacimiento != "")
                        {
                            HistoriaClinica.fecnacimiento = Convert.ToDateTime(fechanacimiento);
                        }
                        if (gruposanguineo != null)
                        {
                            HistoriaClinica.gruposanguineo = gruposanguineo;
                        }
                        HistoriaClinica.Empresaid = empresaid1;
                        HistoriaClinica.Usuarioid = usuarioid1;
                        db.Entry(HistoriaClinica).State = EntityState.Modified;
                        db.SaveChanges();
                        mensaje = "Guardado";
                    }
                    else if (comprobar <=0) {
                        HistoriaClinica histroiaclicnica = new HistoriaClinica(); ;
                        histroiaclicnica.AntecedentesMedicos = antecedentesmedicos;
                        histroiaclicnica.antecedentesGinecologico = antecedentesginecologico;
                        histroiaclicnica.idpaciente = idpacientebuscar;
                        histroiaclicnica.alergia = alergias;
                        histroiaclicnica.medicamentos = Medicamentos;
                        histroiaclicnica.Revisionporsistema = Revision;
                        histroiaclicnica.motivoconsulta = consulta;
                        histroiaclicnica.historia = Enfermedad;
                        histroiaclicnica.antsociales = personales;
                        histroiaclicnica.antecedentesfamiliares = familiares;
                        histroiaclicnica.habitos = Habitos;
                        histroiaclicnica.estaturas = estatura;
                        histroiaclicnica.unidad = centimetro;
                        histroiaclicnica.peso = peso;
                        histroiaclicnica.cantidadpeso = peso1;
                        histroiaclicnica.medidatemperatura = temperatura1;
                        histroiaclicnica.temperatura = temperatura;
                        histroiaclicnica.Estatus = 1;
                        histroiaclicnica.gruposanguineo = gruposanguineo;
                        if (fechanacimiento != null || fechanacimiento != "")
                        {
                            histroiaclicnica.fecnacimiento = Convert.ToDateTime(fechanacimiento);
                        }
                        if (gruposanguineo != null)
                        {
                            histroiaclicnica.gruposanguineo = gruposanguineo;
                        }
                        histroiaclicnica.Empresaid = empresaid1;
                        histroiaclicnica.Usuarioid = usuarioid1;
                        db.HistoriaClinica.Add(histroiaclicnica);

                        db.SaveChanges();
                        mensaje = "Guardado";

                    }
                }
                else {
                    if (nombre != null || nombre != "")
                    {
                        paciente.nombre = nombre.ToUpper();
                    }
                    if (fechanacimiento != null || fechanacimiento != "")
                    {
                        var fech = Convert.ToDateTime(fechanacimiento);
                        DateTime now = DateTime.Today;
                        var ano = fech.Year;
                        int edad = DateTime.Today.Year - ano;

                        if (DateTime.Today < fech.AddYears(edad))
                        {
                            --edad;
                        }
                        paciente.edad = edad; paciente.fechanacimiento = Convert.ToDateTime(fechanacimiento);
                    }
                    if (direccion != null || direccion != "") {
                        paciente.direccion = direccion.ToUpper();

                    }
                    paciente.telefono = telefono;
                    paciente.telefono2 = telefono2;
                    paciente.cedula = cedula;

                    if (email != null || email != "")
                    {
                        paciente.email = email.ToUpper();
                    }
                    paciente.Estatus = 1;
                    paciente.idciuddad = ciudad;
                    paciente.Empresaid = empresaid1;
                    paciente.Usuarioid = usuarioid1;
                    paciente.Foto = Foto;
                    if (sexo!="undefinded") {
                        paciente.sexo = sexo.ToUpper();
                    }
                    if (EstadoCivil != "undefinded")
                    {
                        paciente.EstadoCivil = EstadoCivil.ToUpper();
                    }
                    db.paciente.Add(paciente);
                    db.SaveChanges();
                    var id = paciente.idPaciente;
                    //var s = (from datos in db.paciente where datos.idPaciente == idpacientebuscar select datos).FirstOrDefault();
                    //if (s != null)
                    //{
                    HistoriaClinica histroiaclicnica = new HistoriaClinica(); ;
                    histroiaclicnica.AntecedentesMedicos = antecedentesmedicos;
                    histroiaclicnica.antecedentesGinecologico = antecedentesginecologico;
                    histroiaclicnica.idpaciente = id;
                    histroiaclicnica.alergia = alergias;
                    histroiaclicnica.medicamentos = Medicamentos;
                    histroiaclicnica.Revisionporsistema = Revision;
                    histroiaclicnica.motivoconsulta = consulta;
                    histroiaclicnica.historia = Enfermedad;
                    histroiaclicnica.antsociales = personales;
                    histroiaclicnica.antecedentesfamiliares = familiares;
                    histroiaclicnica.habitos = Habitos;
                    histroiaclicnica.estaturas = estatura;
                    histroiaclicnica.unidad = centimetro;
                    histroiaclicnica.peso = peso;
                    histroiaclicnica.cantidadpeso = peso1;
                    histroiaclicnica.medidatemperatura = temperatura1;
                    histroiaclicnica.temperatura = temperatura;
                    histroiaclicnica.Estatus = 1;
                    histroiaclicnica.vacunas = Vacunas;
                    histroiaclicnica.gruposanguineo = gruposanguineo;
                    histroiaclicnica.Empresaid = empresaid1;
                    histroiaclicnica.Usuarioid = usuarioid1;
                    db.HistoriaClinica.Add(histroiaclicnica);

                    db.SaveChanges();


                    mensaje = "PACIENTE E HISTORIA CLÍNICA GUARDADA CON EXITO...";
                    //}
                    //else
                    //{
                    //    mensaje = "ERROR INGRESE LOS DATOS DEL PACIENTE EN EL FORMULARIO QUE ESTÁ MÁS ARRIBA...";

                    //}
                }
            }
                return Json(mensaje);
            }

    

    // GET: pacientes/Delete/5
    public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paciente paciente = db.paciente.Find(id);
            paciente.Estatus = 0;
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
