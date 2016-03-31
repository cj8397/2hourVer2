using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class ProductController : Controller
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class ProductsController : ApiController
        {
            private mergedEntities db = new mergedEntities();

            // GET: api/Products
            public IQueryable<Product> GetProducts()
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Products;
            }

            // GET: api/Products/5
            [ResponseType(typeof(Product))]
            public IHttpActionResult GetProduct(int id)
            {
                // Disable lazy loading otherwise the REST service returns
                // all data in the database.
                db.Configuration.LazyLoadingEnabled = false;

                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }


            // PUT: api/Products/5
            [ResponseType(typeof(void))]
            public IHttpActionResult PutProduct(int id, Product product)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != product.productID)
                {
                    return BadRequest();
                }

                db.Entry(product).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
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

            // POST: api/Products
            [ResponseType(typeof(Product))]
            public IHttpActionResult PostProduct(Product product)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Products.Add(product);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (ProductExists(product.productID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = product.productID }, product);
            }

            // DELETE: api/Products/5
            [ResponseType(typeof(Product))]
            public IHttpActionResult DeleteProduct(int id)
            {
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return NotFound();
                }

                db.Products.Remove(product);
                db.SaveChanges();

                return Ok(product);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool ProductExists(int id)
            {
                return db.Products.Count(e => e.productID == id) > 0;
            }
        }
    }
}