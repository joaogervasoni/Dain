using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Dain.DAL;
using Dain.Models;

namespace Dain.Controllers
{
    [RoutePrefix("api/DainAPI")]
    public class DainAPIController : ApiController
    {
        #region Pub API

        // GET: api/DainAPI/Pubs
        [Route("Pubs")]
        public IQueryable<Pub> GetPubs()
        {
            return PubDAO.ReturnList().AsQueryable();
        }

        // GET: api/DainAPI/PubById/1
        [Route("PubById/{pubId}")]
        [ResponseType(typeof(Pub))]
        public IHttpActionResult GetPubById(int pubId)
        {
            Pub pub = PubDAO.Search(pubId);
            if (pub == null)
            { return NotFound(); }

            return Ok(pub);
        }

        #endregion

        #region #region Person API

        // GET: api/DainAPI/Persons
        [Route("Persons")]
        public IQueryable<Person> GetPersons()
        {
            return PersonDAO.ReturnList().AsQueryable();
        }

        // GET: api/DainAPI/PersonById/1
        [Route("personById/{personId}")]
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPersonById(int personId)
        {
            Person person = PersonDAO.Search(personId);
            if (person == null)
            { return NotFound(); }

            return Ok(person);
        }

        #endregion

        #region #region Product API

        // GET: api/DainAPI/Products
        [Route("Products")]
        public IQueryable<Product> GetProducts()
        {
            return ProductDAO.ReturnList().AsQueryable();
        }

        // GET: api/DainAPI/ProductsById/1
        [Route("ProductsById/{productId}")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProductById(int productId)
        {
            Product product = ProductDAO.Search(productId);
            if (product == null)
            { return NotFound(); }

            return Ok(product);
        }

        // GET: api/DainAPI/ProductsByPubId/1
        [Route("ProductsByPubId/{pubId}")]
        [ResponseType(typeof(List<Product>))]
        public IHttpActionResult GetProductsByPubId(int pubId)
        {
            List<Product> productsList = ProductDAO.ReturnList(pubId);
            if (productsList == null)
            { return NotFound(); }

            return Ok(productsList);
        }

        #endregion
    }
}