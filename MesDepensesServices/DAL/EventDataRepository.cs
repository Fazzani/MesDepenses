using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesDepensesServices.Domain;

namespace MesDepensesServices.DAL
{
    //[LifecycleTransient]
    public class EventDataRepository : IEventDataRepository 
    {
        private readonly IMesdepensesContext _context;

        public EventDataRepository(IMesdepensesContext context)
        {
            _context = context;
        }

        #region generic functions

        public IQueryable<T> ListTiers<T>() where T: BaseDomain
        {
            return _context.Tiers;
        }

        #endregion

        public IQueryable<Domain.Tier> ListTiers()
        {
            return _context.Tiers;
        }

        public IQueryable<Domain.Operation> ListOperations()
        {
            return _context.Operations;
        }

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
         {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return SaveChanges();
        }

        #region Categorie
        public void AddCategorie(Domain.Categorie categorie)
        {
            _context.Categories.Add(categorie);
        }
        public IQueryable<Domain.Categorie> ListCategories()
        {

            return _context.Categories;
        }

        #endregion
    }
}
