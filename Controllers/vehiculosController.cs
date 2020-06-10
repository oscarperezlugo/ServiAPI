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
    public class vehiculosController : ApiController
    {
        private serviEntities db = new serviEntities();

        // GET: api/vehiculoes
        public ICollection<vehiculo> Getvehiculo(Guid guid)
        {
            return db.usuario
                       .Where(u => u.guid == guid)
                       .FirstOrDefault<usuario>().vehiculo;
        }

        /*
        // GET: api/vehiculoes/5
        [ResponseType(typeof(vehiculo))]
        public IHttpActionResult Getvehiculo(int id)
        {
            vehiculo vehiculo = db.vehiculo.Find(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return Ok(vehiculo);
        }
        */

        // PUT: api/vehiculoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putvehiculo(Guid guid, vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (guid != vehiculo.guid)
            {
                return BadRequest();
            }

            db.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vehiculoExists(guid))
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

        // POST: api/vehiculoes
        [ResponseType(typeof(vehiculo))]
        public IHttpActionResult Postvehiculo(vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vehiculo.Add(vehiculo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vehiculo.id_vehiculo }, vehiculo);
        }

        // DELETE: api/vehiculoes/5
        [ResponseType(typeof(vehiculo))]
        public IHttpActionResult Deletevehiculo(Guid guid)
        {
            vehiculo vehiculo = db.vehiculo.Where(u => u.guid == guid).FirstOrDefault<vehiculo>();

            //vehiculo vehiculo = db.vehiculo.Find(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            db.vehiculo.Remove(vehiculo);
            db.SaveChanges();

            return Ok(vehiculo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vehiculoExists(Guid guid)
        {
            //return db.vehiculo.Count(e => e.id_vehiculo == id) > 0;
            return db.vehiculo.Count(e => e.guid == guid) > 0;
        }
    }
}