using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MesDepensesServices.Domain
{
    public class BaseDomain
    {
        [Key]
        public int Id { get; set; }
    }
}
