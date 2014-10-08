using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesDepensesServices.DAL
{
    //[LifecycleTransient]
    public class EventDataRepository : IEventDataRepository 
    {
        private readonly MesdepensesContext _context;

        public EventDataRepository(MesdepensesContext context)
        {
            _context = context;
        }
        public IEnumerable<Domain.Categorie> ListCategories()
        {
            
            return _context.Categories;
        }

        public IEnumerable<Domain.Tier> ListTiers()
        {
            return _context.Tiers;
        }

        public IEnumerable<Domain.Operation> ListOperations()
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
    }
}
