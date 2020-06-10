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
    public class empresasController : ApiController
    {
        private serviEntities db = new serviEntities();

        // GET: api/empresas
        public ICollection<empresa> Getempresa(Guid guid)
        {
            return db.usuario
                       .Where(u => u.guid == guid)
                       .FirstOrDefault<usuario>().empresa;
        }

        /*
        // GET: api/empresas/5
        [ResponseType(typeof(empresa))]
        public IHttpActionResult Getempresa(int id)
        {
            empresa empresa = db.empresa.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }
        */

        // PUT: api/empresas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putempresa(Guid guid, empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (guid != empresa.guid)
            {
                return BadRequest();
            }

            db.Entry(empresa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!empresaExists(guid))
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

        // POST: api/empresas
        [ResponseType(typeof(empresa))]
        public IHttpActionResult Postempresa(empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.empresa.Add(empresa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empresa.id_empresa }, empresa);
        }

        // DELETE: api/empresas/5
        [ResponseType(typeof(empresa))]
        public IHttpActionResult Deleteempresa(Guid guid)
        {
            empresa empresa = db.empresa.Where(u => u.guid == guid).FirstOrDefault<empresa>();

            //empresa empresa = db.empresa.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            db.empresa.Remove(empresa);
            db.SaveChanges();

            return Ok(empresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool empresaExists(Guid guid)
        {
            //return db.empresa.Count(e => e.id_empresa == id) > 0;
            return db.empresa.Count(e => e.guid == guid) > 0;
        }
    }
}