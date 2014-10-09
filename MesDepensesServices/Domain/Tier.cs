using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesDepensesServices.Domain
{
    public class Tier : BaseDomain
    {
        [Required]
        [StringLength(100)]
        public string Libelle { get; set; }
        public virtual ICollection<Categorie> Categories { get; set; }
    }
}
