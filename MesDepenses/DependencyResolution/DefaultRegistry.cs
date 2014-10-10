// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MesDepensesServices.DAL;
using StructureMap.Web;

namespace MesDepenses.DependencyResolution {
    using MesDepensesServices.Domain;
    using MesDepensesServices.Services;
    using Repository.Pattern.DataContext;
    using Repository.Pattern.Ef6;
    using Repository.Pattern.Ef6.Factories;
    using Repository.Pattern.Repositories;
    using Repository.Pattern.UnitOfWork;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Pipeline;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<CategorieService>();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                    scan.ExcludeType<RepositoryFactories>();
                });
            For<IUnitOfWorkAsync>().HybridHttpOrThreadLocalScoped().Use<UnitOfWork>();
            For<IDataContextAsync>().HybridHttpOrThreadLocalScoped().Use<MesdepensesContext>();
            For<IRepositoryAsync<Categorie>>().HybridHttpOrThreadLocalScoped().Use<Repository<Categorie>>();
            For<IRepositoryProvider>().HybridHttpOrThreadLocalScoped().Use<RepositoryProvider>().Ctor<RepositoryFactories>().Is(new RepositoryFactories());
        }

        #endregion
    }
}