using ServiAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ServiAPI.Controllers
{
    [Authorize]
    public class tiempoController : ApiController
    {
        private serviEntities db = new serviEntities();

        // GET: api/tiempoes
        public ICollection<tiempo> Gettiempo(Guid guid)
        {
            return db.usuario
                      .Where(u => u.guid == guid)
                      .FirstOrDefault<usuario>().tiempo;
        }

        /*
        // GET: api/tiempoes/5
        [ResponseType(typeof(tiempo))]
        public IHttpActionResult Gettiempo(int id)
        {
            tiempo tiempo = db.tiempo.Find(id);
            if (tiempo == null)
            {
                return NotFound();
            }

            return Ok(tiempo);
        }
        */

        // PUT: api/tiempoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttiempo(Guid guid, tiempo tiempo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (guid != tiempo.guid)
            {
                return BadRequest();
            }

            db.Entry(tiempo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tiempoExists(guid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tiempoes
        [ResponseType(typeof(tiempo))]
        public IHttpActionResult Posttiempo(tiempo tiempo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tiempo.Add(tiempo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tiempo.id_tiempo }, tiempo);
        }

        // DELETE: api/tiempoes/5
        [ResponseType(typeof(tiempo))]
        public IHttpActionResult Deletetiempo(Guid guid)
        {
            tiempo tiempo = db.tiempo.Where(u => u.guid == guid).FirstOrDefault<tiempo>();

            //tiempo tiempo = db.tiempo.Find(id);
            if (tiempo == null)
            {
                return NotFound();
            }

            db.tiempo.Remove(tiempo);
            db.SaveChanges();

            return Ok(tiempo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tiempoExists(Guid guid)
        {
            //return db.tiempo.Count(e => e.id_tiempo == id) > 0;
            return db.tiempo.Count(e => e.guid == guid) > 0;
        }
    }
}