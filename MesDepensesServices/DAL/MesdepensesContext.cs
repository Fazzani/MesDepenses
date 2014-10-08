using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesDepensesServices.Domain;

namespace MesDepensesServices.DAL
{
    public class MesdepensesContext: DbContext
    {
        //public MesdepensesContext()
        // {
        //    // Turn off the Migrations, (NOT a code first Db)
        //     //Database.SetInitializer<MesdepensesContext>(null);
        //    // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MesdepensesContext>());
        // }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Tier> Tiers { get; set; }
 
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Database does not pluralize table names
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //   // modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        //}
    }
}
