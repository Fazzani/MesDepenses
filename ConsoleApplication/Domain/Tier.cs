using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesDepensesServices.Domain
{
    public class Tier
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        //public virtual ICollection<Categorie> Categories { get; set; }
    }
}
