using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using MesDepensesServices.DAL;
using MesDepensesServices.Domain;

namespace MesDepenses.Controllers
{
    public class CategorieController : ApiController
    {
        private readonly IEventDataRepository _repository;

        public CategorieController(IEventDataRepository repository)
        {
            _repository = repository;
        }

        [EnableQuery]
        public IQueryable<Categorie> Get()
        {
                return _repository.ListCategories();
        }

        // GET api/<controller>/5
        [ResponseType(typeof(Categorie))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var categorie = await _repository.ListCategories().FirstOrDefaultAsync(x => x.Id == id);
                if (categorie == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return Ok(_repository.ListCategories().FirstOrDefault(x => x.Id == id));
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody]Categorie categorie)
        {
            _repository.ListCategories().Add(categorie);
            if (await _repository.SaveChangesAsync() > 0)
                    return Created(Request.RequestUri + categorie.Id.ToString(), categorie);
            return InternalServerError();
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]Categorie cat)
        {
            var categorie = _repository.ListCategories().FirstOrDefault(x => x.Id == id);
                if (categorie == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                categorie.Libelle = cat.Libelle;
                categorie.CategorieParent = cat.CategorieParent;
                await _repository.SaveChangesAsync();
                return Ok();
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_repository.ListCategories().Any(x => x.Id == id))
                {
                    _repository.ListCategories().Remove(_repository.ListCategories().FirstOrDefault(x => x.Id == id));
                    if (await _repository.SaveChangesAsync() > 0)
                        return Ok();
                    return InternalServerError();
                }
                return NotFound();
        }
    }
}