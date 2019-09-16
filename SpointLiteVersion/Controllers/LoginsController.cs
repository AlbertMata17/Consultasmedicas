using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;

namespace RentCar.Controllers
{
    public class LoginsController : Controller
    {
        private ConsultaMedicasEntities db = new ConsultaMedicasEntities();

        // GET: Logins
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }

            return View(db.Login.ToList());
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // GET: Logins
        [ActionName("Login")]
        public ActionResult Login()
        {
            ViewBag.error = TempData["error"];

            return View();
        }
        [HttpPost]
        public ActionResult Verify(string Username, string Password)
        {
            var a = from Login in db.Login.Where(x => x.Username == Username && x.Password == Password ) select Login;
            var p = from Login in db.Login select Login.Privilegio;
            var nombre = (from s in db.Login where s.Username==Username && s.Password==Password select s.Nombre).FirstOrDefault();
            var apellido = (from s in db.Login where s.Username == Username && s.Password == Password select s.Apellido).FirstOrDefault();
            var userid = (from s in db.Login where s.Username == Username && s.Password == Password select s.LoginId).FirstOrDefault();
            var empresaid = (from s in db.Login where s.Username == Username && s.Password == Password select s.empresaid).FirstOrDefault();


            if (a.Any())
            {
                Session["Username"] = Username;
                Session["nombre"] = nombre;
                Session["apellido"] = apellido;
                Session["userid"] = userid;
                Session["empresaid"] = empresaid;


                return RedirectToAction("Index", "pacientes");
            }
            else
            {
                TempData["error"] = "Usuario o Contraseña Incorrectos";
                return RedirectToAction("Login");
            }

        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session.Abandon();

            return RedirectToAction("Login");
        }

        // POST: Logins/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginId,Username,Password,Privilegio")] Login login)
        {

            if (ModelState.IsValid)
            {
                db.Login.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(login);
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginId,Username,Password,Privilegio")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(login);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Login.Find(id);
            db.Login.Remove(login);
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
