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
using MesDepensesServices.Services;
using Repository.Pattern.UnitOfWork;

namespace MesDepenses.Controllers
{
    public class CategorieController : ApiController
    {
        private readonly CategorieService _categorieService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CategorieController(CategorieService categorieService, IUnitOfWorkAsync unitOfWork)
        {
            _categorieService = categorieService;
            _unitOfWork = unitOfWork;
        }

        [EnableQuery,HttpGet]
        public IQueryable<Categorie> Get()
        {
            return _categorieService.Queryable();
        }

        // GET api/<controller>/5
        [ResponseType(typeof(Categorie)),HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var categorie = await _categorieService.Queryable().FirstOrDefaultAsync(x => x.Id == id);
            if (categorie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Ok(_categorieService.Queryable().FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]Categorie categorie)
        {
            _categorieService.Insert(categorie);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return Created(Request.RequestUri + categorie.Id.ToString(), categorie);
            return InternalServerError();
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody]Categorie cat)
        {
            var categorie = await _categorieService.Queryable().FirstOrDefaultAsync(x => x.Id == id);
            if (categorie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            categorie.Libelle = cat.Libelle;
            categorie.CategorieParent = cat.CategorieParent;
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_categorieService.Queryable().Any(x => x.Id == id))
            {
                await _categorieService.DeleteAsync(id);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                    return Ok();
                return InternalServerError();
            }
            return NotFound();
        }
    }
}