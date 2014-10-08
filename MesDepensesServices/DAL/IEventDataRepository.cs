using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesDepensesServices.Domain;

namespace MesDepensesServices.DAL
{
    interface IEventDataRepository: IDisposable
    {
        IEnumerable<Categorie> ListCategories();
        IEnumerable<Tier> ListTiers();
        IEnumerable<Operation> ListOperations();
    }
}
