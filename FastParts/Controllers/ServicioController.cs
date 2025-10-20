using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FastParts.Models;

namespace FastParts.Controllers
{
    public class ServicioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Servicio
        public ActionResult Index()
        {
            return View(db.ServicioModels.ToList());
        }

        // GET: Servicio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicioModel servicioModel = db.ServicioModels.Find(id);
            if (servicioModel == null)
            {
                return HttpNotFound();
            }
            return View(servicioModel);
        }

        // GET: Servicio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Servicio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdServicio,Nombre,Descripcion,PrecioServicio,Activo")] ServicioModel servicioModel)
        {
            if (ModelState.IsValid)
            {
                db.ServicioModels.Add(servicioModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicioModel);
        }

        // GET: Servicio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServicioModel servicioModel = db.ServicioModels.Find(id);
            if (servicioModel == null)
            {
                return HttpNotFound();
            }
            return View(servicioModel);
        }

        // POST: Servicio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdServicio,Nombre,Descripcion,PrecioServicio,Activo")] ServicioModel servicioModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicioModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicioModel);
        }

        // GET: Servicio/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ServicioModel servicioModel = db.ServicioModels.Find(id);
        //    if (servicioModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(servicioModel);
        //}

        //// POST: Servicio/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ServicioModel servicioModel = db.ServicioModels.Find(id);
        //    db.ServicioModels.Remove(servicioModel);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
