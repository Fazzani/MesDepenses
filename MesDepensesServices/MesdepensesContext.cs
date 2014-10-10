using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MesDepensesServices.Domain;
using Repository.Pattern.Ef6;

namespace MesDepensesServices.DAL
{
    public interface IMesdepensesContext
    {
        DbSet<Operation> Operations { get; set; }
        DbSet<Categorie> Categories { get; set; }
        DbSet<Tier> Tiers { get; set; }
        void Dispose();
    }

    public class MesdepensesContext : DataContext, IMesdepensesContext
    {
        public MesdepensesContext()
            : base("MesdepensesContext")
        {
            Configuration.ProxyCreationEnabled = false;
            // Turn off the Migrations, (NOT a code first Db)
            //Database.SetInitializer<MesdepensesContext>(null);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MesdepensesContext>());
        }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Tier> Tiers { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // Database does not pluralize table names
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //   // modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        //}
        public void Dispose()
        {
            base.Dispose();
        }
    }
}
