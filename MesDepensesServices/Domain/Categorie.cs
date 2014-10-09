using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesDepensesServices.Domain
{
    public class Categorie : Entity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Libelle { get; set; }
        public Categorie CategorieParent { get; set; }

    }
}
