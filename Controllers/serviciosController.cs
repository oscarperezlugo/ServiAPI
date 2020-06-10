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
    public class serviciosController : ApiController
    {
        private serviEntities db = new serviEntities();

        // GET: api/servicios
        public ICollection<servicio> Getservicio(Guid guid)
        {
            return db.usuario
                    .Where(u => u.guid == guid)
                    .FirstOrDefault<usuario>().servicio;
        }

        /*
        // GET: api/servicios/5
        [ResponseType(typeof(servicio))]
        public IHttpActionResult Getservicio(int id)
        {
            servicio servicio = db.servicio.Find(id);
            if (servicio == null)
            {
                return NotFound();
            }

            return Ok(servicio);
        }
        */

        // PUT: api/servicios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putservicio(Guid guid, servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (guid != servicio.guid)
            {
                return BadRequest();
            }

            db.Entry(servicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!servicioExists(guid))
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

        // POST: api/servicios
        [ResponseType(typeof(servicio))]
        public IHttpActionResult Postservicio(servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.servicio.Add(servicio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = servicio.id_servicio }, servicio);
        }

        // DELETE: api/servicios/5
        [ResponseType(typeof(servicio))]
        public IHttpActionResult Deleteservicio(Guid guid)
        {
            servicio servicio = db.servicio.Where(u => u.guid == guid).FirstOrDefault<servicio>();

            //servicio servicio = db.servicio.Find(id);
            if (servicio == null)
            {
                return NotFound();
            }

            db.servicio.Remove(servicio);
            db.SaveChanges();

            return Ok(servicio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool servicioExists(Guid guid)
        {
            //return db.servicio.Count(e => e.id_servicio == id) > 0;
            return db.servicio.Count(e => e.guid == guid) > 0;
        }
    }
}