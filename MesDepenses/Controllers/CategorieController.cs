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

        [EnableQuery()]
        public IQueryable<Categorie> Get()
        {
            using (var repository = new MesdepensesContext())
            {
                return repository.Categories;
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(Categorie))]
        public async Task<IHttpActionResult> Get(int id)
        {
            using (var repository = new MesdepensesContext())
            {
                var categorie = await repository.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (categorie == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return Ok(repository.Categories.FirstOrDefault(x => x.Id == id));
            }
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody]Categorie categorie)
        {
            using (var repository = new MesdepensesContext())
            {
                repository.Categories.Add(categorie);
                if (await repository.SaveChangesAsync() > 0)
                    return Created(Request.RequestUri + categorie.Id.ToString(), categorie);
            }
            return InternalServerError();
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]Categorie cat)
        {
            using (var repository = new MesdepensesContext())
            {
                var categorie = repository.Categories.FirstOrDefault(x => x.Id == id);
                if (categorie == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                categorie.Libelle = cat.Libelle;
                categorie.CategorieParent = cat.CategorieParent;
                await repository.SaveChangesAsync();
                return Ok();
            }
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            using (var repository = new MesdepensesContext())
            {
                if (repository.Categories.Any(x => x.Id == id))
                {
                    repository.Categories.Remove(repository.Categories.FirstOrDefault(x => x.Id == id));
                    if (await repository.SaveChangesAsync() > 0)
                        return Ok();
                    return InternalServerError();
                }
                return NotFound();
            }
        }
    }
}