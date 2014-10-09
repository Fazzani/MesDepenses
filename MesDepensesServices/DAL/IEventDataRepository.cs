using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesDepensesServices.Domain;

namespace MesDepensesServices.DAL
{
    public interface IEventDataRepository : IDisposable
    {
        IQueryable<Categorie> ListCategories();
        IQueryable<Tier> ListTiers();
        IQueryable<Operation> ListOperations();
        void AddCategorie(Categorie categorie);
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
